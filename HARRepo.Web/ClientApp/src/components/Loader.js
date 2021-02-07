import React from 'react';
import { ProgressBar } from 'primereact/progressbar';

const Loader = () => {
    return (
        <span><ProgressBar mode="indeterminate" style={{ height: '6px' }} /></span>
    );
}

export default Loader;