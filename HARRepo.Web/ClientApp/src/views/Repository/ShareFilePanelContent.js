import React, { useState } from 'react';
import Input from '../../components/Input';
import PageHeader from '../../components/PageHeader';

const ShareFilePanelContent = props => {

    const [email, setEmail] = useState('');
    const [comment, setComment] = useState('');

    const emailChangeHandler = e => {
        e.persist();
        setEmail(e.target.value);
    }

    const commentChangeHandler = e => {
        e.persist();
        setComment(e.target.value);
    }

    return <>
        <PageHeader title="Share File" />
        <div>
            <Input label="User email" value={email} change={emailChangeHandler} />
            <Input label="Comment" value={comment} change={commentChangeHandler} />
        </div>
        <div className="d-block" style={{ height: '50px' }}>
            <button className="btn btn-primary float-right ml-1" onClick={() => props.share(props.id, email, comment)}>Share</button>
            <button className="btn btn-secondary float-right" onClick={() => props.setPanel({ isVisible: false, type: '' })}>
                Cancel
            </button>
        </div>
    </>;
}

export default React.memo(ShareFilePanelContent);