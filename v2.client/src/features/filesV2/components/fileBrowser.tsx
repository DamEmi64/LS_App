import { useState, useRef, useCallback, useEffect, useMemo, ChangeEvent } from "react";
import { Paper, Stack, Grid, Box } from "@mui/material";

import FolderOpenIcon from "@mui/icons-material/FolderOpen";
import InsertDriveFileIcon from "@mui/icons-material/InsertDriveFile";
import { saveAs } from 'file-saver';

import FileCard from "./fileCard";
import FolderCard from "./folderCard";
import TopBar, { BreadcrumbItem } from "./topBar";
import NewFolderDialog from "./newFolderDialog";

import { Directory, FileEditFormData, FileItem, FileV2, Privilage } from "../types";
import { call, raw } from "@/shared/components/apiClient";
import FileDialog from "./fileDialog";
import { getMimeFromExtension } from "@/lib/utils";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { useModal } from "@/shared";
import { t } from "i18next";
import FileWrapper from "./fileWrapper";
import { useAuth } from "@/features/auth/context/authProvider";
import { fi } from "date-fns/locale";

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
    const dirs = await call<Directory[]>(api => api.directoriesApi.get, {
      parentId: directoryId ?? undefined,
    });
    const fileList = await call<FileV2[]>(api => api.filesV2Api.get, {
      directoryId: directoryId ?? undefined
    })

    setDirectories(dirs);
    setFiles(fileList);
  }, [directoryId, search]);

  useEffect(() => {
    refresh();
  }, [directoryId, search]);


  const deleteDirectory = async (id: string) => {
    await call<any>(api => api.directoriesApi.deleteById, { id }).then(refresh)
  }

  const getFilePrivilage = (file: FileV2): Privilage => {

    if (!user) return Privilage.NONE;

    if (user.userId == file.owner) return Privilage.OWNER;

    if (!file.fileUsers) return Privilage.NONE;

    var fileUser = file.fileUsers.find(x => x.userId == user.userId);

    return fileUser.privilage as Privilage;
  }

  const handleSelectDirectory = async (id: string | null) => {
    setDirectoryId(id);

    if (id === null) {
      setPath([{ id: null, title: "All files" }]);
      return;
    }

    const trail: BreadcrumbItem[] = [];

    let current = await call<Directory>(
      api => api.directoriesApi.getById,
      { id }
    );

    while (current) {
      trail.unshift({
        id: current.id,
        title: current.title,
      });

      current = current.parentId
        ? await call<Directory>(
          api => api.directoriesApi.getById,
          { id: current.parentId }
        )
        : null;
    }

    setPath([{ id: null, title: "All files" }, ...trail]);
  };

  const handleUploadClick = () => {

    modal.showModal(<FileWrapper file={{} as FileV2} onSubmit={handleCreate} />);
  }

  const handleCreate = async (file: FileEditFormData) => {

    await call(api => api.filesV2Api.create, {
      file: file.File,
      description: file.description,
      directoryId: directoryId ?? undefined,
      title: file.title
    });

    modal.hideModal();
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

  const handleDownload = async (file: FileV2) => {
    raw(api => api.filesV2Api.getByIdDownload, { id: file.id })
      .then((response) => {
        let filename = file.title + response.data.extension;

        const mime = getMimeFromExtension(response.data.extension);

        const byteCharacters = atob(response.data.content);

        // convert to byte array
        const byteNumbers = new Array(byteCharacters.length);

        for (let i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);

        // create blob
        const blob = new Blob([byteArray], { type: mime });
        saveAs(blob, filename);
      })
      .catch((error) => console.error('Download failed:', error));
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

  const saveEdit = (id: string, form: FileEditFormData) => {
    call(api => api.filesV2Api.updateById, {
      id: id,
      title: form.title,
      description: form.description,
      file: form.File

    }).then(refresh);
  }

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
    call(api => api.filesV2Api.deleteById, { id: file.id }).then(refresh);
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
        let privilage = getFilePrivilage(file);

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