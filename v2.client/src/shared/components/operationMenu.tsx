import { IconButton, Menu, MenuItem } from "@mui/material";
import { t } from "i18next";
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { useState } from "react";
import { Operations } from "@/shared";
import ArrowCircleRightIcon from '@mui/icons-material/ArrowCircleRight';

export const OperationMenu = <T,>({
    data,
    operations
}: {
    data: T;
    operations: Operations<T>[];
}) => {

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

    const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const handleMethod = (method: (data: any) => void) => {
        handleMenuClose();
        method?.(data);
    };

    if (operations.length === 0) {
        return null;
    }

    const availableOperations = operations.filter(op => !(op.hidden && op.hidden(data)));
    
    if (availableOperations.length === 0) {
        return null;
    }

    if(availableOperations.length === 1) {
        return (
            <IconButton
                aria-label="more"
                aria-controls="operation-menu"
                aria-haspopup="true"
                onClick={() => handleMethod(availableOperations[0].method)}
                size="small"
            >
                <ArrowCircleRightIcon />
            </IconButton>
        )
    }
    else {
        return (<>
                <IconButton
                    aria-label="more"
                    aria-controls="operation-menu"
                    aria-haspopup="true"
                    onClick={handleMenuOpen}
                    size="small"
                >
                    <MoreVertIcon />
                </IconButton>
                <Menu
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl)}
                    onClose={handleMenuClose}
                    anchorOrigin={{
                        vertical: 'bottom',
                        horizontal: 'right',
                    }}
                    transformOrigin={{
                        vertical: 'top',
                        horizontal: 'right',
                    }}
                >
                    {availableOperations.map((operation) => ( 
                        <MenuItem key={availableOperations.indexOf(operation)} onClick={() => handleMethod(operation.method)}>{t(operation.name)}</MenuItem>
                    ))}
                </Menu>
            </>)
    }
}