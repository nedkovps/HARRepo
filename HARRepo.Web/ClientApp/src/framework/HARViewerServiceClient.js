import { APIClient } from "./APIClient";

export class HARViewerServiceClient {

    static APIUrl = process.env.REACT_APP_HAR_VIEWERAPI_URL;

    static getHAR = async id => {
        const HAR = APIClient.getSafe(`${this.APIUrl}/HAR?id=${id}`);
        return HAR;
    }

}