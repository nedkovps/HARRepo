import React from 'react';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';

const AccessDenied = props => {

    const error = props.location.state;

    return <ShadowBlock>
        <PageHeader title="Access Denied" link={{ label: 'Back', to: 'back' }} />
        <p>{error && error.message ? error.message : 'You don\'t have permission to access this resource.'}</p>
    </ShadowBlock>;
}

export default React.memo(AccessDenied);