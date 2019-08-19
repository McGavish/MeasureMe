import { observable, computed } from 'mobx';
export class WithCoordinates {
    @observable
    protected _x = 0;
    @observable
    protected _y = 0;
    @computed
    public get x() {
        return this._x;
    }
    public set x(value: number) {
        //console.log("x" + value);
        this._x = value;
    }
    @computed
    public get y() {
        return this._y;
    }
    public set y(value: number) {
        //console.log("y" + value);
        this._y = value;
    }
}