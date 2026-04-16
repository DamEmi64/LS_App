import { Box, Menu, MenuItem, useTheme } from "@mui/material";
import { useState } from "react";
import { Link } from "react-router-dom";
import { NavbarItemProps } from "@/shared";
import { useTranslation } from "react-i18next";

const navItem = (props: NavbarItemProps) => {
    const theme = useTheme();
    const textColor = theme.palette.mode === 'dark' ? theme.palette.text.primary : theme.palette.text.secondary;
    const { t } = useTranslation();
    const [anchorEl, setAnchorEl] = useState(null);
    const open = Boolean(anchorEl);
    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    if (props.submenu.length == 1) {
        return (
            <Link to={props.submenu[0].href} unstable_viewTransition={true} style={{ textDecoration: "none" }}>
                <button>
                    <Box
                        className="flex p-1 px-3 justify-center items-center rounded-t relative max-[640px]:p-0.5 max-[640px]:px-2"
                    >
                        <Box
                            className="relative max-[640px]:text-sm"
                            style={{
                                color: props.href === window.location.pathname ? "rgb(255, 255, 255)" : "rgba(48, 48, 48, 1)",
                                font: "400 16px/140% Inter, -apple-system, Roboto, Helvetica, sans-serif",
                            }}
                        >
                            {t(`menu.${props.label}`)}
                        </Box>
                    </Box>
                </button>
            </Link>
        );
    }

    if (props.submenu.length > 0) {
        return (
            <div onMouseEnter={handleClick} onMouseLeave={handleClose} >
                <button >
                    <div
                        className="flex p-1 px-3 justify-center items-center rounded-t relative max-[640px]:p-0.5 max-[640px]:px-2"
                    >
                        <Box
                            className="relative max-[640px]:text-sm"
                            style={{
                                color: props.href === window.location.pathname ? "rgb(255, 255, 255)" : "rgba(48, 48, 48, 1)",
                                font: "400 16px/140% Inter, -apple-system, Roboto, Helvetica, sans-serif",
                            }}
                        >
                            {t(`menu.${props.label}`)}
                        </Box>
                    </div>
                </button>

                <Menu
                    id="basic-menu"
                    anchorEl={anchorEl}
                    open={open}

                    onClose={handleClose}
                    slotProps={{
                        list: {
                            'aria-labelledby': 'basic-button',
                        },
                    }}
                >   {props.submenu.map((item, index) => (
                    <Link to={item.href} key={index} unstable_viewTransition={true} style={{ textDecoration: "none" }}>
                        <MenuItem onClick={handleClose} style={{ color: textColor, opacity: '50%' }}>
                            {t(`menu.${item.label}`)}
                        </MenuItem>
                    </Link>
                ))}
                </Menu>
            </div>
        );
    }

    return (
        <Link to={props.href} unstable_viewTransition={true} style={{ textDecoration: "none" }}>
            <button>
                <Box
                    className="flex p-1 px-3 justify-center items-center rounded-t relative max-[640px]:p-0.5 max-[640px]:px-2"
                >
                    <Box
                        className="relative max-[640px]:text-sm"
                        style={{
                            color: props.href === window.location.pathname ? "rgb(255, 255, 255)" : "rgba(48, 48, 48, 1)",
                            font: "400 16px/140% Inter, -apple-system, Roboto, Helvetica, sans-serif",
                        }}
                    >
                        {t(`menu.${props.label}`)}
                    </Box>
                </Box>
            </button>
        </Link>
    );
}

export default navItem;