import React from 'react';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';

const AccessDenied = props => {
    return <ShadowBlock>
        <PageHeader title="Access Denied" />
        <p>You don't have permission to access this resource.</p>
    </ShadowBlock>;
}

export default React.memo(AccessDenied);