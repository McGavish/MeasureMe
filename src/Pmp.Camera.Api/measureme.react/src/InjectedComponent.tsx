import { Component } from 'react';

export class InjectedComponent<T> extends Component<{}> {
  get injectedProps() {
    return this.props as T;
  }
}
