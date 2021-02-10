import React from 'react';
import PageDetails from './PageDetails';

const HARDetails = props => {
    return <>{props.HAR && props.HAR.log && props.HAR.log.pages.map(p => {

        const entries = props.HAR.log.entries.filter(x => x.pageref === p.id);
        const pageWithEntries = { ...p, entries: entries };

        return <PageDetails key={p.id} page={pageWithEntries} />
    })}</>;
}

export default React.memo(HARDetails);