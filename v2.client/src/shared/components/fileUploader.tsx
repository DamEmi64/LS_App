import { useCallback, useState } from "react";
import { useDropzone } from "react-dropzone";
import {
  Paper,
  Stack,
  Typography,
  Button,
} from "@mui/material";
import {t} from "i18next";
import UploadFileIcon from "@mui/icons-material/UploadFile";

type FileUploaderProps = {
  onUpload: (file: File) => void;
  accept?: Record<string, string[]>;
};

export default function FileUploader({
  onUpload,
  accept,
}: FileUploaderProps) {
  const [file, setFile] = useState<File | null>(null);

  const onDrop = useCallback(
    (acceptedFiles: File[]) => {
      if (!acceptedFiles.length) return;

      const selected = acceptedFiles[0];
      setFile(selected);
      onUpload(selected);
    },
    [onUpload]
  );

  const { getRootProps, getInputProps, open, isDragActive } = useDropzone({
    onDrop,
    accept,
    multiple: false,
    noKeyboard: true,
  });

  return (
    <Paper
      {...getRootProps()}
      variant="outlined"
      sx={{
        p: 4,
        borderStyle: "dashed",
        borderWidth: 2,
        textAlign: "center",
        cursor: "pointer",
        transition: "0.2s",
        bgcolor: isDragActive ? "action.hover" : "background.paper",
        "&:hover": {
          bgcolor: "action.hover",
        },
      }}
    >
      <input {...getInputProps()} />

      <Stack spacing={2} alignItems="center">
        <UploadFileIcon color="primary" sx={{ fontSize: 48 }} />

        {file ? (
          <>
            <Typography variant="h6">{file.name}</Typography>
            <Typography variant="body2" color="text.secondary">
              {t('opt.file.drop_replace')}
            </Typography>

            <Button variant="outlined" onClick={open}>
              {t("opt.file.select")}
            </Button>
          </>
        ) : (
          <>
            <Typography variant="h6">
              {isDragActive
                ? t('opt.file.drop_active')
                : t('opt.file.drop')}
            </Typography>
          </>
        )}
      </Stack>
    </Paper>
  );
}