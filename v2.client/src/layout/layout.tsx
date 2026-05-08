import { useTranslation } from "react-i18next";
import React, { useState } from "react";

import { useAuth } from "@/features/auth/context/authProvider";
import AuthPage from "@/features/auth/pages/AuthPage";
import PermissionPage from "@/features/auth/pages/PermissionPage";

import Navbar from "@/features/system/components/navbar";
import Authbar from "@/features/system/components/navMenu";
import { GridCloseIcon, GridMenuIcon } from "@mui/x-data-grid";
import { Button, Grid, IconButton, useMediaQuery, useTheme } from "@mui/material";

const Layout = ({
    image,
    content,
    title,
    permissions,
    menu,
    allowAnonymous,
}: any) => {
    const { t } = useTranslation();
    const auth = useAuth();

    const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
    document.title = t(title);

    let toShow;

    if (auth.user || allowAnonymous) {
        if (permissions?.length > 0) {
            toShow = auth.checkPermission(permissions)
                ? React.createElement(content)
                : <PermissionPage />;
        } else {
            toShow = React.createElement(content);
        }
    } else {
        toShow = <AuthPage />;
    }

    return (
        <Grid
            className="min-h-screen w-full overflow-hidden flex flex-col"
            style={{
                backgroundImage: `url(${image})`,
                backgroundRepeat: "no-repeat",
                backgroundPosition: "center",
                backgroundSize: "cover",
            }}
        >
            {/* HEADER */}
            <header className="relative w-full h-14 flex items-center">

                {/* Desktop centered navbar */}
                <div className="hidden md:block absolute left-1/2 -translate-x-1/2">
                    <Navbar user={auth.user} menu={menu} />
                </div>

                {/* Desktop authbar */}
                <div className="hidden md:flex absolute right-4">
                    <Authbar />
                </div>

                {/* Mobile menu button */}
                <Button
                    onClick={() => setMobileMenuOpen(true)}
                    sx={{
                        ml: "auto",
                        display: isMobile && !mobileMenuOpen ? "flex" : "none",
                        minWidth: 40,
                    }}
                >
                    <GridMenuIcon />
                </Button>
            </header>

            {/* MOBILE DRAWER */}
            <Grid
                className={`fixed top-0 right-0 h-full w-72 bg-black/90 z-50 transition-transform duration-300
                ${mobileMenuOpen ? "translate-x-0" : "translate-x-full"}`}
            >
                <Grid className="p-4 flex flex-col gap-4 text-white">
                    <Button
                        onClick={() => setMobileMenuOpen(false)}
                        className="self-end"
                    >
                        <GridCloseIcon />
                    </Button>

                    <Navbar user={auth.user} menu={menu} isDrawer />
                    <Authbar />
                </Grid>
            </Grid>

            {/* CONTENT */}
            <div className="flex-1 w-full overflow-hidden" style={{ width: '100vw' }}>
                <div className="w-full h-full overflow-auto">
                    {toShow}
                </div>
            </div>
        </Grid>
    );
};

export default Layout;