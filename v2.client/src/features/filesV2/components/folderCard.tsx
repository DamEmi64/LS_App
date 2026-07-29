import React from 'react';
import { Card, CardActionArea, CardContent, Typography, Box, CardActions, Button } from '@mui/material';
import { FileItem } from '../types'
import { GridDeleteIcon } from '@mui/x-data-grid';


const FolderCard: React.FC<FileItem> = ({ name, icon, onClick, onDelete }) => {

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
        <Button onClick={onDelete}>
          <GridDeleteIcon />
        </Button>
      </CardActions>

    </Card>
  );
}

export default FolderCard;
