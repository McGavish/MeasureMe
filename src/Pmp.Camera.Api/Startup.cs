using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MisteriousMachine.COM;
using Pmp.Camera.Lib;

namespace Pmp.Camera.Api
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<OiCamera>((x) =>
            {
                var camera = OiCameraBootstraper.Connect();
                var result = camera.Result;
                result.Record();
                return result;
            });
            services.AddSingleton<IReceiverProducer<object, RawFrame>, OiUdpStreamBlock>();
            //Func<IServiceProvider, Task<IProducer<byte[]>>> p = async (isp) =>
            //     {
            //         //var camera = await OiCameraBootstraper.Connect();
            //         //var endpoint = await camera.Record();
            //         //var receiver = new OiPacketProducer(endpoint); //new FakePacketProducer(new DirectoryInfo(@"D:\datas\78fa3913-660f-469b-8957-9adb4e6a6226"));
            //         //var pipeline = new Pipeline(receiver);
            //         //var producer = await pipeline.Start(CancellationToken.None);
            //         //return producer;
            //     };
            //services.AddSingleton(typeof(Task<IProducer<byte[]>>), p);

            services.AddSingleton<SerialPortDescriptor>(s => UcClient.GetFirstConnected());
            //services.AddSingleton<UcClient>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:44386", "http://localhost:3000");
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "measureme.react\\build")),
                RequestPath = ""

            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "measureme.react\\build")),
                RequestPath = "/build"
            });
            app.UseMvc();
            app.UseSignalR(x => x.MapHub<Hubs.ScannerControlHub>("/control"));
        }
    }
}
