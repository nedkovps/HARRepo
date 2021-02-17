export class APIClient {

    static history = null;

    static getHeaders = () => {
        let headers = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        const token = localStorage.getItem('access_token');
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        var userProfile = localStorage.getItem('profile');
        if (userProfile) {
            headers.UserId = JSON.parse(userProfile).email;
        }
        return headers;
    }

    static fetchSafe = async (url, method, data) => {
        try {
            const response = await fetch(url, {
                method: method,
                headers: this.getHeaders(),
                body: data ? data : null
            });
            const result = await response.text();
            let resultJSON = result ? JSON.parse(result) : {};
            if (response.status === 200) {
                return resultJSON;
            }
            else if (response.status === 401) {
                if (this.history) {
                    this.history.push({
                        pathname: '/AccessDenied',
                        state: resultJSON
                    });
                }
            }
            else if (response.status === 404) {
                if (this.history) {
                    this.history.push({
                        pathname: '/NotFound',
                        state: resultJSON
                    });
                }
            }
            else if (response.status === 400) {
                this.history.push({
                    pathname: '/Error',
                    state: resultJSON
                });
            }
            else {
                if (this.history) {
                    this.history.push('/Error');
                }
            }
        }
        catch (error) {
            this.history.push('/Error');
        }
    }

    static getSafe = async url => {
        return await this.fetchSafe(url, 'GET');
    }

    static postSafe = async (url, data) => {
        return await this.fetchSafe(url, 'POST', data);
    }

    static putSafe = async url => {
        return await this.fetchSafe(url, 'PUT');
    }

    static deleteSafe = async url => {
        return await this.fetchSafe(url, 'DELETE');
    }

}