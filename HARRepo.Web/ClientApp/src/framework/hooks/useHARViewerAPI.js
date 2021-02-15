import { useHistory } from "react-router-dom"
import { APIClient } from "../APIClient";
import { HARViewerServiceClient } from "../HARViewerServiceClient";

const useHARViewerAPI = () => {
    const history = useHistory();
    APIClient.history = history;

    return HARViewerServiceClient;
}

export default useHARViewerAPI;