import { observable, action } from 'mobx';
export class SettingItem {
    possibleValues: string[] = [];
    isEnum: boolean = false;
    setCommand: string = '';
    name: string = '';
    @observable
    value: string = '';
    getCommand: string = '';

    @action setValue(v: string)
    {
        this.value = v;
    }
}
