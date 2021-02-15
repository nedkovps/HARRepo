import React, { useState, useEffect, useCallback } from 'react';
import { withRouter } from 'react-router';
import AuthInstance from '../framework/AuthInstance';
import { faSignInAlt as signIn, faCaretDown as down } from '@fortawesome/free-solid-svg-icons';
import { ActionButton } from './ActionButton';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Menu } from 'primereact/menu';

const UserActions = props => {

    const { login, logout, isAuthenticated, getProfile } = AuthInstance.GetInstance(props.history);
    const [profile, setProfile] = useState(null);
    const isUserAuthenticated = isAuthenticated();

    const doLogout = () => {
        logout();
        localStorage.removeItem('profile');
    }

    let menu = null;
    let menuItems = [
        {
            label: 'Options',
            items: [{ label: 'My Repos', icon: 'pi pi-fw pi-bars', command: () => props.history.push('/') }]
        },
        {
            label: 'Account',
            items: [{ label: 'Sign Out', icon: 'pi pi-fw pi-sign-out', command: doLogout }]
        }
    ];

    const loadUserProfile = useCallback(() => {
        if (!isUserAuthenticated) {
            return;
        }
        const userProfile = localStorage.getItem('profile');
        if (userProfile) {
            setProfile(JSON.parse(userProfile));
            return;
        }
        getProfile((profile, error) => {
            setProfile(profile);
            localStorage.setItem('profile', JSON.stringify(profile));
        });
    }, [isUserAuthenticated, getProfile]);

    useEffect(() => {
        loadUserProfile();
    }, [loadUserProfile]);

    const signedInTemplate = () => {
        return <div className="clickable active" style={{ width: 'fit-content', marginTop: '7.5px', marginLeft: '3px' }} onClick={e => menu.toggle(e)}>
            <img style={{ maxWidth: '24px', maxHeight: '24px', borderRadius: '50%', marginRight: '2px', backgroundColor: '#e9ecef', border: '1px solid #e9ecef' }} src={profile.picture} alt="profile" />
            <FontAwesomeIcon style={{ marginLeft: '2px' }} icon={down} />
            <Menu model={menuItems} popup={true} ref={el => menu = el} style={{ marginTop: '4px' }} />
        </div>;
    }

    return isUserAuthenticated ?
        profile && signedInTemplate() :
        <ActionButton className="btn btn-outline clickable" style={{ marginTop: props.isSmall ? '2px' : '0px' }} click={login} icon={signIn} tooltip="Sign In" />;
}

export default React.memo(withRouter(UserActions));