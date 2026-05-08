import React, { useRef } from "react";
import {
  Box,
  Button,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Typography,
  Stack,
  useTheme,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import UploadFileIcon from "@mui/icons-material/UploadFile";
import DownloadIcon from "@mui/icons-material/Download";
import { FileItem } from "../types";
import { download } from "@/lib/utils";
import { useTranslation } from "react-i18next";

type Props = {
  files: FileItem[];
  editMode: boolean;
  remove: (id: string) => Promise<void>;
  add: (file: File) => Promise<void>;

};

export default function FileManager({ files, editMode, add, remove }: Props) {
  const inputRef = useRef<HTMLInputElement | null>(null);
  const { t } = useTranslation();

  const handleAddClick = () => {
    inputRef.current?.click();
  };

  const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    const selected = e.target.files;
    if (!selected) return;

    add(selected.item(0));

    // reset input so same file can be selected again
    e.target.value = "";
  };

  const theme = useTheme();

    const textColor = theme.palette.mode === 'dark'
        ? theme.palette.text.primary
        : theme.palette.text.secondary;

  const downloadFile = async (file: FileItem) => {
    download(file.content, file.title);
  };

  return (
    <Box>
      <Stack
        direction="row"
        justifyContent="space-between"
        alignItems="center"
        mb={2}
      >
        <Stack spacing={0.3}>
          <Typography variant="h6" fontWeight={700} color={textColor}>
            {t("files.title")}
          </Typography>

          <Typography variant="body2" color={textColor}>
            {files.length}{" "}
            {files.length === 1
              ? t("files.single")
              : t("files.multiple")}
          </Typography>
        </Stack>

        {editMode && (
          <IconButton onClick={() => handleAddClick()}>
            <UploadFileIcon/>
          </IconButton>
        )}
      </Stack>

      <input
        ref={inputRef}
        type="file"
        hidden
        onChange={handleFileSelect}
      />

      {files.length === 0 ? (
        <Box
          sx={{
            border: (theme) => `1px dashed ${theme.palette.divider}`,
            borderRadius: 3,
            py: 5,
            px: 2,
            textAlign: "center",
            bgcolor: "background.default",
          }}
        >
          <UploadFileIcon
            sx={{
              fontSize: 40,
              color: "text.disabled",
              mb: 1,
            }}
          />

          <Typography fontWeight={600} color={textColor}>
            {t("files.empty")}
          </Typography>
        </Box>
      ) : (
        <List disablePadding>
          {files.map((file, index) => (
            <ListItem
              key={file.id}
              sx={{
                px: 2,
                py: 1.5,
                mb: index !== files.length - 1 ? 1 : 0,
                borderRadius: 3,
                border: (theme) => `1px solid ${theme.palette.divider}`,
                transition: "0.2s ease",
                "&:hover": {
                  bgcolor: "action.hover",
                },
              }}
              secondaryAction={
                <Stack direction="row" spacing={1}>
                  {!editMode && (
                    <IconButton
                      edge="end"
                      onClick={() => downloadFile(file)}
                      sx={{
                        borderRadius: 2,
                      }}
                    >
                      <DownloadIcon fontSize="small" />
                    </IconButton>
                  )}

                  {editMode && (
                    <IconButton
                      edge="end"
                      color="error"
                      onClick={() => remove(file.id)}
                      sx={{
                        borderRadius: 2,
                      }}
                    >
                      <DeleteIcon fontSize="small" />
                    </IconButton>
                  )}
                </Stack>
              }
            >
              <ListItemText
                primary={
                  <Typography fontWeight={600} color={textColor}>
                    {file.title}
                  </Typography>
                }
              />
            </ListItem>
          ))}
        </List>
      )}
    </Box>
  );
}