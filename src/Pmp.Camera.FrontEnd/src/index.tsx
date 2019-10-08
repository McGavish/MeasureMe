import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';
import { CanvasModel } from "./Models/CanvasModel";
import { RouterStore, syncHistoryWithStore } from 'mobx-react-router';
import { createBrowserHistory } from 'history';
import { Router } from 'react-router';
import { Provider } from 'mobx-react';
import { MachineClient } from './MachineClient';

const browserHistory = createBrowserHistory();
const model = new CanvasModel();
const routingStore = new RouterStore();
const machineClient = new MachineClient('');
const stores = {
    // Key can be whatever you want
    canvas: model,
    routing: routingStore,
    machineClient: machineClient
    // ...other stores
};

export type AllStores = typeof stores;

const history = syncHistoryWithStore(browserHistory, routingStore);


ReactDOM.render(<Provider {...stores}>
    <Router history={history} >
        <App />
    </Router>
</Provider>, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
