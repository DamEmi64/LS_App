import {
  Divider,
  Grid,
  Paper,
  Typography,
} from "@mui/material";
import { t } from "i18next";
import FileWrapper from "./fileWrapper";
import ShareWrapper from "./shareWrapper";
import { FileEditFormData, FileV2 } from "../types";

interface FileDialogProps {
  file: FileV2 | null;
  onSubmit: (file: FileEditFormData) => void;
  onClose: () => void;
}

export default function FileDialog({ file, onClose, onSubmit }: FileDialogProps) {


  return (
    <Paper variant="outlined" sx={{ p: 3, borderRadius: 2}}>
      <Typography variant="h6" gutterBottom>
        {t("files.name")}
      </Typography>

      <Divider sx={{ mb: 3 }} />

      <Grid container spacing={3}>
        <Grid size={{ xs: 12}}>
          <FileWrapper
            file={file}
            onSubmit={onSubmit}
          />
        </Grid>

        <Grid size={{ xs: 12 }}>
          <ShareWrapper
            file={file}
            onSubmit={() => {}}
          />
        </Grid>
      </Grid>
    </Paper>
  );
}