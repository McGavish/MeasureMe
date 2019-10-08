import * as SignalR from '@aspnet/signalr'
import { observable, action } from 'mobx'
import { SettingItem, ButtonWithParameterDescription, ParameterDescription } from "./SettingItem";

export class MachineClient {
    client: SignalR.HubConnection;
    @observable.shallow
    ucItems: ButtonWithParameterDescription[] = [];
    constructor(public baseUrl: string) {
        this.settingItems = [];
        this.result = "";
        this.client = new SignalR.HubConnectionBuilder()
            .withUrl('http://localhost:3005/control')
            .configureLogging(SignalR.LogLevel.Debug)
            .build();

        this.client.onclose(() => {
            this.setCallback(false);
            this.setMachineStateCallback(false);
            for (let index = 0; index < 8; index++) {
                this.switches.set(index, false);
            }
        });

        this.switches = observable.map();

        this.client.start().then(async x => {
            await this.refreshUcItems();
            await this.refreshCameraItems();
        });

        this.client.on('setCameraState', this.setCallback);
        this.client.on('setMachineState', this.setMachineStateCallback);
        this.client.on('setLed', this.setLedCallback);
    }

    @action async refreshCameraItems() {
        const list = await this.client.invoke('listSettings') as SettingItem[];
        action(() => {
            this.settingItems = list.map(x => {
                const si = new SettingItem();
                si.getCommand = x.getCommand;
                si.isEnum = x.isEnum;
                si.setCommand = x.setCommand;
                si.value = x.value;
                si.name = x.name;
                si.possibleValues = x.possibleValues;
                si.client = this;
                return si;
            });

            const promises = this.settingItems.map(async x => {
                const value = await this.execute(x.getCommand);
                x.setValue(value);
            });

            Promise.all(promises);

        })();
    }

    @action async refreshUcItems() {
        const list = await this.client.invoke('listUcSettings') as ButtonWithParameterDescription[];
        action(() => {
            this.ucItems = list.map(x => {
                const buttonWithParameters = new ButtonWithParameterDescription();
                buttonWithParameters.client = this;
                buttonWithParameters.name = x.name;
                buttonWithParameters.parameterDescription = x.parameterDescription.map(x => {
                    const r = new ParameterDescription();
                    r.Value = x.Value;
                    r.name= x.name;
                    return r;
                })
                return buttonWithParameters;
            })
        })();
    }

    @observable.shallow
    settingItems: SettingItem[];

    @observable
    switches: Map<number, boolean>

    @observable
    result: string;

    public async record() {
        await this.client.invoke('startRecord');
        await this.refreshCameraItems()
    }

    @action
    public async execute(command: string) {
        const result = await this.client.invoke('execute', command);
        this.result = result;
        return result;
    }

    public async invokeUc(panel: ButtonWithParameterDescription) {
        await this.client.invoke('invokeUc', panel.name, panel.parameterDescription);
    }

    @action
    public async executePost(command: string, body: string) {
        const result = await this.client.invoke('executePost', command, body);
        this.result = result;
    }

    public async toggleLed(led: number) {
        const currentState = this.switches.get(led);
        await this.client.invoke('setLed', led, !currentState);
    }

    @action.bound
    public setLedCallback(this: MachineClient, port: number, value: boolean) {
        this.switches.set(port, value);
    }

    @action.bound
    public setCallback(this: MachineClient, b: boolean): void {
        this.isCameraConnected = b;
    }

    @action.bound
    public setMachineStateCallback(this: MachineClient, b: boolean): void {
        this.isMachineAvailable = b;
    }

    @observable
    public isCameraConnected = false;

    @observable
    public isMachineAvailable = false;

}
