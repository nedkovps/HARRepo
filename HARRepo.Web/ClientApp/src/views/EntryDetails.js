import React, { useState } from 'react';
import { Tag } from 'primereact/tag';
import { Button } from 'primereact/button';
import { ProgressBar } from 'primereact/progressbar';
import { TabView, TabPanel } from 'primereact/tabview';
import classNames from 'classnames';
import HeadersTable from './HeadersTable';

const EntryDetails = props => {

    const [isExpanded, setIsExpanded] = useState(false);

    const data = props.entry;
    const splitUrl = data.request.url.split('/');

    const timeAfterBeginning = ((new Date(data.startedDateTime)).getTime() - props.startTime) / 1000;
    const percentAfterBeginning = (timeAfterBeginning / props.totals.totalTime) * 100;

    const timeInSeconds = Math.round(((data.time / 1000)) * 100) / 100;
    const percentOfTotalTime = ((data.time / 1000) / props.totals.totalTime) * 100 + percentAfterBeginning;

    const detailsButtonClasses = classNames({
        'p-button-sm': true,
        'p-button-secondary': isExpanded,
        'p-button-outlined': true
    });

    return <>
        <div className="product-item">
            <div className="product-detail">
                <div className="product-name">{data.request.method}<span className="font-weight-bold"> {splitUrl[splitUrl.length - 1]}</span></div>
                <div className="product-description font-italic"><span className="request-url">{data.request.url}</span></div>
                <div><Tag className="p-mr-2" severity={data.response.status >= 200 && data.response.status < 300 ? 'success' : (data.response.status > 400 ? 'danger' : 'warning')} value={data.response.status}></Tag></div>
            </div>
            <div className="request-timeline pl-3 pr-3">
                <ProgressBar value={percentOfTotalTime} style={{ marginLeft: `${percentAfterBeginning}%` }}
                    displayValueTemplate={() => <span>{`${timeInSeconds}s`}</span>}></ProgressBar>
            </div>
            <div className="product-action">
                <span className="product-category">{`${props.calcResponseSize(data.response.content.size)}`}</span>
                <span className="product-category"><i className="pi pi-clock"></i>{` ${timeInSeconds}s`}</span>
                <Button icon="pi pi-bars" label="Details" className={detailsButtonClasses}
                    onClick={() => setIsExpanded(!isExpanded)}></Button>
            </div>
        </div>
        {isExpanded && <div className="request-details">
            <TabView>
                <TabPanel header="Headers">
                    <HeadersTable requestHeaders={data.request.headers} responseHeaders={data.response.headers} />
                </TabPanel>
                <TabPanel header="Response">
                    {data.response.content.text}
                </TabPanel>
            </TabView>
        </div>}
    </>;
}

export default React.memo(EntryDetails);