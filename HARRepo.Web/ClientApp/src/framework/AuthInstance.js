import Auth from './Auth';

export class AuthInstance {
    static _instance = null;

    static GetInstance(history) {
        if (this._instance === null) {
            this._instance = new Auth(history);
        }
        return this._instance;
    }
}

export default AuthInstance;