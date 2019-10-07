import * as React from 'react'
import { Component } from 'react';
import { observer } from 'mobx-react';
import { Card, Input, Select, Col, Row } from 'antd';
import { MachineClient } from './MachineClient';
import { SettingItem } from './SettingItem';

const { Option } = Select;

@observer
export class SettingBoard extends Component<{
    machineClient: MachineClient;
    item: SettingItem;
}> {
    render() {
        const { item, machineClient } = this.props;
        const children = [<span>{item.name} {item.value}</span>];
        if (this.props.item.setCommand != null) {
            if (this.props.item.isEnum) {
                children.push(<Select style={{ width: 100 }} value={item.value} onChange={(x: string) => item.post(x)}>
                    {item.possibleValues.map(x => <Option value={x} key={x}>{x}</Option>)}
                </Select>);
            }
            else {
                children.push(<Input value={item.value} placeholder="value..." onPressEnter={(x) => item.post(x.currentTarget.value)} />);
            }
        }
        return (<Card>
            <Col>
                {children.map(x => <Row >{x}</Row>)}
            </Col>
        </Card>);
    }
}
