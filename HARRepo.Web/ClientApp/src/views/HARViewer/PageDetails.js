import React, { useRef, useState } from 'react';
import { DataScroller } from 'primereact/datascroller';
import EntryDetails from './EntryDetails';
import './PageDetails.css';

const PageDetails = props => {

    const ds = useRef(null);
    const [isExpanded, setIsExpanded] = useState(false);

    const calcResponseSize = number => {
        let unit = 'KB';
        number = number / 1000;
        if (number > 1000) {
            unit = 'MB';
            number = number / 1000;
        }
        number = Math.round(number * 10) / 10;

        return `${number} ${unit}`;
    }

    const calcTotals = entries => {
        let size = 0;
        let time = (new Date(entries[entries.length - 1].startedDateTime).getTime() - (new Date(entries[0].startedDateTime).getTime()) + entries[entries.length - 1].time) / 1000;
        for (let i = 0; i < entries.length; i++) {
            size += entries[i].response.content.size;
        }
        return { totalSize: calcResponseSize(size), totalTime: Math.round((time) * 100) / 100 };
    }

    const totals = calcTotals(props.page.entries);

    const itemTemplate = data => {
        return <EntryDetails entry={data} startTime={new Date(props.page.entries[0].startedDateTime).getTime()}
            totals={totals} calcResponseSize={calcResponseSize} />;
    }

    const headerText = `${props.page.entries.length} Requests | ${totals.totalTime}s (onload: ${Math.round(((props.page.pageTimings.onLoad / 1000)) * 100) / 100}s)`;
    const footerText = `Total Response Size: ${totals.totalSize}`;

    return <>
        <div className="clickable" onClick={() => setIsExpanded(!isExpanded)}><i className={`pi pi-${isExpanded ? 'minus' : 'plus'}`}></i><span> {props.page.title}{!isExpanded && <span> ({headerText})</span>}</span></div>
        {isExpanded && <DataScroller ref={ds} value={props.page.entries} itemTemplate={itemTemplate} rows={props.page.entries.length}
            header={headerText} footer={footerText} inline scrollHeight="650px" className="mt-1" />}
    </>;
}

export default React.memo(PageDetails);