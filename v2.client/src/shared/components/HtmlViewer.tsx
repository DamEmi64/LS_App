import { Box, IconButton, Typography } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useModal } from "@/shared/context/modal";

type HtmlViewerProps = {
  title: string;
  html: string;
};

export default function HtmlViewer({ title, html }: HtmlViewerProps) {
  const modal = useModal();

  return (
    <Box sx={{ height: "80vh", display: "flex", flexDirection: "column", gap: 1 }}>
      <Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", gap: 1 }}>
        <Typography variant="h6" noWrap>{title}</Typography>
        <IconButton aria-label="Close viewer" onClick={modal.hideModal}><CloseIcon /></IconButton>
      </Box>
      <Box component="iframe" title={title} srcDoc={html} sandbox="" sx={{ border: 0, width: "100%", flex: 1, minHeight: 0 }} />
    </Box>
  );
}