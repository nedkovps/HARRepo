import React, { useCallback, useEffect, useState } from 'react';
import Loader from '../../components/Loader';
import PageHeader from '../../components/PageHeader';
import ShadowBlock from '../../components/ShadowBlock';
import { FileManagerServiceClient } from '../../framework/FileManagerServiceClient';
import RepositoryFileManager from './RepositoryFileManager';

const Repository = props => {

    const id = props.match.params.id;
    const modelTemplate = {
        isLoading: true,
        data: null
    };
    const [model, setModel] = useState(modelTemplate);
    const [repoUpdates, setRepoUpdates] = useState({ updates: 0, expandedKeys: {} });

    const loadRepository = useCallback(async () => {
        const repo = await FileManagerServiceClient.getRepository(id);
        setModel({ isLoading: false, data: repo });
    }, [id]);

    useEffect(() => {
        loadRepository();
    }, [loadRepository, repoUpdates.updates]);

    const repoUpdatedHandler = expandedKeys => {
        setRepoUpdates({ updates: repoUpdates.updates + 1, expandedKeys: expandedKeys });
    }

    return <ShadowBlock>
        <PageHeader title={`${!model.isLoading && model.data ? `${model.data.name}` : ''}`} />
        {model.isLoading && <Loader />}
        {!model.isLoading && model.data && <RepositoryFileManager expandedKeys={repoUpdates.expandedKeys} root={model.data.root} repoUpdated={repoUpdatedHandler} />}
    </ShadowBlock>;
}

export default React.memo(Repository);