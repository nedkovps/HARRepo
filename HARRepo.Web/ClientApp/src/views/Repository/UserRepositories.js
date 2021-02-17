import React, { useCallback, useEffect, useState } from 'react';
import { Grid } from '../../components/Grid';
import { Column } from 'primereact/column';
import PageHeader from '../../components/PageHeader';
import ShadowBlock from '../../components/ShadowBlock';
import { faExternalLinkAlt as viewIcon, faTrash as deleteIcon } from '@fortawesome/free-solid-svg-icons';
import Loader from '../../components/Loader';
import useFileManagerAPI from '../../framework/hooks/useFileManagerAPI';
import InfoPanel from '../../components/InfoPanel';

const UserRepositories = props => {

    const client = useFileManagerAPI();

    const modelTemplate = {
        isLoading: true,
        data: []
    };

    const [repos, setRepos] = useState(modelTemplate);
    const [sharedWithUserFilesCount, setSharedWithUserFilesCount] = useState(0);

    const loadUserRepos = useCallback(async () => {
        const repos = await client.getRepositories();
        setRepos({ isLoading: false, data: repos });
    }, [client]);

    const loadSharedWithUserFilesCount = useCallback(async () => {
        const count = await client.getSharedWithUserFilesCount();
        setSharedWithUserFilesCount(count);
    }, [client]);

    useEffect(() => {
        loadUserRepos();
        loadSharedWithUserFilesCount();
    }, [loadUserRepos, loadSharedWithUserFilesCount]);

    const viewRepo = repoId => {
        props.history.push(`/Repos/${repoId}`);
    }

    const deleteRepo = repoId => {
        props.history.push(`/DeleteRepo/${repoId}`);
    }

    const repoActions = [
        { type: 'button', icon: viewIcon, tooltip: 'View', action: repo => viewRepo(repo.id) },
        { type: 'button', icon: deleteIcon, tooltip: 'Delete', action: repo => deleteRepo(repo.id) }
    ];

    return <>
        {sharedWithUserFilesCount > 0 && <InfoPanel text={`${sharedWithUserFilesCount !== 1 ? `There are ${sharedWithUserFilesCount} files` : 'There is one file'} shared with you. You can navigate to the collaboration page by clicking on your user icon in the Nav Bar and selecting the Collaborations menu.`} />}
        <ShadowBlock>
            <PageHeader title="My Repos" link={{ label: 'Create', to: '/CreateRepo' }} />
            <div className="mt-3">
                {repos.isLoading && <Loader />}
                {!repos.isLoading && repos.data.length === 0 && <p>You don't have any HAR Repos yet. To create you first repo click on the 'Create' button.</p>}
                {!repos.isLoading && repos.data.length > 0 && <Grid items={repos.data} actions={repoActions}>
                    <Column field="name" header="Name" sortable={true} />
                    <Column body={repo => new Date(repo.lastActivityOn).toLocaleString()} header="Last Activity" sortable={true} />
                </Grid>}
            </div>
        </ShadowBlock>
    </>;
}

export default React.memo(UserRepositories);