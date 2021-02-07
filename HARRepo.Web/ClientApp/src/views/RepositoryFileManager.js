import React, { useRef, useState } from 'react';
import { Tree } from 'primereact/tree';
import { ContextMenu } from 'primereact/contextmenu';
import { Sidebar as Panel } from 'primereact/sidebar';
import PageHeader from '../components/PageHeader';
import Input from '../components/Input';

const RepositoryFileManager = props => {

    const treeModel = [{
        key: props.root.id,
        label: props.root.name,
        icon: 'pi pi-fw pi-folder',
        children: props.root.subDirectories.map(f => {
            return {
                key: f.id,
                label: f.name,
                icon: 'pi pi-fw pi-folder',
                children: []
            };
        })
    }];

    const [panelVisible, setPanelVisible] = useState(false);
    const [folderParentId, setFolderParentId] = useState(null);
    const [folderName, setFolderName] = useState('');
    const [selectedNodeKey, setSelectedNodeKey] = useState(null);
    const menuRef = useRef(null);
    const menu = [
        {
            label: 'Upload',
            icon: 'pi pi-upload',
            command: () => {
                
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

    const folderNameChangeHandler = e => {
        e.persist();
        setFolderName(e.target.value);
    }

    const createFolder = async () => {
        await fetch(`https://localhost:44363/api/directories?parentId=${folderParentId}&name=${folderName}`, {
            method: 'POST',
            body: null
        });

    }

    return <>
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
        <Tree className="mt-3" value={treeModel} contextMenuSelectionKey={selectedNodeKey}
            onContextMenuSelectionChange={event => setSelectedNodeKey(event.value)}
            onContextMenu={event => menuRef.current.show(event.originalEvent)} />
    </>;
}

export default React.memo(RepositoryFileManager);