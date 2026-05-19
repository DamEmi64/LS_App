import { Box, Menu, MenuItem, useTheme } from "@mui/material";
import { useState } from "react";
import { Link } from "react-router-dom";
import { NavbarItemProps } from "@/shared";
import { useAuth } from "@/features/auth/context/authProvider";
import { useTranslation } from "react-i18next";

const NavItem = (props: NavbarItemProps) => {
    const theme = useTheme();
    const { t } = useTranslation();
    const { checkPermission } = useAuth();

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);

    const handleClick = (event: any) => setAnchorEl(event.currentTarget);
    const handleClose = () => setAnchorEl(null);

    const textColor =
        theme.palette.mode === 'dark'
            ? theme.palette.text.primary
            : theme.palette.text.secondary;

    const availableMenu = props.submenu.filter(
        x => !x.permissions || checkPermission(x.permissions)
    );

    const isMobile = typeof window !== "undefined" && window.innerWidth < 768;

    // SINGLE ITEM
    if (availableMenu.length === 1) {
        return (
            <Link to={availableMenu[0].href} style={{ textDecoration: "none" }}>
                <Box
                    sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        p: 1,
                        px: 2,
                        width: { xs: '100%', md: 'auto' },
                        textAlign: 'center',
                    }}
                >
                    {t(`menu.${props.label}`)}
                </Box>
            </Link>
        );
    }

    // MULTI MENU
    if (availableMenu.length > 0) {
        return (
            <div
                onMouseEnter={!isMobile ? handleClick : undefined}
                onMouseLeave={!isMobile ? handleClose : undefined}
            >
                <Box
                    onClick={isMobile ? handleClick : undefined}
                    sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        p: 1,
                        px: 2,
                        width: { xs: '100%', md: 'auto' },
                    }}
                >
                    {t(`menu.${props.label}`)}
                </Box>

                <Menu anchorEl={anchorEl} open={open} onClose={handleClose}>
                    {availableMenu.map((item, index) => (
                        <Link
                            to={item.href}
                            key={index}
                            style={{ textDecoration: "none" }}
                        >
                            <MenuItem onClick={handleClose} style={{ color: textColor }}>
                                {t(`menu.${item.label}`)}
                            </MenuItem>
                        </Link>
                    ))}
                </Menu>
            </div>
        );
    }

    // SINGLE LINK
    return (
        <Link to={props.href} style={{ textDecoration: "none" }}>
            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    p: 1,
                    px: 2,
                    width: { xs: '100%', md: 'auto' },
                }}
            >
                {t(`menu.${props.label}`)}
            </Box>
        </Link>
    );
};

export default NavItem;