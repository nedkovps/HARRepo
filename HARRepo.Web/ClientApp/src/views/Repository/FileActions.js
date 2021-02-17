import React from 'react';
import { ActionButton } from '../../components/ActionButton';
import { faTrash, faExternalLinkAlt, faShareSquare } from '@fortawesome/free-solid-svg-icons';

const FileActions = props => {
    return <div className="w-100">
        <span>{props.name}</span>
        <div className="btn-group float-right" role="group">
            <ActionButton icon={faExternalLinkAlt} tooltip="View" click={() => props.view(props.id)} />
            <ActionButton icon={faShareSquare} tooltip="Share" click={() => props.initShare(props.id)} />
            <ActionButton icon={faTrash} tooltip="Delete" click={() => props.confirmDelete(props.id)} />
        </div>
    </div>;
}

export default React.memo(FileActions);