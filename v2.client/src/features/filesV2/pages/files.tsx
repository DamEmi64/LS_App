import { useCallback, useEffect, useRef, useState } from "react";
import { Box } from "@mui/material";
import { DirectoryDto, FileDto, FileV2Dto } from "@/shared/api/generated";
import FileTable from "../components/fileTable";
import NewFolderDialog from "../components/newFolderDialog";
import ShareDialog from "../components/sharedDialog";
import Sidebar from "../components/sideBar";
import TopBar, { BreadcrumbItem } from "../components/topBar";
import { call } from "@/shared";
import FileBrowser from "../components/fileBrowser";

export default function Files() {
    const [view, setView] = useState<"files" | "shared" | "trash">("files");
    const [directoryId, setDirectoryId] = useState<string | null>(null);
    const [path, setPath] = useState<BreadcrumbItem[]>([{ id: null, title: "All files" }]);
    const [directories, setDirectories] = useState<DirectoryDto[]>([]);
    const [files, setFiles] = useState<FileV2Dto[]>([]);
    const [search, setSearch] = useState("");
    const [viewMode, setViewMode] = useState<"list" | "grid">("list");
    const [shareTarget, setShareTarget] = useState<FileV2Dto | null>(null);
    const [newFolderOpen, setNewFolderOpen] = useState(false);
    const fileInputRef = useRef<HTMLInputElement | null>(null);

    const refresh = useCallback(async () => {
        const [dirs, fileList] = await Promise.all([
            call<DirectoryDto[]>(api => api.directoriesApi.get, { parentId: directoryId || undefined }),
            call<FileV2Dto[]>(api => api.filesV2Api.get, { directoryId: directoryId || undefined, search }),
        ]);
        setDirectories(dirs);
        setFiles(fileList);
    }, [directoryId, search]);

    useEffect(() => {
        refresh();
    }, [refresh]);

    const handleSelectDirectory = async (id: string | null) => {
        setDirectoryId(id);
        if (id === null) {
            setPath([{ id: null, title: "All files" }]);
            return;
        }
        // Walk up via repeated Get calls to rebuild the breadcrumb trail.
        const trail: BreadcrumbItem[] = [];
        let current: DirectoryDto | null = await call<DirectoryDto>(api => api.directoriesApi.getById, { id });
        while (current) {
            trail.unshift({ id: current.id, title: current.title });
            current = current.parentId ? await call<DirectoryDto>(api => api.directoriesApi.getById, { id: current.parentId }) : null;
        }
        setPath([{ id: null, title: "All files" }, ...trail]);
    };

    const handleUploadClick = () => fileInputRef.current?.click();

    const handleFileSelected = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const selected = e.target.files?.[0];
        e.target.value = "";
        if (!selected) return;

        await call(api => api.filesV2Api.create, {
            file: selected,
            description: '',
            directoryId: directoryId ?? undefined,
        });
        refresh();
    };

    const handleCreateFolder = async (title: string) => {
        await call(api => api.directoriesApi.create, { createDirectoryDto: { title, parentId: directoryId ?? undefined } });
        setNewFolderOpen(false);
        refresh();
    };

    const handleDownload = async (file: FileDto) => {
        //TODO
    };

    const handleRename = async (file: FileDto) => {
        const title = window.prompt("Rename file", file.title);
        if (!title || title === file.title) return;
        await call<FileDto>(api => api.filesV2Api.updateById, { id: file.id, updateFileDto: { title } });
        refresh();
    };

    const handleDelete = async (file: FileDto) => {
        if (!window.confirm(`Delete "${file.title}"? This can't be undone.`)) return;
        await call<FileDto>(api => api.filesV2Api.deleteById, { id: file.id });
        refresh();
    };

    return (<FileBrowser/>);
}