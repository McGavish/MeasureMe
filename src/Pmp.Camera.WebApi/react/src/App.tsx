import { observer, inject } from 'mobx-react'
import { RouterStore } from 'mobx-react-router';
import { Route } from 'react-router';
import { MeasureApp } from './MeasureApp';
import { Layout, Menu, Icon } from 'antd';
import { InjectedComponent } from './InjectedComponent';
import { CanvasModel } from "./Models/CanvasModel";
import { CameraApp } from './CameraApp';
import { MachineClient } from './MachineClient';
import * as React from 'react'
import 'antd/dist/antd.css';
import './index.css';
import './App.css';

const { SubMenu } = Menu;
const { Header, Content, Sider } = Layout;

export class AppProps {
  canvas!: CanvasModel
  routing!: RouterStore
  machineClient!: MachineClient
}

@observer
@inject('canvas', 'routing', 'machineClient')
export default class App extends InjectedComponent<AppProps>{
  render() {
    const { push, location } = this.injectedProps.routing;

    return (
      <Layout>
        <Header className="header">
          <div className="logo" />
          <Menu
            theme="dark"
            mode="horizontal"
            style={{ lineHeight: '64px' }}
          >
            <Menu.Item onClick={() => push('/')} key="1">PMP</Menu.Item>
          </Menu>
        </Header>
        <Layout>
          <Sider width={200} style={{ background: '#fff' }}>
            <Menu
              mode="inline"
              defaultSelectedKeys={[location.pathname === '/camera' ? '2' : '1']}
              defaultOpenKeys={['sub1']}
              style={{ height: '100%', borderRight: 0 }}
            >
              <SubMenu key="sub1" title={<span><Icon type="user" />Apps</span>}
              >
                <Menu.Item onClick={() => this.injectedProps.routing.push('/')} key="1">Measure.Me</Menu.Item>
                <Menu.Item onClick={() => this.injectedProps.routing.push('/camera')} key="2">Camera Live</Menu.Item>
                <Menu.Item key="3">Coming soon</Menu.Item>
              </SubMenu>
            </Menu>
          </Sider>
          <Layout style={{ padding: '0 24px 24px' }}>
            <Content
              style={{
                background: '#fff',
                padding: 24,
                margin: 0,
                minHeight: 280,
              }}
            >
              <Route exact path="/" component={MeasureApp}/>
              <Route exact path="/camera" component={CameraApp}/>
            </Content>
          </Layout>
        </Layout>
      </Layout>
    )
  }
}