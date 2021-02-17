import React, { useCallback, useEffect, useState } from 'react';
import { Grid } from '../../components/Grid';
import { Column } from 'primereact/column';
import Loader from '../../components/Loader';
import PageHeader from '../../components/PageHeader';
import ShadowBlock from '../../components/ShadowBlock';
import useFileManagerAPI from '../../framework/hooks/useFileManagerAPI';
import { faExternalLinkAlt as viewIcon, faUserSlash as unshareIcon } from '@fortawesome/free-solid-svg-icons';

const Collaboration = props => {

    const client = useFileManagerAPI();
    const [actionsCount, setActionsCount] = useState(0);

    const filesSharedWithUserModelTemplate = {
        isLoading: true,
        data: []
    };

    const sharedFilesModelTemplate = {
        isLoading: true,
        data: []
    };

    const [filesSharedWithUser, setFilesSharedWithUser] = useState(filesSharedWithUserModelTemplate);
    const [sharedFiles, setSharedFiles] = useState(sharedFilesModelTemplate);

    const loadFilesSharedWithUser = useCallback(async () => {
        const filesSharedWithUserData = await client.getFilesSharedWithUser();
        setFilesSharedWithUser({ isLoading: false, data: filesSharedWithUserData });
    }, [client]);

    const loadSharedFiles = useCallback(async () => {
        const sharedFilesData = await client.getSharedFiles();
        setSharedFiles({ isLoading: false, data: sharedFilesData });
    }, [client, actionsCount]); // eslint-disable-line no-use-before-define

    useEffect(() => {
        loadFilesSharedWithUser();
        loadSharedFiles();
    }, [loadFilesSharedWithUser, loadSharedFiles]);

    const viewHar = path => {
        props.history.push(`/ViewHAR/${path}`);
    }

    const unshare = async id => {
        setSharedFiles({ isLoading: true, data: [] });
        await client.unshareFile(id);
        setActionsCount(actionsCount + 1);
    }

    const sharedWithMeActions = [
        { type: 'button', icon: viewIcon, tooltip: 'View', action: share => viewHar(share.file.path) }
    ];

    const sharedActions = [
        { type: 'button', icon: viewIcon, tooltip: 'View', action: share => viewHar(share.file.path) },
        { type: 'button', icon: unshareIcon, tooltip: 'Unshare', action: share => unshare(share.id) }
    ];

    return <ShadowBlock>
        <PageHeader title="Collaboration" />
        <div className="mt-3">
            <h4>Shared with me</h4>
            {filesSharedWithUser.isLoading ? <Loader /> : (filesSharedWithUser.data.length > 0 ?
                <Grid items={filesSharedWithUser.data} actions={sharedWithMeActions}>
                    <Column field="file.name" header="File Name" sortable={true} />
                    <Column field="sharedBy.name" header="Shared By" sortable={true} />
                    <Column field="comment" header="Comment" sortable={true} />
                </Grid> :
                <p>There are no files shared with you yet.</p>)}
        </div>
        <div className="mt-3">
            <h4>Shared by me</h4>
            {sharedFiles.isLoading ? <Loader /> : (sharedFiles.data.length > 0 ?
                <Grid items={sharedFiles.data} actions={sharedActions}>
                    <Column field="file.name" header="File Name" sortable={true} />
                    <Column field="sharedWith.name" header="Shared With" sortable={true} />
                    <Column field="comment" header="Comment" sortable={true} />
                </Grid> :
                <p>You haven't shared any files yet.</p>)}
        </div>
    </ShadowBlock>;
}

export default React.memo(Collaboration);