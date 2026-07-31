import { Box, Breadcrumbs, Button, IconButton, InputAdornment, Link, TextField, ToggleButton, ToggleButtonGroup } from "@mui/material";
import SearchRounded from "@mui/icons-material/SearchRounded";
import UploadRounded from "@mui/icons-material/UploadRounded";
import CreateNewFolderRounded from "@mui/icons-material/CreateNewFolderRounded";
import { t } from "i18next";

export interface BreadcrumbItem {
  id: string | null;
  title: string;
}

interface TopBarProps {
  path: BreadcrumbItem[];
  onNavigate: (id: string | null) => void;
  search: string;
  onSearchChange: (value: string) => void;
  viewMode: "list" | "grid";
  onViewModeChange: (mode: "list" | "grid") => void;
  onUploadClick: () => void;
  onNewFolderClick: () => void;
}

export default function TopBar({
  path,
  onNavigate,
  search,
  onSearchChange,
  viewMode,
  onViewModeChange,
  onUploadClick,
  onNewFolderClick,
}: TopBarProps) {
  return (
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
        gap: 2,
        px: 3,
        py: 1.5,
        borderBottom: "1px solid",
        borderColor: "divider",
      }}
    >
      <Breadcrumbs sx={{ flexShrink: 0 }}>
        {path.map((item, i) => {
          const isLast = i === path.length - 1;
          return isLast ? (
            <Box key={item.id ?? "root"} component="span" sx={{ fontWeight: 600 }}>
              {item.title}
            </Box>
          ) : (
            <Link
              key={item.id ?? "root"}
              component="button"
              underline="hover"
              color="text.secondary"
              onClick={() => onNavigate(item.id)}
            >
              {item.title}
            </Link>
          );
        })}
      </Breadcrumbs>

      <TextField
        size="small"
        placeholder={t('')}
        value={search}
        onChange={(e) => onSearchChange(e.target.value)}
        sx={{ ml: "auto", width: 260 }}
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <SearchRounded fontSize="small" sx={{ color: "text.secondary" }} />
            </InputAdornment>
          ),
        }}
      />

      <IconButton onClick={onNewFolderClick} aria-label="New folder" size="small">
        <CreateNewFolderRounded fontSize="small" />
      </IconButton>

      <Button variant="contained" startIcon={<UploadRounded />} onClick={onUploadClick}>
        {t('opt.import')}
      </Button>
    </Box>
  );
}