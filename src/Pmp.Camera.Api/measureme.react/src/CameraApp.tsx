import * as React from 'react'
import { Component } from 'react';
import { observer, inject } from 'mobx-react';
import { InjectedComponent } from './InjectedComponent';
import { AppProps } from './App';
import { Row, Col, Card, Icon, Button, Input } from 'antd';
import { ButtonProps } from 'antd/lib/button';
import './CameraApp.css';
import TextArea from 'antd/lib/input/TextArea';

@inject('canvas', 'routing', 'machineClient')
@observer
export class CameraApp extends InjectedComponent<AppProps> {
    renderStatus() {
        return <Row gutter={16}>
            <Col span={4}>
                <Card>
                    Camera is: <Icon type={this.injectedProps.machineClient.isCameraConnected ? 'plus' : 'minus'} />
                </Card>
            </Col>
            <Col span={4}>
                <Card>
                    Controller is: <Icon type={this.injectedProps.machineClient.isMachineAvailable ? 'plus' : 'minus'} />
                </Card>
            </Col>
        </Row>
    }

    render() {
        const createPropsFor = (led: number) => {
            return ({
                shape: "circle",
                icon: "bulb",
                type: this.injectedProps.machineClient.switches.get(led) ? 'danger' : 'default',
                onClick: ((x) => this.injectedProps.machineClient.toggleLed(led)) as React.MouseEventHandler<HTMLElement>
            }) as ButtonProps
        };

        return (
            <React.Fragment>
                {this.renderStatus()}
                <Row gutter={16}>
                    <Col span={12}>
                        <Row gutter={16} align="middle" justify="center">
                            <Col span={1}>
                                <Button  {...createPropsFor(0)} />
                            </Col>
                            <Col span={2} offset={6}>
                                <Button  {...createPropsFor(1)} />
                            </Col>
                            <Col span={1} offset={6} >
                                <Button  {...createPropsFor(2)} />
                            </Col>
                        </Row>
                        <Row gutter={16} align="middle" justify="center">
                            <Col span={1}>
                                <Button  {...createPropsFor(7)} />
                            </Col>
                            <Col span={14} >
                                <div className={'container'} style={({backgroundImage: "url(http://localhost:3005/api/camera/stream)"})}/>
                            </Col>
                            <Col span={1}>
                                <Button {...createPropsFor(3)} />
                            </Col>
                        </Row>
                        <Row gutter={16} align="middle" justify="center">
                            <Col span={1}>
                                <Button  {...createPropsFor(4)} />
                            </Col>
                            <Col span={2} offset={6}>
                                <Button  {...createPropsFor(5)} />
                            </Col>
                            <Col span={1} offset={6}>
                                <Button  {...createPropsFor(6)} />
                            </Col>
                        </Row>
                    </Col>
                    <Col span={4}>
                        <Row gutter={16} align="middle" justify="center">
                        <Button  shape="circle" icon="" type={'default'} onClick={((x) => this.injectedProps.machineClient.record())} />
                        <TextArea rows={8} placeholder="type command"  onPressEnter={x => this.injectedProps.machineClient.execute(x.currentTarget.value)} />
                        <span>
                            {this.injectedProps.machineClient.result}
                        </span>
                        </Row>
                    </Col>
                </Row>

            </React.Fragment >
        );
    }
}

