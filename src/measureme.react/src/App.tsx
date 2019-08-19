import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import { Stage, Layer, Rect, Text, Image, Circle } from 'react-konva';
import Konva from 'konva';
import { Star } from 'react-konva';
import { observer } from 'mobx-react'
import { CanvasModel } from "./Models/CanvasModel";
import { ImageModel } from "./Models/ImageModel";

@observer
export default class App extends Component<{ model: CanvasModel }>{
  render() {
    const model = this.props.model;
    const image = this.props.model.image;

    return (
      <div className="App">
        <div>
          <div>
            <input type="file" onChange={x => x.currentTarget && x.currentTarget.files && this.props.model.loadImage(x.currentTarget.files[0])} />
          </div>
        </div>
        <div>
          <Stage width={1000} height={500}
            scaleX={this.props.model.zoom}
            scaleY={this.props.model.zoom}
            onWheel={e => {
              e.evt.preventDefault();

              const scaleBy = 1.15;
              const oldScale = this.props.model._zoom;
              const stage = e.target.getStage();
              if (!stage) {
                console.warn("error!")
                return;
              }
              const newScale = e.evt.deltaY > 0 ? oldScale / scaleBy : oldScale * scaleBy;

              this.props.model.zoom = newScale
            }
            }>

            <Layer>

              <Image
                image={this.props.model.image.image}
                x={this.props.model.image.x}
                y={this.props.model.image.y}
                numPoints={5}
                innerRadius={10}
                outerRadius={20}
                fill="#89b717"
                opacity={1}
                draggable
                shadowColor="black"
                shadowBlur={10}
                shadowOpacity={0.6}

                onDragEnd={(x) => {
                  this.props.model.image.x = x.currentTarget.x();
                  this.props.model.image.y = x.currentTarget.y();
                }}
                onDragMove={(x) => {
                  this.props.model.image.x = x.currentTarget.x();
                  this.props.model.image.y = x.currentTarget.y();
                }}
                onClick={(x) => {
                  const stage = x.target.getStage();
                  if (stage) {
                    const pos = stage.getPointerPosition();
                    this.props.model.image.setPoint(1 / model.zoom * pos.x - image.x, 1 / model.zoom * pos.y - image.y) // wywołanie metody setPoint
                  }

                }}
              >
              </Image>
            </Layer>
            <Layer >
              {this.props.model.image.measuredPoints.map(x => <Circle
                x={x.x}
                y={x.y}
                key={x.guid}
                radius={5}
                fill="red"
                scaleX={this.props.model.zoom}
                scaleY={this.props.model.zoom}
                shadowBlur={5}

              />)}
            </Layer>
          </Stage></div>
        <div>
          <div>
            <h2>Zoom: {this.props.model.zoom}</h2>

          </div>
          <div>
            <h2>Image Coordinate:</h2>
            <h2>x = {this.props.model.image.x}</h2>
            <h2>y = {this.props.model.image.y}</h2>

          </div>
        </div>
      </div>
    );
  }
}
