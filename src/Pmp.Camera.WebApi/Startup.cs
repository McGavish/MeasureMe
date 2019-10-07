using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MisteriousMachine.COM;
using Pmp.Camera.Lib;
using Pmp.Camera.WebApi.Hubs;

namespace Pmp.Camera.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<OiCamera>((x) =>
            {
                var camera = OiCameraBootstraper.Connect();
                var result = camera.Result;
                result.TryStart();
                return result;
            });
            services.AddSingleton<IReceiverProducer<object, RawFrame>, FakePacketProducer>();
            services.AddSingleton<SerialPortDescriptor>(s => UcClient.GetFirstConnected());
            services.AddSingleton(x =>
            {
                var spd = x.GetService<SerialPortDescriptor>();
                var result = default(IUcScannerClient);
                if (spd != null)
                {
                    result = new UcClient(spd);
                }
                else
                {
                    result = new FakeUcClient();
                }
                result.TryStartAsync();
                return result;
            });
            services.AddSingleton<Notifier>();
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3001")
                    .WithOrigins("http://localhost:3000")
                    ;
            }));
            services.AddSignalR();
            services.AddControllers();
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ScannerControlHub>("/control");
            });
        }
    }
}
