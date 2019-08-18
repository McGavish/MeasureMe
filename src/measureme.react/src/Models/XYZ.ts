
import { observable, computed, action } from 'mobx'
import data from './data.json'
function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
  
export class MeasurePointModel {

    guid: string;

    @observable.shallow
    parent: ImageModel;

    constructor(x: number, y: number, parent: ImageModel){
        this.x = x;
        this.parent = parent;
        this.y = y;
        this.guid = uuidv4();
    }

    @observable
    _x = 0;

    @observable
    _y = 0;

    @computed
    get x() {
        return this._x + this.parent.x;
    }
    set x(value: number) {
        //console.log("x" + value);
        this._x = value;
    }

    @computed
    get y() {
        return this._y + this.parent.y;
    }
    set y(value: number) {
        //console.log("y" + value);
        this._y = value;
    }

}

export class ImageModel {

    @observable.shallow
    measuredPoints: MeasurePointModel[] = [];

    @observable
    image: any;

    @observable
    _x = 0;

    @observable
    _y = 0;

    @computed
    get x() {
        return this._x;
    }
    set x(value: number) {
        //console.log("x" + value);
        this._x = value;
    }

    @computed
    get y() {
        return this._y;
    }
    set y(value: number) {
        //console.log("y" + value);
        this._y = value;
    }

    constructor(src: string) {
        const image = new Image();
        image.src = src;
        console.log(image.src)
        this.image = image;
    }

    @action
    setPoint(x: number, y: number){
        const point = new MeasurePointModel(x, y, this)
        this.measuredPoints.push(point)
    }
}


export class CanvasModel {

    @observable.shallow
    image!: ImageModel;

    @observable
    _zoom = 1;

    get zoom() {
        return this._zoom;
    }

    set zoom(value: number) {
        this._zoom = value;
    }

    @action
    setImage(data: string | ArrayBuffer | null) {
        this.image = new ImageModel(data as string);
    }

    @action
    loadImage(file: File) {
        var fr = new FileReader();
        fr.onload = () => {
            if (fr) {
                this.setImage(fr.result);
            }
        }
        fr.readAsDataURL(file);
    }

    constructor() {
        this.setImage(data.img);
    }

}

