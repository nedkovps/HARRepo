import React from 'react';
import './HeadersTable.css';

const HeadersTable = props => {
    return <div cellPadding="0" cellSpacing="0" className="d-table netInfoHeadersText netInfoText netInfoHeadersTable ">
        <div className="netInfoResponseHeadersTitle d-table-row">
            <div colSpan="2" className="d-table-cell">
                <div className="netInfoHeadersGroup ">Response Headers</div>
            </div>
        </div>
        {props.responseHeaders.map((h, i) => <div className="d-table-row" key={i}>
            <div className="netInfoParamName d-table-cell">{h.name}</div>
            <div className="netInfoParamValue d-table-cell">{h.value}</div>
        </div>)}
        <div className="netInfoRequestHeadersTitle d-table-row">
            <div colSpan="2" className="d-table-cell">
                <div className="netInfoHeadersGroup ">Request Headers</div>
            </div>
        </div>
        {props.requestHeaders.map((h, i) => <div key={i} className="d-table-row">
            <div className="netInfoParamName d-table-cell">{h.name}</div>
            <div className="netInfoParamValue d-table-cell">{h.value}</div>
        </div>)}
    </div>;
}

export default React.memo(HeadersTable);