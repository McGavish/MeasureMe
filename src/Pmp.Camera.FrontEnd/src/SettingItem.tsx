import { observable, action } from 'mobx';
import { MachineClient } from './MachineClient';
export class SettingItem {
    possibleValues: string[] = [];
    isEnum: boolean = false;
    setCommand: string = '';
    name: string = '';
    @observable
    value: string = '';
    getCommand: string = '';
    client: MachineClient | null= null;

    @action async post(value: string)
    {
        if(this.client)
        {

        await this.client.executePost(this.setCommand, value);
        const v = await this.client.execute(this.getCommand);
        this.setValue(v);
    }
}

    @action setValue(v: string)
    {
        this.value = v;
    }
}

export class ButtonWithParameterDescription {
    public name: string = '';
    public parameterDescription: ParameterDescription[] = [];
    client: MachineClient | null = null;

    @action invoke()
    {
        if(this.client != null)
        {
            this.client.invokeUc(this);
        }
    }
}

export class ParameterDescription {
    public name: string = '';
    public value: string = '';
}