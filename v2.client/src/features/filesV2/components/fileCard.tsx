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
    <Card
      variant="outlined"
      sx={{
        display: "flex",
        alignItems: "center",
      }}
    >
      <CardActionArea
        onClick={onClick}
        sx={{
          flex: 1,
        }}
      >
        <CardContent
          sx={{
            display: "flex",
            alignItems: "center",
            py: 1.5,
          }}
        >
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              gap: 2,
              minWidth: 0,
            }}
          >
            {icon}

            <Typography noWrap variant="subtitle1">
              {name}
            </Typography>
          </Box>
        </CardContent>
      </CardActionArea>

      <CardActions sx={{ p: 1 }}>
        <IconButton
          size="small"
          onClick={handleMenuOpen}
          onMouseDown={(e) => e.stopPropagation()}
        >
          <MoreVertIcon fontSize="small" />
        </IconButton>
      </CardActions>

      <Menu
        anchorEl={anchorEl}
        open={open}
        onClose={handleMenuClose}
        onClick={(e) => e.stopPropagation()}
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
    </Card>
  );
};

export default FileCard;
