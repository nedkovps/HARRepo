import React from 'react';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';

const NotFound = props => {

    const error = props.location.state;

    return <ShadowBlock>
        <PageHeader title="Not Found" link={{ label: 'Back', to: 'back' }} />
        <p>{error && error.message ? error.message : 'The resource was not found.'}</p>
    </ShadowBlock>;
}

export default React.memo(NotFound);