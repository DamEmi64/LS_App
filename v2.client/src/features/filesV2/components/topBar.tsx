import {
  Box,
  Breadcrumbs,
  Button,
  IconButton,
  InputAdornment,
  Link,
  TextField,
} from "@mui/material";
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
  onUploadClick,
  onNewFolderClick,
}: TopBarProps) {
  return (
    <Box
      sx={{
        display: "flex",
        flexWrap: "wrap",
        alignItems: "center",
        gap: 2,
        px: { xs: 1, sm: 2, md: 3 },
        py: 2,
        borderBottom: "1px solid",
        borderColor: "divider",
      }}
    >
      <Breadcrumbs
        sx={{
          width: { xs: "100%", md: "auto" },
          overflow: "hidden",
          "& .MuiBreadcrumbs-ol": {
            flexWrap: "wrap",
          },
        }}
      >
        {path.map((item, i) => {
          const isLast = i === path.length - 1;

          return isLast ? (
            <Box
              key={item.id ?? "root"}
              component="span"
              sx={{ fontWeight: 600 }}
            >
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
        placeholder={t("search")}
        value={search}
        onChange={(e) => onSearchChange(e.target.value)}
        sx={{
          flexGrow: 1,
          minWidth: { xs: "100%", sm: 220 },
          order: { xs: 3, md: 2 },
        }}
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <SearchRounded
                fontSize="small"
                sx={{ color: "text.secondary" }}
              />
            </InputAdornment>
          ),
        }}
      />

      <Box
        sx={{
          display: "flex",
          gap: 1,
          ml: { xs: 0, md: "auto" },
          width: { xs: "100%", md: "auto" },
          justifyContent: { xs: "space-between", md: "flex-end" },
          order: { xs: 2, md: 3 },
        }}
      >
        <IconButton
          onClick={onNewFolderClick}
          aria-label="New folder"
        >
          <CreateNewFolderRounded />
        </IconButton>

        <Button
          variant="contained"
          startIcon={<UploadRounded />}
          onClick={onUploadClick}
          fullWidth={false}
        >
          {t("opt.import")}
        </Button>
      </Box>
    </Box>
  );
}