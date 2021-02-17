import React, { useEffect, useRef, useState } from 'react';
import { Tree } from 'primereact/tree';
import { ContextMenu } from 'primereact/contextmenu';
import { Sidebar as Panel } from 'primereact/sidebar';
import { withRouter } from 'react-router-dom';
import InfoPanel from '../../components/InfoPanel';
import useFileManagerAPI from '../../framework/hooks/useFileManagerAPI';
import CreateFolderPanelContent from './CreateFolderPanelContent';
import DeleteFolderPanelContent from './DeleteFolderPanelContent';
import DeleteFilePanelContent from './DeleteFilePanelContent';
import ShareFilePanelContent from './ShareFilePanelContent';
import FileActions from './FileActions';
import FolderActions from './FolderActions';
import { mapRoot } from '../../framework/FileManagerHelpers';
import Loader from '../../components/Loader';

const RepositoryFileManager = props => {

    const client = useFileManagerAPI();
    const [root, setRoot] = useState(props.root);
    const [isInProcess, setIsInProcess] = useState(false);
    const uploadHARRef = useRef(null);

    useEffect(() => {
        setRoot(props.root);
        setExpandedKeys(props.expandedKeys);
        
    }, [props.root, props.expandedKeys]);

    const mappedRoot = mapRoot(root);

    const [panel, setPanel] = useState({ isVisible: false, type: '' });
    const [folderParentId, setFolderParentId] = useState(null);
    const [selectedNodeKey, setSelectedNodeKey] = useState(null);
    const [expandedKeys, setExpandedKeys] = useState({});

    const uploadHARCommand = key => {
        setFolderParentId(key);
        uploadHARRef.current.click();
    }

    const newFolderCommand = key => {
        setFolderParentId(key);
        setPanel({ isVisible: true, type: 'newFolder' });
    }

    const deleteFolderCommand = key => {
        confirmDeleteDirectory(key);
    }

    const menuRef = useRef(null);
    const fileMenuRef = useRef(null);
    const menu = [{
            label: 'Upload HAR',
            icon: 'pi pi-upload',
            command: () => {
                uploadHARCommand(selectedNodeKey);
            }
        },
        {
            label: 'New Folder',
            icon: 'pi pi-plus',
            command: () => {
                deleteFolderCommand(selectedNodeKey);
            }
        }];

    if (selectedNodeKey !== root.id) {
        menu.push({
            label: 'Delete Folder',
            icon: 'pi pi-trash',
            command: () => {
                confirmDeleteDirectory(selectedNodeKey);
            }
        });
    }

    const fileMenu = [
        {
            label: 'View',
            icon: 'pi pi-eye',
            command: () => {
                viewFile(selectedNodeKey);
            }
        },
        {
            label: "Share",
            icon: 'pi pi-share-alt',
            command: () => {
                initFileShare(selectedNodeKey);
            }
        },
        {
            label: 'Delete File',
            icon: 'pi pi-trash',
            command: () => {
                confirmDeleteFile(selectedNodeKey);
            }
        }
    ];

    const createFolder = async folderName => {
        setIsInProcess(true);
        await client.createFolder(folderParentId, folderName);
        setPanel({ isVisible: false, type: '' });
        let expandedKeysCopy = { ...expandedKeys };
        expandedKeysCopy[folderParentId] = true;
        props.repoUpdated(expandedKeysCopy);
        setIsInProcess(false);
    }

    const uploadHAR = async (fileName, content) => {
        setIsInProcess(true);
        const uploadModel = {
            name: fileName,
            directoryId: folderParentId,
            content: content
        };
        await client.uploadFile(uploadModel);
        let expandedKeysCopy = { ...expandedKeys };
        expandedKeysCopy[folderParentId] = true;
        props.repoUpdated(expandedKeysCopy);
        setIsInProcess(false);
    }

    const uploadHARHandler = e => {
        const file = e.target.files[0];
        const reader = new FileReader();
        reader.onload = function (evt) {
            uploadHAR(file.name, evt.target.result);
        };
        reader.readAsBinaryString(file);
    }

    const confirmDeleteDirectory = id => {
        setPanel({ isVisible: true, type: 'deleteFolder', id: id });
    }

    const deleteDirectory = async id => {
        setIsInProcess(true);
        await client.deleteDirectory(id);
        setPanel({ isVisible: false, type: '' });
        props.repoUpdated(expandedKeys);
        setIsInProcess(false);
    }

    const confirmDeleteFile = id => {
        setPanel({ isVisible: true, type: 'deleteFile', id: id });
    }

    const deleteFile = async id => {
        setIsInProcess(true);
        await client.deleteFile(id);
        setPanel({ isVisible: false, type: '' });
        props.repoUpdated(expandedKeys);
        setIsInProcess(false);
    }

    const initFileShare = id => {
        setPanel({ isVisible: true, type: 'shareFile', id: id });
    }

    const shareFile = async (id, userEmail, comment) => {
        await client.shareFile({
            fileId: id,
            userEmail: userEmail,
            comment: comment
        });
        setPanel({ isVisible: false, type: '' });
    }

    const viewFile = id => {
        props.history.push(`/ViewHAR/${mappedRoot.filePaths[id]}`);
    }

    const findMovedFile = (original, modified) => {
        if (original.children.length >= modified.children.length) {
            const originalFolders = original.children.filter(x => x.type === 'dir');
            const modifiedFolders = modified.children.filter(x => x.type === 'dir');
            for (var i = 0; i < originalFolders.length; i++) {
                var movedFile = findMovedFile(originalFolders[i], modifiedFolders[i]);
                if (movedFile) {
                    return movedFile;
                }
            }
        }
        else {
            const originalFilesKeys = original.children.filter(x => x.type === 'file').map(x => x.key);
            const fileId = modified.children.find(x => x.type === 'file' && !originalFilesKeys.includes(x.key)).key;
            const directoryId = modified.key;
            return { fileId, directoryId };
        }
    }

    const moveFile = async updatedTree => {
        const movedFile = findMovedFile(mappedRoot.tree[0], updatedTree[0]);
        await client.changeFileLocation(movedFile.fileId, movedFile.directoryId);
        props.repoUpdated(expandedKeys);
    }

    const expandOrCondenseFolder = key => {
        let expandedKeysCopy = { ...expandedKeys };
        if (expandedKeysCopy[key]) {
            delete expandedKeysCopy[key];
        }
        else {
            expandedKeysCopy[key] = true;
        }
        setExpandedKeys(expandedKeysCopy);
    }

    const nodeTemplate = (node) => {
        if (node.type === 'file') {
            return <FileActions id={node.key} name={node.label} view={viewFile} initShare={initFileShare} confirmDelete={confirmDeleteFile} />
        }
        else {
            return <FolderActions id={node.key} name={node.label} isRoot={node.key === root.id} uploadHAR={uploadHARCommand}
                createNew={newFolderCommand} deleteFolder={deleteFolderCommand} expandOrCondense={expandOrCondenseFolder} />
        }
    }

    return !isInProcess ? <>
        <input id='selectHAR' ref={uploadHARRef} hidden type="file" onChange={uploadHARHandler} />
        <Panel position="bottom" visible={panel.isVisible} onHide={() => setPanel({ isVisible: false, type: '' })} style={{ height: 'auto' }}>
            {panel.type === 'newFolder' && <CreateFolderPanelContent create={createFolder} setPanel={setPanel} />}
            {panel.type === 'deleteFolder' && <DeleteFolderPanelContent id={panel.id} delete={deleteDirectory} setPanel={setPanel} />}
            {panel.type === 'deleteFile' && <DeleteFilePanelContent id={panel.id} delete={deleteFile} setPanel={setPanel} />}
            {panel.type === 'shareFile' && <ShareFilePanelContent id={panel.id} share={shareFile} setPanel={setPanel} />}
        </Panel>
        <ContextMenu model={menu} ref={menuRef} onHide={() => setSelectedNodeKey(null)} />
        <ContextMenu model={fileMenu} ref={fileMenuRef} onHide={() => setSelectedNodeKey(null)} />
        <InfoPanel text="Right click on a folder or file and use the context menu to manage your repo files and folders. Alternatively the action buttons on the right can be used. Files can be moved around folders by drag and drop." />
        <Tree className="mt-3 border-0 p-0" value={mappedRoot.tree} contextMenuSelectionKey={selectedNodeKey} nodeTemplate={nodeTemplate}
            onContextMenuSelectionChange={event => setSelectedNodeKey(event.value)} dragdropScope="demo"
            onContextMenu={event => event.node.type === 'dir' ? menuRef.current.show(event.originalEvent) : fileMenuRef.current.show(event.originalEvent)}
            expandedKeys={expandedKeys} onToggle={e => setExpandedKeys(e.value)} onDragDrop={event => moveFile(event.value)} />
    </> : <Loader />;
}

export default React.memo(withRouter(RepositoryFileManager));