import React, { useState } from 'react';
import Loader from '../../components/Loader';
import PageHeader from '../../components/PageHeader';
import ShadowBlock from '../../components/ShadowBlock';
import useFileManagerAPI from '../../framework/hooks/useFileManagerAPI';

const DeleteRepo = props => {

    const client = useFileManagerAPI();
    const id = props.match.params.id;

    const [isDeleting, setIsDeleting] = useState(false);

    const deleteRepo = async () => {
        setIsDeleting(true);
        await client.deleteRepository(id);
        setIsDeleting(false);
        props.history.push('/');
    }

    return <ShadowBlock>
        <PageHeader title="Delete Repo" />
        <p>Are you sure you want to delete the repository?</p>
        {!isDeleting ? <div className="d-block" style={{ height: '50px' }}>
            <button className="btn btn-danger float-right ml-1" onClick={deleteRepo}>Delete</button>
            <button className="btn btn-secondary float-right" onClick={() => props.history.push('/')}>Cancel</button>
        </div> : <Loader />}
    </ShadowBlock>;
}

export default React.memo(DeleteRepo);