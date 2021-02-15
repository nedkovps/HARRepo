import React from 'react';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';

const NotFound = props => {
    return <ShadowBlock>
        <PageHeader title="Not Found" />
        <p>The resource was not found.</p>
    </ShadowBlock>;
}

export default React.memo(NotFound);