import React, { useState } from 'react';
import Input from '../../components/Input';
import PageHeader from '../../components/PageHeader';
import ShadowBlock from '../../components/ShadowBlock';
import useFileManagerAPI from '../../framework/hooks/useFileManagerAPI';

const CreateRepository = props => {

    const client = useFileManagerAPI();

    const modelTemplate = {
        name: '',
        errors: {}
    };
    const [model, setModel] = useState(modelTemplate);

    const nameChangeHandler = e => {
        e.persist();
        setModel(m => { return { ...m, name: e.target.value, errors: {} } });
    }

    const cancel = () => {
        props.history.push('/');
    }

    const create = async () => {
        if (!model.name) {
            setModel(m => { return { ...m, errors: { name: "Name is required." } } });
            return;
        }
        await client.createRepository(model.name);
        props.history.push('/');
    }

    return <ShadowBlock>
        <PageHeader title="Create New Repo" />
        <div>
            <Input label="Name" value={model.name} change={nameChangeHandler} errors={model.errors.name} />
        </div>
        <div className="d-block" style={{ height: '50px' }}>
            <button className="btn btn-primary float-right ml-1" onClick={create}>Create</button>
            <button className="btn btn-secondary float-right" onClick={cancel}>Cancel</button>
        </div>
    </ShadowBlock>;
}

export default React.memo(CreateRepository);