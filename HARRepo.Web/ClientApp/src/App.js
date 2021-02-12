import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import './custom.css'
import CreateRepository from './views/Repository/CreateRepository';
import HARViewer from './views/HARViewer/HARViewer';
import Repository from './views/Repository/Repository';
import UserRepositories from './views/Repository/UserRepositories';
import DeleteRepo from './views/Repository/DeleteRepo';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <Route exact path='/' component={UserRepositories} />
            <Route exact path='/CreateRepo' component={CreateRepository} />
            <Route path='/Repos/:id' component={Repository} />
            <Route path='/DeleteRepo/:id' component={DeleteRepo} />
            <Route path='/ViewHAR/:id' component={HARViewer} />
      </Layout>
    );
  }
}
