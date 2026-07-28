import React, { useState } from 'react';
import { Card, CardActionArea, CardContent, CardActions, Typography, IconButton, Menu, MenuItem, Box } from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { t } from 'i18next';
import { FileItem, Privilage } from '../types'


const FileCard: React.FC<FileItem> = ({ name, icon, onClick, onDetails, onEdit, onDelete, privilage }) => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleMenuOpen = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleMenuItemClick = (callback?: () => void) => {
    return (event: React.MouseEvent<HTMLElement>) => {
      event.stopPropagation();
      callback?.();
      handleMenuClose();
    };
  };

  return (
    <Card variant="outlined" sx={{ width: "100%" }}>
      <CardActionArea onClick={onClick}>
        <CardContent
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            py: 1.5,
          }}
        >
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              gap: 2,
              flex: 1,
              minWidth: 0,
              direction: "row"
            }}
          >
            {icon}

            <Typography noWrap variant="subtitle1">
              {name}
            </Typography>
          </Box>

          <IconButton
            size="small"
            aria-label="more options"
            aria-controls={open ? "file-item-menu" : undefined}
            aria-haspopup="true"
            aria-expanded={open ? "true" : undefined}
            onClick={handleMenuOpen}
          >
            <MoreVertIcon fontSize="small" />
          </IconButton>

          <Menu
            id="file-item-menu"
            anchorEl={anchorEl}
            open={open}
            onClose={handleMenuClose}
            anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
            transformOrigin={{ vertical: "top", horizontal: "right" }}
            onClick={(event) => event.stopPropagation()}
          >
            <MenuItem onClick={handleMenuItemClick(onDetails)}>
              {t("opt.details")}
            </MenuItem>
            {(privilage == Privilage.OWNER || privilage == Privilage.WRITE) && (<>
              <MenuItem onClick={handleMenuItemClick(onEdit)}>
                {t("opt.edit")}
              </MenuItem>
              {privilage == Privilage.OWNER && (
                <MenuItem onClick={handleMenuItemClick(onDelete)}>
                  {t("opt.delete")}
                </MenuItem>)}
            </>)}
          </Menu>
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default FileCard;
