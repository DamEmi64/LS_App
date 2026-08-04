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
        width: "100%",
      }}
    >
      <CardActionArea
        onClick={onClick}
        sx={{
          flex: 1,
          minWidth: 0,
        }}
      >
        <CardContent
          sx={{
            display: "flex",
            alignItems: "center",
            py: { xs: 1.25, sm: 1.5 },
            px: { xs: 1.5, sm: 2 },
          }}
        >
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              gap: { xs: 1.5, sm: 2 },
              width: "100%",
              minWidth: 0,
            }}
          >
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
                flexShrink: 0,
              }}
            >
              {icon}
            </Box>

            <Typography
              variant="subtitle1"
              sx={{
                flex: 1,
                minWidth: 0,
                fontSize: {
                  xs: "0.95rem",
                  sm: "1rem",
                },
                overflow: "hidden",
                display: "-webkit-box",
                WebkitBoxOrient: "vertical",
                WebkitLineClamp: 2,
                wordBreak: "break-word",
              }}
            >
              {name}
            </Typography>
          </Box>
        </CardContent>
      </CardActionArea>

      <CardActions
        sx={{
          p: 0.5,
          flexShrink: 0,
        }}
      >
        <IconButton
          onClick={handleMenuOpen}
          onMouseDown={(e) => e.stopPropagation()}
          sx={{
            width: 44,
            height: 44,
          }}
        >
          <MoreVertIcon />
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

        {(privilage === Privilage.OWNER || privilage === Privilage.WRITE) && (
          <>
            <MenuItem onClick={handleMenuItemClick(onEdit)}>
              {t("opt.edit")}
            </MenuItem>

            {privilage === Privilage.OWNER && (
              <MenuItem onClick={handleMenuItemClick(onDelete)}>
                {t("opt.delete")}
              </MenuItem>
            )}
          </>
        )}
      </Menu>
    </Card>
  );
};

export default FileCard;
