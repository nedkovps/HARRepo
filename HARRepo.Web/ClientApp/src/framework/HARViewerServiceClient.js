export class HARViewerServiceClient {

    static APIUrl = process.env.REACT_APP_HAR_VIEWERAPI_URL;

    static getHAR = async id => {
        const HARResponse = await fetch(`${this.APIUrl}?id=${id}`);
        const HAR = await HARResponse.json();
        return HAR;
    }

}