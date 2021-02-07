import React, { useEffect, useState } from 'react';
import { Grid } from '../components/Grid';
import { Column } from 'primereact/column';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';
import { faExternalLinkAlt as viewIcon, faTrash as deleteIcon } from '@fortawesome/free-solid-svg-icons';
import Loader from '../components/Loader';

const UserRepositories = props => {

    const userId = 1;

    const modelTemplate = {
        isLoading: true,
        data: []
    };

    const [repos, setRepos] = useState(modelTemplate);

    const loadUserRepos = async () => {
        const response = await fetch(`https://localhost:44363/api/users/${userId}/repositories`);
        const repos = await response.json();
        setRepos({ isLoading: false, data: repos });
    }

    useEffect(() => {
        loadUserRepos();
    }, []);

    const viewRepo = repoId => {
        props.history.push(`/Repos/${repoId}`);
    }

    const deleteRepo = repoId => {

    }

    const repoActions = [
        { type: 'button', icon: viewIcon, tooltip: 'View', action: repo => viewRepo(repo.id) },
        { type: 'button', icon: deleteIcon, tooltip: 'Delete', action: repo => deleteRepo(repo.id) }
    ];

    return <ShadowBlock>
        <PageHeader title="My Repos" link={{ label: 'Create', to: '/CreateRepo' }} />
        <div className="mt-3">
            {repos.isLoading && <Loader />}
            {!repos.isLoading && repos.data.length === 0 && <p>You don't have any HAR Repos yet. To create you first repo click on the 'Create' button.</p>}
            {!repos.isLoading && repos.data.length > 0 && <Grid items={repos.data} actions={repoActions}>
                <Column field="name" header="Name" sortable={true} />
                <Column body={repo => new Date(repo.lastActivityOn).toLocaleString()} header="Last Activity" sortable={true} />
            </Grid>}
        </div>
    </ShadowBlock>;
}

export default React.memo(UserRepositories);