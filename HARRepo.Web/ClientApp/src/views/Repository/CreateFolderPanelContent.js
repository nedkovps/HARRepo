import React, { useState } from 'react';
import PageHeader from '../../components/PageHeader';
import Input from '../../components/Input';

const CreateFolderPanelContent = props => {

    const [folderName, setFolderName] = useState('');
    const [error, setError] = useState('');

    const folderNameChangeHandler = e => {
        e.persist();
        setError('');
        setFolderName(e.target.value);
    }

    const submit = () => {
        if (!folderName) {
            setError('Folder name is required.');
            return;
        }
        props.create(folderName);
    }

    return <>
        <PageHeader title="Create New Folder" />
        <div>
            <Input label="Name" value={folderName} change={folderNameChangeHandler} errors={error} />
        </div>
        <div className="d-block" style={{ height: '50px' }}>
            <button className="btn btn-primary float-right ml-1" onClick={submit}>Create</button>
            <button className="btn btn-secondary float-right" onClick={() => props.setPanel({ isVisible: false, type: '' })}>
                Cancel
            </button>
        </div>
    </>;
}

export default React.memo(CreateFolderPanelContent);