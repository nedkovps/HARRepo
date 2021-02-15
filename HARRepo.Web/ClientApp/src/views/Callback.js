import React from 'react';
import { useEffect } from 'react';
import AuthInstance from '../framework/AuthInstance';

const Callback = props => {

    const { handleAuthentication } = AuthInstance.GetInstance(props.history);

    useEffect(() => {
        if (/access_token|id_token|error/.test(props.location.hash)) {
            handleAuthentication();
        }
        else {
            throw new Error('Invalid callback URL.');
        }
    }, [props.location.hash, handleAuthentication]);

    return (
        <>Loading...</>
    );
}

export default Callback;