import { IconButton, Menu, MenuItem } from "@mui/material";
import { t } from "i18next";
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { useState } from "react";
import { Operations } from "../table/definitions";

export const OperationMenu: React.FC<{ data: any, operations: Operations[] }> = ({ data, operations }) => {
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
            {operations.map((operation) => ( (operation.hidden && operation.hidden(data)) ? null :
                <MenuItem onClick={() => handleMethod(operation.method)}>{t(operation.name)}</MenuItem>
            ))}
        </Menu>
    </>

    )
}