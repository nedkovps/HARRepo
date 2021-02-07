import React from 'react';
import classNames from 'classnames';
import PropTypes from 'prop-types';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';

const input = props => {

    let classes = classNames({
        'form-control': true,
        'is-invalid': props.errors
    });

    if (props.inputClasses) {
        classes = classNames({
            'form-control': true,
            'is-invalid': props.errors
        }, ...props.inputClasses);
    }

    return (
        <div className="form-group">
            {props.label && <label>{props.label}</label>}
            {!props.isMultiline ? <InputText {...props.config}
                className={classes}
                value={props.value ?? ''}
                onChange={props.change}
                onBlur={props.blur}
                onFocus={props.focus}
                tooltip={props.errors ? props.errors : null} /> :
                <InputTextarea {...props.config}
                    rows={5}
                    className={classes}
                    value={props.value ?? ''}
                    onChange={props.change}
                    onBlur={props.blur}
                    onFocus={props.focus}
                    tooltip={props.errors ? props.errors : null} />}
        </div>
    );
}

// Validating props
input.propTypes = {
    label: PropTypes.string,
    errors: PropTypes.string,
    value: PropTypes.oneOfType([
        PropTypes.string,
        PropTypes.number
    ]),
    change: PropTypes.func.isRequired,
    config: PropTypes.object,
    inputClasses: PropTypes.arrayOf(PropTypes.string)
}


export default React.memo(input);