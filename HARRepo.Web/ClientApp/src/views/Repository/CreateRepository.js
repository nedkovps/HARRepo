import React, { useState } from 'react';
import Input from '../../components/Input';
import PageHeader from '../../components/PageHeader';
import ShadowBlock from '../../components/ShadowBlock';

const CreateRepository = props => {

    const userId = 1;
    const modelTemplate = {
        userId: userId,
        name: ''
    };
    const [model, setModel] = useState(modelTemplate);

    const nameChangeHandler = e => {
        e.persist();
        setModel(m => { return { ...m, name: e.target.value } });
    }

    const cancel = () => {
        props.history.push('/');
    }

    const create = async () => {
        await fetch(`https://localhost:44363/api/users/${userId}/repositories?name=${model.name}`, {
            method: 'POST',
            body: null
        });
        props.history.push('/');
    }

    return <ShadowBlock>
        <PageHeader title="Create New Repo" />
        <div>
            <Input label="Name" value={model.name} change={nameChangeHandler} />
        </div>
        <div className="d-block" style={{ height: '50px' }}>
            <button className="btn btn-primary float-right ml-1" onClick={create}>Create</button>
            <button className="btn btn-secondary float-right" onClick={cancel}>Cancel</button>
        </div>
    </ShadowBlock>;
}

export default React.memo(CreateRepository);