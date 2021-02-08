import React, { useEffect, useRef, useState } from 'react';
import { Tree } from 'primereact/tree';
import { ContextMenu } from 'primereact/contextmenu';
import { Sidebar as Panel } from 'primereact/sidebar';
import PageHeader from '../components/PageHeader';
import Input from '../components/Input';
import { withRouter } from 'react-router-dom';

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
                        type: 'file'
                    };
                });
            }

            return {
                key: f.id,
                label: f.name,
                icon: 'pi pi-fw pi-folder',
                children: [...subs, ...files],
                type: 'dir'
            };
        });
    }

    const treeModel = [{
        key: root.id,
        label: root.name,
        icon: 'pi pi-fw pi-folder',
        children: mapSubDirectories(root.subDirectories)
    }];

    const [panelVisible, setPanelVisible] = useState(false);
    const [folderParentId, setFolderParentId] = useState(null);
    const [folderName, setFolderName] = useState('');
    const [selectedNodeKey, setSelectedNodeKey] = useState(null);
    const [expandedKeys, setExpandedKeys] = useState({});

    const menuRef = useRef(null);
    const fileMenuRef = useRef(null);
    const menu = [
        {
            label: 'Upload',
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
                setPanelVisible(true);
            }
        }
    ];

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
                deleteFile(selectedNodeKey);
            }
        }
    ];

    const folderNameChangeHandler = e => {
        e.persist();
        setFolderName(e.target.value);
    }

    const createFolder = async () => {
        await fetch(`https://localhost:44363/api/directories?parentId=${folderParentId}&name=${folderName}`, {
            method: 'POST',
            body: null
        });
        setPanelVisible(false);
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
        await fetch(`https://localhost:44363/api/files`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(uploadModel)
        });
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

    const deleteFile = async id => {
        await fetch(`https://localhost:44363/api/files/${selectedNodeKey}`, {
            method: 'DELETE'
        });
        props.repoUpdated(expandedKeys);
    }

    const viewFile = id => {
        props.history.push(`/ViewHAR/${filePaths[id]}`);
    }

    return <>
        <input id='selectHAR' ref={uploadHARRef} hidden type="file" onChange={uploadHARHandler} />
        <Panel position="bottom" visible={panelVisible} onHide={() => setPanelVisible(false)} style={{ height: 'auto' }}>
            <PageHeader title="Create New Folder" />
            <div>
                <Input label="Name" value={folderName} change={folderNameChangeHandler} />
            </div>
            <div className="d-block" style={{ height: '50px' }}>
                <button className="btn btn-primary float-right ml-1" onClick={createFolder}>Create</button>
                <button className="btn btn-secondary float-right" onClick={() => setPanelVisible(false)}>Cancel</button>
            </div>
        </Panel>
        <ContextMenu model={menu} ref={menuRef} onHide={() => setSelectedNodeKey(null)} />
        <ContextMenu model={fileMenu} ref={fileMenuRef} onHide={() => setSelectedNodeKey(null)} />
        <Tree className="mt-3 border-0 p-0" value={treeModel} contextMenuSelectionKey={selectedNodeKey}
            onContextMenuSelectionChange={event => setSelectedNodeKey(event.value)}
            onContextMenu={event => event.node.type === 'dir' ? menuRef.current.show(event.originalEvent) : fileMenuRef.current.show(event.originalEvent)}
            expandedKeys={expandedKeys} onToggle={e => setExpandedKeys(e.value)} />
    </>;
}

export default React.memo(withRouter(RepositoryFileManager));