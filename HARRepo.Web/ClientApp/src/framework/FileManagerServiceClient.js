import { APIClient } from "./APIClient";

export class FileManagerServiceClient {

    static APIUrl = process.env.REACT_APP_FILE_MANAGER_API_URL;

    static getRepositories = async () => {
        const repos = APIClient.getSafe(`${this.APIUrl}/users/current/repositories`);
        return repos;
    }

    static getRepository = async id => {
        const repo = APIClient.getSafe(`${this.APIUrl}/repositories/${id}`);
        return repo;
    }

    static createRepository = async name => {
        await APIClient.postSafe(`${this.APIUrl}/users/current/repositories?name=${name}`);
    }

    static deleteRepository = async id => {
        await APIClient.deleteSafe(`${this.APIUrl}/repositories/${id}`);
    }

    static createFolder = async (parentId, name) => {
        await APIClient.postSafe(`${this.APIUrl}/directories?parentId=${parentId}&name=${name}`);
    }

    static deleteDirectory = async id => {
        await APIClient.deleteSafe(`${this.APIUrl}/directories/${id}`);
    }

    static uploadFile = async file => {
        await APIClient.postSafe(`${this.APIUrl}/files`, JSON.stringify(file));
    }

    static changeFileLocation = async (fileId, directoryId) => {
        await APIClient.putSafe(`${this.APIUrl}/files/${fileId}/directory/${directoryId}`);
    }

    static deleteFile = async id => {
        await APIClient.deleteSafe(`${this.APIUrl}/files/${id}`);
    }
}