import * as React from "react";
import { Stage, Layer, Image, Circle } from "react-konva";
import { CanvasModel } from "./Models/CanvasModel";
import { InjectedComponent } from "./InjectedComponent";
import { observer, inject } from "mobx-react";
import { Col, Row, Statistic, Card, Upload, Icon, Button } from "antd";

@inject("canvas")
@observer
export class MeasureApp extends InjectedComponent<{
  canvas: CanvasModel;
}> {
  render() {
    const model = this.injectedProps.canvas;
    const image = this.injectedProps.canvas.image;
    return (
      <div>
        <div>
          <Stage
            width={1000}
            height={500}
            scaleX={this.injectedProps.canvas.zoom}
            scaleY={this.injectedProps.canvas.zoom}
            onWheel={e => {
              e.evt.preventDefault();
              const scaleBy = 1.15;
              const oldScale = this.injectedProps.canvas._zoom;
              const stage = e.target.getStage();
              if (!stage) {
                console.warn("error!");
                return;
              }
              const newScale =
                e.evt.deltaY > 0 ? oldScale / scaleBy : oldScale * scaleBy;
              this.injectedProps.canvas.zoom = newScale;
            }}
          >
            <Layer>
              <Image
                image={this.injectedProps.canvas.image.image}
                x={this.injectedProps.canvas.image.x}
                y={this.injectedProps.canvas.image.y}
                numPoints={5}
                innerRadius={10}
                outerRadius={20}
                fill="#89b717"
                opacity={1}
                draggable
                shadowColor="black"
                shadowBlur={10}
                shadowOpacity={0.6}
                onDragEnd={x => {
                  this.injectedProps.canvas.image.x = x.currentTarget.x();
                  this.injectedProps.canvas.image.y = x.currentTarget.y();
                }}
                onDragMove={x => {
                  this.injectedProps.canvas.image.x = x.currentTarget.x();
                  this.injectedProps.canvas.image.y = x.currentTarget.y();
                }}
                onClick={x => {
                  const stage = x.target.getStage();
                  if (stage) {
                    const pos = stage.getPointerPosition();
                    this.injectedProps.canvas.image.setPoint(
                      (1 / model.zoom) * pos.x - image.x,
                      (1 / model.zoom) * pos.y - image.y
                    ); // wywołanie metody setPoint
                  }
                }}
                onMouseMove={n => {
                  const stage = n.target.getStage();
                  if (stage) {
                    const pos = stage.getPointerPosition();
                    this.injectedProps.canvas.image.setMousePosition(
                      (1 / model.zoom) * pos.x - image.x,
                      (1 / model.zoom) * pos.y - image.y
                    );
                  }
                }}
              ></Image>
            </Layer>
            <Layer>
              {this.injectedProps.canvas.image.measuredPoints.map(x => (
                <Circle
                  x={x.x}
                  y={x.y}
                  key={x.guid}
                  radius={5}
                  fill="red"
                  scaleX={this.injectedProps.canvas.zoom}
                  scaleY={this.injectedProps.canvas.zoom}
                  shadowBlur={5}
                />
              ))}
            </Layer>
          </Stage>
        </div>
        <Row style={{ paddingTop: "20px" }} gutter={16}>
          <Col span={4}>
            <Upload
              name="file"
              onChange={x =>
                x &&
                x.file &&
                this.injectedProps.canvas.loadImage(x.file
                  .originFileObj as File)
              }
            >
              <Button>
                <Icon type="upload" /> Click to Upload
              </Button>
            </Upload>
          </Col>
          <Col span={4}>
            <Card>
              <Statistic
                title="Zoom"
                value={this.injectedProps.canvas.zoom}
                precision={2}
                valueStyle={{ color: "#3f8600" }}
              />
            </Card>
          </Col>
          <Col span={4}>
            <Card>
              Image
              <Statistic
                title="X"
                value={this.injectedProps.canvas.image.x}
                precision={2}
                valueStyle={{ color: "#3f8600" }}
              />
              <Statistic
                title="Y"
                value={this.injectedProps.canvas.image.y}
                precision={2}
                valueStyle={{ color: "#3f8600" }}
              />
            </Card>
          </Col>
          <Col span={4}>
            <Card>
              Mouse
              <Statistic
                title="X"
                value={this.injectedProps.canvas.image.mouse.x}
                precision={2}
                valueStyle={{ color: "#3f8600" }}
              />
              <Statistic
                title="Y"
                value={this.injectedProps.canvas.image.mouse.y}
                precision={2}
                valueStyle={{ color: "#3f8600" }}
              />
            </Card>
          </Col>
        </Row>
      </div>
    );
  }
}
