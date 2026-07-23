import { useState } from "react";
import {
  Box,
  Chip,
  IconButton,
  Menu,
  MenuItem,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";
import InsertDriveFileOutlined from "@mui/icons-material/InsertDriveFileOutlined";
import FolderRounded from "@mui/icons-material/FolderRounded";
import MoreVertRounded from "@mui/icons-material/MoreVertRounded";
import { DirectoryDto, FileV2Dto } from "@/shared/api/generated";

interface FileTableProps {
  directories: DirectoryDto[];
  files: FileV2Dto[];
  onOpenDirectory: (id: string) => void;
  onDownload: (file: FileV2Dto) => void;
  onShare: (file: FileV2Dto) => void;
  onRename: (file: FileV2Dto) => void;
  onDelete: (file: FileV2Dto) => void;
}

export default function FileTable({
  directories,
  files,
  onOpenDirectory,
  onDownload,
  onShare,
  onRename,
  onDelete,
}: FileTableProps) {
  const [menuFile, setMenuFile] = useState<FileV2Dto | null>(null);
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);

  const openMenu = (e: React.MouseEvent<HTMLElement>, file: FileV2Dto) => {
    setAnchorEl(e.currentTarget);
    setMenuFile(file);
  };
  const closeMenu = () => {
    setAnchorEl(null);
    setMenuFile(null);
  };

  const isEmpty = directories.length === 0 && files.length === 0;

  if (isEmpty) {
    return (
      <Box sx={{ textAlign: "center", py: 10 }}>
        <Typography variant="body2" color="text.secondary">
          Nothing here yet. Upload a file or create a folder to get started.
        </Typography>
      </Box>
    );
  }

  return (
    <>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell sx={{ fontWeight: 600 }}>Name</TableCell>
            <TableCell sx={{ fontWeight: 600 }}>Owner</TableCell>
            <TableCell sx={{ fontWeight: 600 }}>Access</TableCell>
            <TableCell />
          </TableRow>
        </TableHead>
        <TableBody>
          {directories.map((dir) => (
            <TableRow
              key={dir.id}
              hover
              onDoubleClick={() => onOpenDirectory(dir.id)}
              sx={{ cursor: "pointer" }}
            >
              <TableCell>
                <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                  <FolderRounded fontSize="small" sx={{ color: "primary.main" }} />
                  {dir.title}
                </Box>
              </TableCell>
              <TableCell colSpan={2}>
                <Typography variant="caption" color="text.secondary">
                  {dir.fileCount} file{dir.fileCount === 1 ? "" : "s"}
                </Typography>
              </TableCell>
              <TableCell />
            </TableRow>
          ))}

          {files.map((file) => (
            <TableRow key={file.id} hover>
              <TableCell>
                <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                  <InsertDriveFileOutlined fontSize="small" sx={{ color: "text.secondary" }} />
                  {file.title}
                </Box>
              </TableCell>
              <TableCell>
                <Typography variant="body2" color="text.secondary">
                  {file.ownerLogin}
                </Typography>
              </TableCell>
              <TableCell>
                {file.public ? (
                  <Chip label="Public" size="small" color="primary" variant="outlined" />
                ) : (
                  <Chip label="Private" size="small" variant="outlined" />
                )}
              </TableCell>
              <TableCell align="right">
                <IconButton size="small" onClick={(e) => openMenu(e, file)}>
                  <MoreVertRounded fontSize="small" />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>

      <Menu anchorEl={anchorEl} open={!!menuFile} onClose={closeMenu}>
        <MenuItem
          onClick={() => {
            if (menuFile) onDownload(menuFile);
            closeMenu();
          }}
        >
          Download
        </MenuItem>
        <MenuItem
          onClick={() => {
            if (menuFile) onShare(menuFile);
            closeMenu();
          }}
        >
          Share
        </MenuItem>
        <MenuItem
          onClick={() => {
            if (menuFile) onRename(menuFile);
            closeMenu();
          }}
        >
          Rename
        </MenuItem>
        <MenuItem
          onClick={() => {
            if (menuFile) onDelete(menuFile);
            closeMenu();
          }}
          sx={{ color: "error.main" }}
        >
          Delete
        </MenuItem>
      </Menu>
    </>
  );
}
