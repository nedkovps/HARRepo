export class FileManagerServiceClient {

    static APIUrl = process.env.REACT_APP_FILE_MANAGER_API_URL;

    static getRepositories = async userId => {
        const response = await fetch(`${this.APIUrl}/users/${userId}/repositories`);
        const repos = await response.json();
        return repos;
    }

    static getRepository = async id => {
        const response = await fetch(`${this.APIUrl}/repositories/${id}`);
        const repo = await response.json();
        return repo;
    }

    static createRepository = async (userId, name) => {
        await fetch(`${this.APIUrl}/users/${userId}/repositories?name=${name}`, {
            method: 'POST',
            body: null
        });
    }

    static deleteRepository = async id => {
        await fetch(`${this.APIUrl}/repositories/${id}`, {
            method: 'DELETE'
        });
    }

    static createFolder = async (parentId, name) => {
        await fetch(`${this.APIUrl}/directories?parentId=${parentId}&name=${name}`, {
            method: 'POST',
            body: null
        });
    }

    static deleteDirectory = async id => {
        await fetch(`${this.APIUrl}/directories/${id}`, {
            method: 'DELETE'
        });
    }

    static uploadFile = async file => {
        await fetch(`${this.APIUrl}/files`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(file)
        });
    }

    static changeFileLocation = async (fileId, directoryId) => {
        await fetch(`${this.APIUrl}/files/${fileId}/directory/${directoryId}`, {
            method: 'PUT'
        });
    }

    static deleteFile = async id => {
        await fetch(`${this.APIUrl}/files/${id}`, {
            method: 'DELETE'
        });
    }
}