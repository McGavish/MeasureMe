import * as React from 'react'
import { Component } from 'react';
import { Row, Col } from 'antd';
import { MachineClient } from './MachineClient';
import { SettingBoard } from './SettingBoard';
export class SettingGrid extends Component<{
    machineClient: MachineClient;
}> {
    render() {
        const { machineClient } = this.props;
        const itemsPerRow = 4;


        const rows = Math.ceil(machineClient.settingItems.length / itemsPerRow);
        const cameraItems = Array.apply(0, Array(rows)).map((_, x) => <Row key={'row' + x} gutter={16}>
            {machineClient.settingItems
                .slice(x * itemsPerRow, (x + 1) * itemsPerRow)
                .map(r =>
                    <Col key={'row-col' + x + r.name} span={16 / itemsPerRow}>
                        <SettingBoard machineClient={machineClient} item={r} />
                    </Col>
                )
            }
        </Row>);

        const ucRows = Math.ceil(machineClient.ucItems.length / itemsPerRow);
        const ucItems = Array.apply(0, Array(ucRows)).map((_, x) => <Row key={'row' + x} gutter={16}>
            {machineClient.ucItems
                .slice(x * itemsPerRow, (x + 1) * itemsPerRow)
                .map(r =>
                    <Col key={'row-col-m' + x + r.name} span={16 / itemsPerRow}>
                        <SettingBoard machineClient={machineClient} item={r} />
                    </Col>
                )
            }
        </Row>);
        return (
            <React.Fragment>
                {cameraItems}
                {ucItems}
            </React.Fragment>
        );
    }
}
