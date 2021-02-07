import React, { useCallback, useEffect, useState } from 'react';
import Loader from '../components/Loader';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';
import RepositoryFileManager from './RepositoryFileManager';

const Repository = props => {

    const id = props.match.params.id;
    const modelTemplate = {
        isLoading: true,
        data: null
    };
    const [model, setModel] = useState(modelTemplate);

    const loadRepository = useCallback(async () => {
        const response = await fetch(`https://localhost:44363/api/repositories/${id}`);
        const repo = await response.json();
        setModel({ isLoading: false, data: repo });
    }, [id]);

    useEffect(() => {
        loadRepository();
    }, [loadRepository]);

    return <ShadowBlock>
        <PageHeader title={`Repository Details ${!model.isLoading && model.data ? `(${model.data.name})` : ''}`} />
        {model.isLoading && <Loader />}
        {!model.isLoading && model.data && <RepositoryFileManager root={model.data.root} />}
    </ShadowBlock>;
}

export default React.memo(Repository);