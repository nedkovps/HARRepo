import React, { useState } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import GlobalFilter from './GlobalFilter';
import ActionLink from './ActionLink';
import ActionButton from './ActionButton';
import { v4 as uuid } from 'uuid';
import PropTypes from 'prop-types';

export const Grid = props => {

    const [globalFilter, setGlobalFilter] = useState('');

    const header = <div className="row">
        <div className="col-md-4 col-sm-6 col-xs-12">
            <GlobalFilter value={globalFilter} change={e => setGlobalFilter(e.target.value)} />
        </div>
    </div>;

    const actionTemplate = item => <div className="btn-group" role="group">
        {props.actions && props.actions.length > 0 && props.actions.map(a => {
            if (a.type === 'link') {
                return <ActionLink key={uuid()} icon={a.icon} tooltip={a.tooltip} text={a.text} to={a.action(item)} />;
            }
            else if (a.type === 'button') {
                return <ActionButton key={uuid()} icon={a.icon} tooltip={a.tooltip} text={a.text} click={a.action.bind(this, item)} />;
            }
            else {
                return null;
            }
        })}
    </div>;

    return (
        <>
            <DataTable value={props.items} header={header} globalFilter={globalFilter} className="p-datatable-gridlines"
                paginator={true} rows={10} responsive={true} rowsPerPageOptions={[5, 10, 20]} >
                {props.children}
                {props.actions && props.actions.length > 0 && <Column body={actionTemplate} style={{ textAlign: 'center', width: '8em' }} />}
            </DataTable>
        </>
    );
}

// Validating props
Grid.propTypes = {
    items: PropTypes.array.isRequired,
    actions: PropTypes.array,
    exportUrl: PropTypes.string,
    exportFileName: PropTypes.string
}

export default React.memo(Grid);