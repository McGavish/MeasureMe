import { observable, computed } from 'mobx';
import { WithCoordinates } from './WithCoordinates';
import { uuidv4 } from './XYZ';
import { ImageModel } from "./ImageModel";
export class MeasurePointModel extends WithCoordinates {
    guid: string;
    @observable.shallow
    parent: ImageModel;
    constructor(x: number, y: number, parent: ImageModel) {
        super();
        this._x = x;
        this._y = y;
        this.parent = parent;
        this.guid = uuidv4();
    }
    @computed
    get x() {
        return this._x + this.parent.x;
    }
    @computed
    get y() {
        return this._y + this.parent.y;
    }
}