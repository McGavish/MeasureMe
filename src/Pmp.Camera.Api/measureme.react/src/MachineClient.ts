import * as SignalR from '@aspnet/signalr'
import { observable, action } from 'mobx'
import { SettingItem } from "./SettingItem";

export class MachineClient {
    client: SignalR.HubConnection;
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
            await this.refreshItems();
        });

        this.client.on('setCameraState', this.setCallback);
        this.client.on('setMachineState', this.setMachineStateCallback);
        this.client.on('setLed', this.setLedCallback);
    }

    @action async refreshItems() {
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
                return si;
            });

            const promises = this.settingItems.map(async x => {
                const value = await this.execute(x.getCommand);
                x.setValue(value);
            });

            Promise.all(promises);

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
    }

    @action
    public async execute(command: string) {
        const result = await this.client.invoke('execute', command);
        this.result = result;
        return result;
    }

    @action
    public async executePost(command: string, body: string) {
        debugger;
        const result = await this.client.invoke('executePost', command, body);
        await this.refreshItems();
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
