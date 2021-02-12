import React, { useCallback, useEffect, useState } from 'react';
import PageHeader from '../../components/PageHeader';
import { HARViewerServiceClient } from '../../framework/HARViewerServiceClient';
import HARDetails from './HARDetails';

const HARViewer = props => {

    const id = props.match.params.id;
    const [HAR, setHAR] = useState({});

    const loadHAR = useCallback(async () => {
        const HARData = await HARViewerServiceClient.getHAR(id);
        setHAR(HARData);
    }, [id]);

    useEffect(() => {
        loadHAR();
    }, [loadHAR]);

    return <>
        <PageHeader title="HAR Details" />
        <HARDetails HAR={HAR} />
    </>;
}

export default React.memo(HARViewer);