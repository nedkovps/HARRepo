const mapSubDirectories = (subDirectories, filePaths) => {
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

const mapRoot = root => {
    let filePaths = {};
    const rootSubDirectories = mapSubDirectories(root.subDirectories, filePaths);
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

    return { tree: treeModel, filePaths: filePaths };
}

export { mapRoot };