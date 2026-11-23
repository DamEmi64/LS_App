import { Box, IconButton, Typography } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useModal } from "@/shared/context/modal";
import { useTranslation } from "react-i18next";

type HtmlViewerProps = {
  title: string;
  html: string;
};

export default function HtmlViewer({ title, html }: HtmlViewerProps) {
  const modal = useModal();
  const { t } = useTranslation();
  const readableHtml = html.match(/<\/head\s*>/i)
    ? html.replace(
        /<\/head\s*>/i,
        '<style>html,body{background:#fff!important;color:#111!important}body *{color:#111!important}</style></head>'
      )
    : `<style>html,body{background:#fff!important;color:#111!important}body *{color:#111!important}</style>${html}`;

  return (
    <Box sx={{ height: "80vh", display: "flex", flexDirection: "column", gap: 1 }}>
      <Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", gap: 1 }}>
        <Typography variant="h6" noWrap>{title}</Typography>
        <IconButton aria-label={t("opt.close")} onClick={modal.hideModal}><CloseIcon /></IconButton>
      </Box>
      <Box component="iframe" title={title} srcDoc={readableHtml} sandbox="" sx={{ border: 0, width: "100%", flex: 1, minHeight: 0 }} />
    </Box>
  );
}
