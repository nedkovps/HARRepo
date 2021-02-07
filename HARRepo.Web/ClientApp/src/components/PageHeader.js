import React from "react";
import { Link } from "react-router-dom";
import { withRouter } from "react-router";

const PageHeader = props => {
    const handleBack = () => {
        props.history.goBack();
    };

    return (
        <div className="d-print-none">
            <div className="row">
                <div className="col-12 clearfix">
                    <h3 className="float-left clearfix">{props.title}</h3>
                    {props.link && props.link.to !== "back" && (
                        <Link
                            className="btn btn-primary float-right"
                            to={props.link.to}
                        >
                            {props.link.label}
                        </Link>
                    )}
                    {props.link && props.link.to === "back" && (
                        <button
                            type="button"
                            className="btn btn-primary float-right"
                            onClick={handleBack}
                        >
                            {props.link.label}
                        </button>
                    )}
                    {props.button &&
                        <button
                            type="button"
                            className="btn btn-primary float-right"
                            onClick={props.button.action} >
                            {props.button.label}
                        </button>}
                </div>
            </div>
            {props.children && <div className="row">
                <div className="col-12">
                    {props.children}
                </div>
            </div>}
        </div>
    );
};

export default React.memo(withRouter(PageHeader));
