import React, { useCallback, useEffect, useState } from 'react';
import PageHeader from '../components/PageHeader';
import HARDetails from './HARDetails';

const HARViewer = props => {

    const id = props.match.params.id;
    const [HAR, setHAR] = useState({});

    const loadHAR = useCallback(async () => {
        const HARResponse = await fetch(`http://localhost:1337?id=${id}`);
        const harJSON = await HARResponse.json();
        setHAR(harJSON);
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