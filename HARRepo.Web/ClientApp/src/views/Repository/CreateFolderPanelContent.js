import React, { useState } from 'react';
import PageHeader from '../../components/PageHeader';
import Input from '../../components/Input';

const CreateFolderPanelContent = props => {

    const [folderName, setFolderName] = useState('');

    const folderNameChangeHandler = e => {
        e.persist();
        setFolderName(e.target.value);
    }

    return <>
        <PageHeader title="Create New Folder" />
        <div>
            <Input label="Name" value={folderName} change={folderNameChangeHandler} />
        </div>
        <div className="d-block" style={{ height: '50px' }}>
            <button className="btn btn-primary float-right ml-1" onClick={() => props.create(folderName)}>Create</button>
            <button className="btn btn-secondary float-right" onClick={() => props.setPanel({ isVisible: false, type: '' })}>
                Cancel
            </button>
        </div>
    </>;
}

export default React.memo(CreateFolderPanelContent);