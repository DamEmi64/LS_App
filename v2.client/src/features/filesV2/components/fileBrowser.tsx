import { useState, useRef, useCallback, useEffect, useMemo, ChangeEvent } from "react";
import { Paper, Stack, Grid, Box } from "@mui/material";

import FolderOpenIcon from "@mui/icons-material/FolderOpen";
import InsertDriveFileIcon from "@mui/icons-material/InsertDriveFile";

import FileCard from "./fileCard";
import FolderCard from "./folderCard";
import TopBar, { BreadcrumbItem } from "./topBar";
import ShareDialog from "./sharedDialog";
import NewFolderDialog from "./newFolderDialog";

import { DirectoryDto, FileV2Dto } from "@/shared/api/generated";
import { FileItem } from "../types";
import { call } from "@/shared/components/apiClient";

export default function FileBrowser() {
  const [directoryId, setDirectoryId] = useState<string | null>(null);
  const [path, setPath] = useState<BreadcrumbItem[]>([
    { id: null, title: "All files" },
  ]);

  const [directories, setDirectories] = useState<DirectoryDto[]>([]);
  const [files, setFiles] = useState<FileV2Dto[]>([]);
  const [search, setSearch] = useState("");
  const [viewMode, setViewMode] = useState<"list" | "grid">("grid");
  const [shareTarget, setShareTarget] = useState<FileV2Dto | null>(null);
  const [newFolderOpen, setNewFolderOpen] = useState(false);

  const fileInputRef = useRef<HTMLInputElement>(null);

  const refresh = useCallback(async () => {
    const [dirs, fileList] = await Promise.all([
      call<DirectoryDto[]>(api => api.directoriesApi.get, {
        parentId: directoryId ?? undefined,
      }),
      call<FileV2Dto[]>(api => api.filesV2Api.get, {
        directoryId: directoryId ?? undefined,
        search,
      }),
    ]);

    setDirectories(dirs);
    setFiles(fileList);
  }, [directoryId, search]);

  useEffect(() => {
  refresh();
  }, [directoryId, search]);


  const deleteDirectory = async(id:string) => {
    await call<any>(api => api.directoriesApi.deleteById, {id}).then(refresh)
  }

  const handleSelectDirectory = async (id: string | null) => {
    setDirectoryId(id);

    if (id === null) {
      setPath([{ id: null, title: "All files" }]);
      return;
    }

    const trail: BreadcrumbItem[] = [];

    let current = await call<DirectoryDto>(
      api => api.directoriesApi.getById,
      { id }
    );

    while (current) {
      trail.unshift({
        id: current.id,
        title: current.title,
      });

      current = current.parentId
        ? await call<DirectoryDto>(
            api => api.directoriesApi.getById,
            { id: current.parentId }
          )
        : null;
    }

    setPath([{ id: null, title: "All files" }, ...trail]);
  };

  const handleUploadClick = () => fileInputRef.current?.click();

  const handleFileSelected = async (e: ChangeEvent<HTMLInputElement>) => {
    const selected = e.target.files?.[0];
    e.target.value = "";

    if (!selected) return;

    await call(api => api.filesV2Api.create, {
      file: selected,
      description: "",
      directoryId: directoryId ?? undefined,
    });

    refresh();
  };

  const handleCreateFolder = async (title: string) => {
    await call(api => api.directoriesApi.create, {
      createDirectoryDto: {
        title,
        parentId: directoryId ?? undefined,
      },
    });

    setNewFolderOpen(false);
    refresh();
  };

  const handleDownload = async (file: FileV2Dto) => {
    // TODO
  };

  const handleRename = async (file: FileV2Dto) => {
    const title = window.prompt("Rename file", file.title);

    if (!title || title === file.title) return;

    await call(api => api.filesV2Api.updateById, {
      id: file.id,
      updateFileDto: { title },
    });

    refresh();
  };

  const handleDelete = async (file: FileV2Dto) => {
    if (!window.confirm(`Delete "${file.title}"?`)) return;

    await call(api => api.filesV2Api.deleteById, {
      id: file.id,
    });

    refresh();
  };

  const items = useMemo<FileItem[]>(
      () => [
      ...directories.map(directory => ({
        id: directory.id,
        name: directory.title,
        icon: <FolderOpenIcon />,
        onClick: () => {
          void handleSelectDirectory(directory.id);
        },
        type: "folder" as const,
      })),
      ...files.map(file => ({
        id: file.id,
        name: file.title,
        icon: <InsertDriveFileIcon />,
        onClick: () => {
          void handleDownload(file);
        },
        onDelete: () => {
          void handleDelete(file);
        },
        onDetails: () => setShareTarget(file),
        onEdit: () => {
          void handleRename(file);
        },
        type: "file" as const,
      })),
    ],
    [directories, files]
  );

  return (
    <>
      <Paper
        variant="outlined"
        sx={{
          p: 3,
          borderRadius: 3,
        }}
      >
        <Stack spacing={3}>
          <Box>
            <TopBar
              path={path}
              onNavigate={handleSelectDirectory}
              search={search}
              onSearchChange={setSearch}
              viewMode={viewMode}
              onViewModeChange={setViewMode}
              onUploadClick={handleUploadClick}
              onNewFolderClick={() => setNewFolderOpen(true)}
            />
          </Box>

          <Grid container spacing={2}>
            {items.map(item => (
              <Grid
                key={item.id}
                size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
              >
                {item.type === "folder" ? (
                  <FolderCard
                    id={item.id}
                    name={item.name}
                    icon={item.icon}
                    onClick={item.onClick}
                    onDelete={() => deleteDirectory(item.id)}
                    type="folder"
                  />
                ) : (
                  <FileCard
                    id={item.id}
                    name={item.name}
                    icon={item.icon}
                    onClick={item.onClick}
                    onDelete={item.onDelete}
                    onDetails={item.onDetails}
                    onEdit={item.onEdit}
                    type="file"
                  />
                )}
              </Grid>
            ))}
          </Grid>
        </Stack>
      </Paper>

      <input
        ref={fileInputRef}
        type="file"
        hidden
        onChange={handleFileSelected}
      />

      <ShareDialog
        file={shareTarget}
        onClose={() => setShareTarget(null)}
        onFileUpdated={updated =>
          setFiles(prev =>
            prev.map(file => (file.id === updated.id ? updated : file))
          )
        }
      />

      <NewFolderDialog
        open={newFolderOpen}
        onClose={() => setNewFolderOpen(false)}
        onCreate={handleCreateFolder}
      />
    </>
  );
}