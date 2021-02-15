import React, { useCallback, useEffect, useState } from 'react';
import Loader from '../../components/Loader';
import PageHeader from '../../components/PageHeader';
import { HARViewerServiceClient } from '../../framework/HARViewerServiceClient';
import HARDetails from './HARDetails';

const HARViewer = props => {

    const id = props.match.params.id;
    const [HAR, setHAR] = useState({ data: null, isLoading: true });

    const loadHAR = useCallback(async () => {
        setHAR({ data: null, isLoading: true });
        const HARData = await HARViewerServiceClient.getHAR(id);
        setHAR({ data: HARData, isLoading: false });
    }, [id]);

    useEffect(() => {
        loadHAR();
    }, [loadHAR]);

    return <>
        <PageHeader title="HAR Details" />
        {HAR.isLoading ? <Loader /> : <HARDetails HAR={HAR.data} />}
    </>;
}

export default React.memo(HARViewer);