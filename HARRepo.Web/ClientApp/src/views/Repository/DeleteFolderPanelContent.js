import React from 'react';
import PageHeader from '../../components/PageHeader';

const DeleteFolderPanelContent = props => {
    return <>
        <PageHeader title="Delete Folder" />
        <p>
            Are you sure you want to delete the folder?
                </p>
        <div className="d-block" style={{ height: '50px' }}>
            <button className="btn btn-danger float-right ml-1" onClick={() => props.delete(props.id)}>Delete</button>
            <button className="btn btn-secondary float-right" onClick={() => props.setPanel({ isVisible: false, type: '' })}>Cancel</button>
        </div>
    </>;
}

export default React.memo(DeleteFolderPanelContent);