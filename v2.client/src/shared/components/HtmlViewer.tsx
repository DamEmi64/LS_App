import { Box, IconButton, Typography } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useModal } from "@/shared/context/modal";
import { useTranslation } from "react-i18next";

type HtmlViewerProps = {
  title: string;
  html: string;
};

function normalizeHtml(html: string) {
  let source = html.replace(/^\uFEFF/, "");

  // Some exports contain a JSON-serialized HTML string instead of the HTML itself.
  try {
    const decoded: unknown = JSON.parse(source);
    if (typeof decoded === "string") source = decoded;
  } catch {
    // The source is regular HTML, not a JSON string.
  }

  // Recover newline escapes in single-line HTML exports without rewriting normal
  // HTML or script/style strings that intentionally contain backslash escapes.
  if (!/[\r\n]/.test(source) && /\\n|\\r/.test(source) && /<\/?[a-z][^>]*>/i.test(source)) {
    source = source.replace(/\\r\\n|\\n|\\r/g, "\n");
  }

  return source;
}

export default function HtmlViewer({ title, html }: HtmlViewerProps) {
  const modal = useModal();
  const { t } = useTranslation();
  const normalizedHtml = normalizeHtml(html);
  const readableHtml = normalizedHtml.match(/<\/head\s*>/i)
    ? normalizedHtml.replace(
        /<\/head\s*>/i,
        '<style>html,body{background:#fff!important;color:#111!important}body *{color:#111!important}</style></head>'
      )
    : `<style>html,body{background:#fff!important;color:#111!important}body *{color:#111!important}</style>${normalizedHtml}`;

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
