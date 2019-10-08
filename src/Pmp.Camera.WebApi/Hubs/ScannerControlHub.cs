using Microsoft.AspNetCore.SignalR;
using MisteriousMachine.COM;
using Pmp.Camera.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pmp.Camera.WebApi.Hubs
{
    public class Notifier : IDisposable
    {
        public Notifier(IHubContext<ScannerControlHub> hubContext, OiCamera camera, IUcScannerClient ucScannerClient)
        {
            this.Context = hubContext;
            this.Camera = camera;
            this.UcScannerClient = ucScannerClient;
            this.Camera.PropertyChanged += this.Camera_PropertyChanged;
            this.UcScannerClient.PropertyChanged += this.MachineController_PropertyChanged;
        }

        public IHubContext<ScannerControlHub> Context { get; }
        public OiCamera Camera { get; }
        public IUcScannerClient UcScannerClient { get; }

        private async void MachineController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is IUcScannerClient client && e.PropertyName == PropertyHelper.ExtractPropertyName(() => client.IsConnected))
            {
                try
                {
                    await this.Context.Clients.All.SendAsync("setMachineState", client.IsConnected);
                }
                catch (Exception ex)
                {
                }
            }
        }
        private async void Camera_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is OiCamera oiCamera && e.PropertyName == PropertyHelper.ExtractPropertyName(() => oiCamera.IsConnected))
            {
                try
                {
                    this.Context.Clients.All.SendAsync("setCameraState", oiCamera.IsConnected);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void Dispose()
        {
            if (this.Camera != null)
            {
                this.Camera.PropertyChanged -= this.Camera_PropertyChanged;
            }
            if (this.UcScannerClient != null)
            {
                this.UcScannerClient.PropertyChanged -= this.MachineController_PropertyChanged;
            }
        }
    }
    public class UcParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class ScannerControlHub : Hub, IDisposable
    {
        public OiCamera Camera { get; }
        public IUcScannerClient MachineController { get; }

        public ScannerControlHub(OiCamera camera, IUcScannerClient machineController, Notifier n)
        {
            this.Camera = camera;
            this.MachineController = machineController;
        }

        public async Task<string> Execute(string command)
        {
            var result = await this.Camera.Exec(command);
            return CommandsResponseReader.TryGetValue(result);
        }

        public async Task<string> ExecutePost(string command, string body)
        {
            var result = await this.Camera.ExecPost(command, body);
            return result;
        }

        public async Task StartRecord()
        {
            var result = await this.Camera.Record();
        }

        public async Task<ButtonDescription[]> ListSettings()
        {
            return await this.Camera.GetDefinition();
        }

        public async Task<ButtonWithParametersDescription[]> ListUcSettings()
        {
            var commands = this.MachineController.Commands.GetType().GetMethods();

            var result = new List<ButtonWithParametersDescription>();

            foreach (var item in commands)
            {
                result.Add(new ButtonWithParametersDescription
                {
                    Name = item.Name,
                    ParameterDescription = item.GetParameters().Select(x => new ParameterDescription() { Name = x.Name }).ToArray()
                });
            }
            return result.ToArray();
        }

        public async Task<string> InvokeUc(string name, UcParameter[] parameters)
        {
            var values = parameters.Concat(new[] { new UcParameter { Name = "action", Value = name } }).ToDictionary(x => x.Name, x => x.Value);
            var command = this.MachineController.Commands.Match(values);
            var result = this.MachineController.Invoke(command);
            return result;
        }

        public async Task SetLed(int port, bool value)
        {
            var result = this.MachineController.Invoke(x => x.Set(port, value));
            await this.Clients.All.SendAsync("setLed", port, value);
        }
    }
}
