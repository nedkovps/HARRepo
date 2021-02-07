import React from 'react';

export const shadowBlock = props => {


    return (
        <div className="shadow-block p-3 mb-3 bg-white rounded" style={props.style}>
            {props.children}
        </div>
    );
}

export default React.memo(shadowBlock);