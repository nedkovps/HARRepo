import React from 'react';
import { Route } from 'react-router';
import AuthInstance from '../framework/AuthInstance';

export const AuthorizedRoute = props => {

    const { login, isAuthenticated } = AuthInstance.GetInstance(props.history);
    if (!isAuthenticated()) {
        login();
    }

    const isAuthorized = isAuthenticated();

    return (
        <Route exact={props.exact}
            path={props.path}
            component={isAuthorized ? props.component : undefined}
            render={isAuthorized ? props.render : () => <div>Not Authoorized. Redirecting to login..</div>} />
    );
}

export default AuthorizedRoute;