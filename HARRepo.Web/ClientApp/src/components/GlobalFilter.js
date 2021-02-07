import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch as search } from '@fortawesome/free-solid-svg-icons';

export const GlobalFilter = props => {
    return (
        <div className="input-group mb-3">
            <div className="input-group-prepend">
                <span className="input-group-text" id="basic-addon1">
                    <FontAwesomeIcon icon={search} />
                </span>
            </div>
            <input value={props.value} onChange={props.change} type="text" className="form-control" placeholder="Global Filter" aria-label="Flobal Filter" aria-describedby="basic-addon1" />
        </div>
    );
}

export default React.memo(GlobalFilter);