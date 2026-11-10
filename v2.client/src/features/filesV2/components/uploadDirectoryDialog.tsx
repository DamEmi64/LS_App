import { useEffect, useState } from "react";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Stack,
  TextField,
  Typography,
} from "@mui/material";

interface UploadDirectoryDialogProps {
  open: boolean;
  onClose: () => void;
  onUpload: (title: string, zip: File) => Promise<void>;
}

export default function UploadDirectoryDialog({ open, onClose, onUpload }: UploadDirectoryDialogProps) {
  const [title, setTitle] = useState("");
  const [zip, setZip] = useState<File | null>(null);
  const [uploading, setUploading] = useState(false);

  useEffect(() => {
    if (open) {
      setTitle("");
      setZip(null);
      setUploading(false);
    }
  }, [open]);

  const handleZipChange = (file?: File) => {
    if (!file) return;
    setZip(file);
    if (!title.trim()) setTitle(file.name.replace(/\.zip$/i, ""));
  };

  const handleUpload = async () => {
    if (!zip || !title.trim() || uploading) return;
    setUploading(true);
    try {
      await onUpload(title.trim(), zip);
    } finally {
      setUploading(false);
    }
  };

  return (
    <Dialog open={open} onClose={uploading ? undefined : onClose} fullWidth maxWidth="sm">
      <DialogTitle>Import ZIP folder</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          <TextField
            autoFocus
            label="Folder name"
            fullWidth
            value={title}
            onChange={(event) => setTitle(event.target.value)}
            disabled={uploading}
          />
          <Button component="label" variant="outlined" disabled={uploading}>
            Choose ZIP file
            <input
              hidden
              type="file"
              accept=".zip,application/zip,application/x-zip-compressed"
              onChange={(event) => {
                handleZipChange(event.target.files?.[0]);
                event.target.value = "";
              }}
            />
          </Button>
          <Typography variant="body2" color="text.secondary">
            {zip?.name ?? "The ZIP folder structure will be preserved."}
          </Typography>
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} disabled={uploading}>Cancel</Button>
        <Button variant="contained" onClick={() => void handleUpload()} disabled={!zip || !title.trim() || uploading}>
          {uploading ? "Importing…" : "Import"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
