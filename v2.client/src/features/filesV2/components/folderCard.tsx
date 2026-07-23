import React, { useState } from 'react';
import { Card, CardActionArea, CardContent, CardActions, Typography, IconButton, Menu, MenuItem, Box } from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import {t} from 'i18next';
import { FileItem } from '../types'
import { GridDeleteIcon } from '@mui/x-data-grid';


const FolderCard: React.FC<FileItem> = ({ name, icon, onClick, onDelete}) => {

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
              direction:"row"
            }}
          >
            {icon}

            <Typography noWrap variant="subtitle1">
              {name}
            </Typography>
          </Box>
          <GridDeleteIcon onClick={onDelete}></GridDeleteIcon>
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default FolderCard;
