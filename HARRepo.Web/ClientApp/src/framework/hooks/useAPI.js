import { useHistory } from "react-router-dom"
import { APIClient } from "../APIClient";
import { FileManagerServiceClient } from "../FileManagerServiceClient";

const useFileManagerAPI = () => {
    const history = useHistory();
    APIClient.history = history;

    return FileManagerServiceClient;
}

export default useFileManagerAPI;