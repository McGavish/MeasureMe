import * as React from 'react'
import { Component } from 'react';
import { observer } from 'mobx-react';
import { Card, Input, Select, Col, Row, Button } from 'antd';
import { MachineClient } from './MachineClient';
import { SettingItem, ButtonWithParameterDescription } from './SettingItem';

const { Option } = Select;

@observer
export class SettingBoard extends Component<{
    machineClient: MachineClient;
    item: SettingItem | ButtonWithParameterDescription;
}> {
    render() {
        const { item, machineClient } = this.props;


        if (item instanceof SettingItem) {

            const children = [<span>{item.name} {item.value}</span>];
            if (item.setCommand != null) {
                if (item.isEnum) {
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
                    {children.map((x, i) => <Row key={'si' + i} >{x}</Row>)}
                </Col>
            </Card>);
        }
        else {
            if (item instanceof ButtonWithParameterDescription) {
                return (<Card>
                    <Col>
                        {item.parameterDescription.map((x, i) =>
                            (<Row key={'pd' + item.name + i}>
                                {x.name}
                                <Input value={x.Value} onChange={(v) => x.Value = v.currentTarget.value} />
                            </Row>)
                        )}
                        <Button onClick={(x) => item.invoke()}>Submit {item.name}</Button>
                    </Col>
                </Card>);
            }
        }
    }
}
