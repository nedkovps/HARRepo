import auth0 from 'auth0-js'

export default class Auth {
    constructor(history) {
        this.history = history;
        this.userProfile = null;
        this.auth0 = new auth0.WebAuth({
            domain: process.env.REACT_APP_AUTH0_DOMAIN,
            clientID: process.env.REACT_APP_AUTH0_CLIENT_ID,
            redirectUri: process.env.REACT_APP_CALLBACK_URL,
            audience: process.env.REACT_APP_API_AUDIENCE,
            responseType: 'token id_token',
            scope: 'openid profile email'
        });
    }

    login = () => {
        this.auth0.authorize();
    }

    handleAuthentication = () => {
        this.auth0.parseHash((err, authResult) => {
            if (authResult && authResult.accessToken && authResult.idToken) {
                this.setSession(authResult);
                this.history.push('/');
            }
            else if (err) {
                this.history.push('/');
                alert(`Error: ${err.error}`);
                console.log(err);
            }
        });
    }

    isAuthenticated = () => {
        const expiresAt = JSON.parse(localStorage.getItem('expires_at'));
        return expiresAt && new Date().getTime() < expiresAt;
    }

    setSession = (authResult) => {
        const expiresAt = JSON.stringify(authResult.expiresIn * 1000 + new Date().getTime());
        localStorage.setItem('access_token', authResult.accessToken);
        localStorage.setItem('id_token', authResult.idToken);
        localStorage.setItem('expires_at', expiresAt);
    }

    getAccessToken = () => {
        const accessToken = localStorage.getItem('access_token');
        if (!accessToken) {
            throw new Error('No access token found.');
        }
        return accessToken;
    }

    getProfile = callback => {
        if (this.userProfile) {
            callback(this.userProfile)
        };
        this.auth0.client.userInfo(this.getAccessToken(), (err, profile) => {
            if (profile) {
                this.userProfile = profile;
                callback(this.userProfile, err);
            }
        })
    }

    logout = () => {
        localStorage.removeItem('access_token');
        localStorage.removeItem('id_token');
        localStorage.removeItem('expires_at');
        this.auth0.logout({
            clientID: process.env.REACT_APP_AUTH0_CLIENT_ID,
            returnTo: process.env.REACT_APP_CALLBACK_URL.replace('callback', '')
        });
    }
}