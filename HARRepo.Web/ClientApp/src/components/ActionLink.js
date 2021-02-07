import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import ReactTooltip from 'react-tooltip';

export const ActionLink = props => {
    return (
        <Link className="btn btn-sm btn-outline-secondary" style={props.style} to={props.to} data-tip={props.tooltip}>
            {props.icon && <FontAwesomeIcon icon={props.icon} />}
            {props.text && <span>{props.text}</span>}
            {props.tooltip && <ReactTooltip effect="solid" />}
        </Link>
    );
}

export default React.memo(ActionLink);