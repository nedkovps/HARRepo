import React from 'react';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';

const Error = props => {

    const error = props.location.state;

    return <ShadowBlock>
        <PageHeader title="Error" link={{ label: 'Back', to: 'back' }} />
        <p>{error && error.message ? error.message : 'An error has occured. Please try again in a moment. If the problem persists contact HarRepo Support.'}</p>
    </ShadowBlock>;
}

export default React.memo(Error);