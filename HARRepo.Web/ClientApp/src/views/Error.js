import React from 'react';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';

const Error = props => {
    return <ShadowBlock>
        <PageHeader title="Error" />
        <p>An error has occured. Please try again in a moment. If the problem persists contact HarRepo Support.</p>
    </ShadowBlock>;
}

export default React.memo(Error);