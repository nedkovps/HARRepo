import React, { Component } from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import './custom.css'
import CreateRepository from './views/Repository/CreateRepository';
import HARViewer from './views/HARViewer/HARViewer';
import Repository from './views/Repository/Repository';
import UserRepositories from './views/Repository/UserRepositories';
import DeleteRepo from './views/Repository/DeleteRepo';
import AuthorizedRoute from './components/AuthorizedRoute';
import Callback from './views/Callback';
import AccessDenied from './views/AccessDenied';
import NotFound from './views/NotFound';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <AuthorizedRoute exact path='/' component={UserRepositories} />
            <AuthorizedRoute exact path='/CreateRepo' component={CreateRepository} />
            <AuthorizedRoute path='/Repos/:id' component={Repository} />
            <AuthorizedRoute path='/DeleteRepo/:id' component={DeleteRepo} />
            <AuthorizedRoute path='/ViewHAR/:id' component={HARViewer} />
            <Route exact path='/callback' component={Callback} />
            <Route exact path='/AccessDenied' component={AccessDenied} />
            <Route exact path='/NotFound' component={NotFound} />
      </Layout>
    );
  }
}
