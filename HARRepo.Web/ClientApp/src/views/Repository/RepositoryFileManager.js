﻿import React, { useEffect, useRef, useState } from 'react';
import { Tree } from 'primereact/tree';
import { ContextMenu } from 'primereact/contextmenu';
import { Sidebar as Panel } from 'primereact/sidebar';
import PageHeader from '../../components/PageHeader';
import Input from '../../components/Input';
import { withRouter } from 'react-router-dom';
import InfoPanel from '../../components/InfoPanel';
import { FileManagerServiceClient } from '../../framework/FileManagerServiceClient';

const RepositoryFileManager = props => {

    const [root, setRoot] = useState(props.root);
    const uploadHARRef = useRef(null);

    useEffect(() => {
        setRoot(props.root);
        setExpandedKeys(props.expandedKeys);
        
    }, [props.root, props.expandedKeys]);

    let filePaths = {};
    const mapSubDirectories = subDirectories => {
        return subDirectories.map(f => {

            let subs = [];
            if (f.subDirectories && f.subDirectories.length > 0) {
                subs = mapSubDirectories(f.subDirectories);
            }
            let files = [];
            if (f.files && f.files.length > 0) {
                files = f.files.map(fl => {

                    filePaths[fl.id] = fl.path;

                    return {
                        key: fl.id,
                        label: fl.name,
                        icon: 'pi pi-fw pi-file',
                        type: 'file',
                        draggable: true,
                        droppable: false
                    };
                });
            }

            return {
                key: f.id,
                label: f.name,
                icon: 'pi pi-fw pi-folder',
                children: [...subs, ...files],
                type: 'dir',
                draggable: false,
                droppable: true
            };
        });
    }

    const rootSubDirectories = mapSubDirectories(root.subDirectories);
    let rootFiles = [];
    if (root.files && root.files.length > 0) {
        rootFiles = root.files.map(fl => {

            filePaths[fl.id] = fl.path;

            return {
                key: fl.id,
                label: fl.name,
                icon: 'pi pi-fw pi-file',
                type: 'file',
                draggable: true,
                droppable: false
            };
        });
    }

    const treeModel = [{
        key: root.id,
        label: root.name,
        icon: 'pi pi-fw pi-folder',
        children: [...rootSubDirectories, ...rootFiles],
        type: 'dir',
        draggable: false,
        droppable: true
    }];

    const [panel, setPanel] = useState({ isVisible: false, type: '' });
    const [folderParentId, setFolderParentId] = useState(null);
    const [folderName, setFolderName] = useState('');
    const [selectedNodeKey, setSelectedNodeKey] = useState(null);
    const [expandedKeys, setExpandedKeys] = useState({});

    const menuRef = useRef(null);
    const fileMenuRef = useRef(null);
    const menu = [{
            label: 'Upload HAR',
            icon: 'pi pi-upload',
            command: () => {
                setFolderParentId(selectedNodeKey);
                uploadHARRef.current.click();
            }
        },
        {
            label: 'New Folder',
            icon: 'pi pi-plus',
            command: () => {
                setFolderParentId(selectedNodeKey);
                setPanel({ isVisible: true, type: 'newFolder' });
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
            label: 'Delete File',
            icon: 'pi pi-trash',
            command: () => {
                confirmDeleteFile(selectedNodeKey);
            }
        }
    ];

    const folderNameChangeHandler = e => {
        e.persist();
        setFolderName(e.target.value);
    }

    const createFolder = async () => {
        await FileManagerServiceClient.createFolder(folderParentId, folderName);
        setPanel({ isVisible: false, type: '' });
        setFolderName('');
        let expandedKeysCopy = { ...expandedKeys };
        expandedKeysCopy[folderParentId] = true;
        props.repoUpdated(expandedKeysCopy);
    }

    const uploadHAR = async (fileName, content) => {
        const uploadModel = {
            name: fileName,
            directoryId: folderParentId,
            content: content
        };
        await FileManagerServiceClient.uploadFile(uploadModel);
        let expandedKeysCopy = { ...expandedKeys };
        expandedKeysCopy[folderParentId] = true;
        props.repoUpdated(expandedKeysCopy);
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
        await FileManagerServiceClient.deleteDirectory(id);
        setPanel({ isVisible: false, type: '' });
        props.repoUpdated(expandedKeys);
    }

    const confirmDeleteFile = id => {
        setPanel({ isVisible: true, type: 'deleteFile', id: id });
    }

    const deleteFile = async id => {
        await FileManagerServiceClient.deleteFile(id);
        setPanel({ isVisible: false, type: '' });
        props.repoUpdated(expandedKeys);
    }

    const viewFile = id => {
        props.history.push(`/ViewHAR/${filePaths[id]}`);
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
        const movedFile = findMovedFile(treeModel[0], updatedTree[0]);
        await FileManagerServiceClient.changeFileLocation(movedFile.fileId, movedFile.directoryId);
        props.repoUpdated(expandedKeys);
    }

    return <>
        <input id='selectHAR' ref={uploadHARRef} hidden type="file" onChange={uploadHARHandler} />
        <Panel position="bottom" visible={panel.isVisible} onHide={() => setPanel({ isVisible: false, type: '' })} style={{ height: 'auto' }}>
            {panel.type === 'newFolder' && <>
                <PageHeader title="Create New Folder" />
                <div>
                    <Input label="Name" value={folderName} change={folderNameChangeHandler} />
                </div>
                <div className="d-block" style={{ height: '50px' }}>
                    <button className="btn btn-primary float-right ml-1" onClick={createFolder}>Create</button>
                    <button className="btn btn-secondary float-right" onClick={() => setPanel({ isVisible: false, type: '' })}>Cancel</button>
                </div>
            </>}
            {panel.type === 'deleteFolder' && <>
                <PageHeader title="Delete Folder" />
                <p>
                    Are you sure you want to delete the folder?
                </p>
                <div className="d-block" style={{ height: '50px' }}>
                    <button className="btn btn-danger float-right ml-1" onClick={() => deleteDirectory(panel.id)}>Delete</button>
                    <button className="btn btn-secondary float-right" onClick={() => setPanel({ isVisible: false, type: '' })}>Cancel</button>
                </div>
            </>}
            {panel.type === 'deleteFile' && <>
                <PageHeader title="Delete File" />
                <p>
                    Are you sure you want to delete the file?
                </p>
                <div className="d-block" style={{ height: '50px' }}>
                    <button className="btn btn-danger float-right ml-1" onClick={() => deleteFile(panel.id)}>Delete</button>
                    <button className="btn btn-secondary float-right" onClick={() => setPanel({ isVisible: false, type: '' })}>Cancel</button>
                </div>
            </>}
        </Panel>
        <ContextMenu model={menu} ref={menuRef} onHide={() => setSelectedNodeKey(null)} />
        <ContextMenu model={fileMenu} ref={fileMenuRef} onHide={() => setSelectedNodeKey(null)} />
        <InfoPanel text="Right click on a folder or file and use the context menu to manage your repo files and folders. Files can be moved around folders by drag and drop." />
        <Tree className="mt-3 border-0 p-0" value={treeModel} contextMenuSelectionKey={selectedNodeKey}
            onContextMenuSelectionChange={event => setSelectedNodeKey(event.value)} dragdropScope="demo"
            onContextMenu={event => event.node.type === 'dir' ? menuRef.current.show(event.originalEvent) : fileMenuRef.current.show(event.originalEvent)}
            expandedKeys={expandedKeys} onToggle={e => setExpandedKeys(e.value)} onDragDrop={event => moveFile(event.value)} />
    </>;
}

export default React.memo(withRouter(RepositoryFileManager));