import { observable, action } from 'mobx';
import data from './data.json';
import { ImageModel } from './ImageModel';
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
        };
        fr.readAsDataURL(file);
    }

    constructor() {
        this.setImage(data.img);
    }
}