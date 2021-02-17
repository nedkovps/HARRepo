import React, { useState } from 'react';
import Input from '../../components/Input';
import PageHeader from '../../components/PageHeader';

const ShareFilePanelContent = props => {

    const [email, setEmail] = useState('');
    const [comment, setComment] = useState('');
    const [errors, setErrors] = useState({});

    const emailChangeHandler = e => {
        e.persist();
        setErrors(err => { return { ...err, email: '' } });
        setEmail(e.target.value);
    }

    const commentChangeHandler = e => {
        e.persist();
        setErrors(err => { return { ...err, comment: '' } });
        setComment(e.target.value);
    }

    const validateEmail = email => {
        const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }

    const submit = () => {
        if (!email) {
            setErrors(err => { return { ...err, email: 'Email is required.' } });
            return;
        }
        if (!validateEmail(email)) {
            setErrors(err => { return { ...err, email: 'You need to enter a valid email address.' } });
            return;
        }
        props.share(props.id, email, comment);
    }

    return <>
        <PageHeader title="Share File" />
        <div>
            <Input config={{ type: 'email' }} label="User email" value={email} change={emailChangeHandler} errors={errors.email} />
            <Input label="Comment" value={comment} change={commentChangeHandler} errors={errors.comment} />
        </div>
        <div className="d-block" style={{ height: '50px' }}>
            <button className="btn btn-primary float-right ml-1" onClick={submit}>Share</button>
            <button className="btn btn-secondary float-right" onClick={() => props.setPanel({ isVisible: false, type: '' })}>
                Cancel
            </button>
        </div>
    </>;
}

export default React.memo(ShareFilePanelContent);