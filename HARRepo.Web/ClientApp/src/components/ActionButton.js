import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import ReactTooltip from 'react-tooltip';

export const ActionButton = props => {
    return (
        <button className={props.className ? props.className : 'btn btn-sm btn-outline-secondary'} style={props.style} onClick={props.click} data-tip={props.tooltip}>
            {props.icon && <FontAwesomeIcon icon={props.icon} />}
            {props.text && <span>{props.text}</span>}
            {props.tooltip && <ReactTooltip effect="solid" />}
        </button>
    );
}

export default React.memo(ActionButton);