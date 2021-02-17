import React from 'react';
import { ActionButton } from '../../components/ActionButton';
import { faPlus, faTrash, faUpload } from '@fortawesome/free-solid-svg-icons';

const FolderActions = props => {
    return <div className="w-100">
        <span className="noselect" onDoubleClick={() => props.expandOrCondense(props.id)}>{props.name}</span>
        <div className="btn-group float-right" role="group">
            <ActionButton icon={faUpload} tooltip="Upload HAR" click={() => props.uploadHAR(props.id)} />
            <ActionButton icon={faPlus} tooltip="New Folder" click={() => props.createNew(props.id)} />
            {!props.isRoot && <ActionButton icon={faTrash} tooltip="Delete Folder" click={() => props.deleteFolder(props.id)} />}
        </div>
    </div>;
}

export default React.memo(FolderActions);