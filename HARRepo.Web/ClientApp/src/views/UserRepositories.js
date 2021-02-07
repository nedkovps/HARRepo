import React, { useEffect, useState } from 'react';
import { Grid } from '../components/Grid';
import { Column } from 'primereact/column';
import PageHeader from '../components/PageHeader';
import ShadowBlock from '../components/ShadowBlock';

const UserRepositories = props => {

    const userId = 1;

    const modelTemplate = {
        isLoading: true,
        data: []
    };

    const [repos, setRepos] = useState(modelTemplate);

    const loadUserRepos = async () => {
        const response = await fetch(`api/users/${userId}/repositories`);
        const repos = await response.json();
        setRepos({ isLoading: false, data: repos });
    }

    useEffect(() => {
        loadUserRepos();
    }, []);

    const createRepo = () => {

    }

    return <ShadowBlock>
        <PageHeader title="My Repos" button={{ label: 'Create', action: createRepo }} />
        <div className="mt-3">
            {repos.data.length === 0 && <p>You don't have any HAR Repos yet. To create you first repo click on the 'Create' button.</p>}
            {repos.data.length > 0 && <Grid items={repos}>
                <Column field="name" header="Name" sortable={true} />
            </Grid>}
        </div>
    </ShadowBlock>;
}

export default React.memo(UserRepositories);