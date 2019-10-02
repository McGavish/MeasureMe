import { observable, action } from 'mobx';
import { WithCoordinates } from './WithCoordinates';
import { MeasurePointModel } from './MeasurePointModel';
import { MouseModel } from './MouseModel';
export class ImageModel extends WithCoordinates {
     
    @observable.shallow
    mouse: MouseModel;

    @observable.shallow
    measuredPoints: MeasurePointModel[] = [];
    
    @observable
    image: any;
    
    constructor(src: string) {
        super();
        const image = new Image();
        image.src = src;
        this.image = image;
        this.mouse = new MouseModel(); // wyowanie konstruktowa mouse model i przypisanie go do this.mouse
    }

    setMousePosition(x: number, y: number){
        this.mouse.x = x;
        this.mouse.y = y;
    }

    @action
    setPoint(x: number, y: number) {
        const point = new MeasurePointModel(x, y, this);
        this.measuredPoints.push(point);
    }
}