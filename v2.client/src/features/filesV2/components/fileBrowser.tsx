import { useState, useCallback, useEffect, useMemo } from "react";
import { Paper, Stack, Grid, Box } from "@mui/material";

import FolderOpenIcon from "@mui/icons-material/FolderOpen";
import InsertDriveFileIcon from "@mui/icons-material/InsertDriveFile";

import FileCard from "./fileCard";
import FolderCard from "./folderCard";
import TopBar from "./topBar";
import NewFolderDialog from "./newFolderDialog";

import { Directory, FileEditFormData, FileItem, FileV2, Privilage } from "../types";
import FileDialog from "./fileDialog";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { useModal } from "@/shared";
import { t } from "i18next";
import FileWrapper from "./fileWrapper";
import { useAuth } from "@/features/auth/context/authProvider";
import {
  createFileEntry,
  deleteFileEntry,
  downloadFile,
  getFilePrivilage,
  loadFiles,
  updateFileEntry,
} from "../services/fileService";
import {
  createDirectoryEntry,
  deleteDirectoryEntry,
  getDirectoryPath,
  loadDirectories,
  type BreadcrumbItem,
} from "../services/directoryService";

export default function FileBrowser() {
  const [directoryId, setDirectoryId] = useState<string | null>(null);
  const [path, setPath] = useState<BreadcrumbItem[]>([
    { id: null, title: "All files" },
  ]);
  const { user } = useAuth();
  const modal = useModal();

  const [directories, setDirectories] = useState<Directory[]>([]);
  const [files, setFiles] = useState<FileV2[]>([]);
  const [search, setSearch] = useState("");
  const [viewMode, setViewMode] = useState<"list" | "grid">("grid");
  const [newFolderOpen, setNewFolderOpen] = useState(false);

  const refresh = useCallback(async () => {
    const [dirs, fileList] = await Promise.all([
      loadDirectories(directoryId),
      loadFiles(directoryId),
    ]);

    setDirectories(dirs);
    setFiles(fileList);
  }, [directoryId]);

  useEffect(() => {
    refresh();
  }, [directoryId, search]);


  const deleteDirectory = async (id: string) => {
    await deleteDirectoryEntry(id);
    await refresh();
  };

  const handleSelectDirectory = async (id: string | null) => {
    setDirectoryId(id);
    setPath(await getDirectoryPath(id));
  };

  const handleUploadClick = () => {

    modal.showModal(<FileWrapper file={{} as FileV2} onSubmit={handleCreate} />);
  }

  const handleCreate = async (file: FileEditFormData) => {
    await createFileEntry(file, directoryId);

    modal.hideModal();
    await refresh();
  };

  const handleCreateFolder = async (title: string) => {
    await createDirectoryEntry(title, directoryId);

    setNewFolderOpen(false);
    await refresh();
  };

  const handleDownload = async (file: FileV2) => {
    try {
      await downloadFile(file);
    } catch (error) {
      console.error("Download failed:", error);
    }
  };

  const handleDetails = async (file: FileV2) => {
    modal.showModal(<FileWrapper
      file={file}
      onSubmit={() => { }}
      readonly
    />)
  }

  const handleEdit = (file: FileV2, privilage: Privilage) => {

    modal.showModal(<FileDialog
      file={file}
      onSubmit={(f) => saveEdit(file.id, f)}
      onClose={() => modal.hideModal}
      privilage={privilage}
    />)
  };

  const saveEdit = async (id: string, form: FileEditFormData) => {
    await updateFileEntry(id, form);
    await refresh();
  };

  const del = (file: FileV2) => {
    modal.showModal(
      <YesNoWindow
        message={t("entity.del_info")}
        yesMethod={() => delConfirm(file)}
        noMethod={modal.hideModal}
        open
        onClose={modal.hideModal}
      />
    );
  };

  const delConfirm = async (file: FileV2) => {
    await deleteFileEntry(file.id!);
    await refresh();
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
        privilage: Privilage.READ
      })),
      ...files.map(file => {
        const privilage = getFilePrivilage(file, user);

        return {
          id: file.id,
          name: file.title,
          icon: <InsertDriveFileIcon />,
          onClick: () => {
            void handleDownload(file);
          },
          onDelete: () => {
            void del(file);
          },
          onDetails: () => handleDetails(file),
          onEdit: () => {
            void handleEdit(file, privilage);
          },
          type: "file" as const,
          privilage: privilage
      } as FileItem
      }),
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
          width: '75%',
          margin: 'auto',
          padding: 2
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
            {items.map(item => item.name.includes(search) && (
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
                    privilage={item.privilage}
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
                    privilage={item.privilage}
                  />
                )}
              </Grid>
            ))}
          </Grid>
        </Stack>
      </Paper>

      <NewFolderDialog
        open={newFolderOpen}
        onClose={() => setNewFolderOpen(false)}
        onCreate={handleCreateFolder}
      />
    </>
  );
}