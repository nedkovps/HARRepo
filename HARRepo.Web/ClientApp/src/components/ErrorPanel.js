import React, { useEffect, useRef } from 'react';
import { Messages } from 'primereact/messages';

const ErrorPanel = props => {

    const msg = useRef(null);
    useEffect(() => {
        msg.current.show({ severity: 'error', summary: '', detail: props.text, sticky: true });
    }, [props.text]);

    return <div><Messages ref={msg} /></div>;
}

export default React.memo(ErrorPanel);