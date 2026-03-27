import { useTranslation } from "react-i18next";

import { useAuth } from "@/features/auth/context/authProvider";
import AuthPage from "@/features/auth/pages/AuthPage";
import PermissionPage from "@/features/auth/pages/PermissionPage";
import React from "react";
import { NavbarItemProps } from "@/features/system/components/definitions";
import Navbar from "@/features/system/components/navbar";
import Authbar from "@/features/system/components/navMenu";

const Layout: React.FC<{ image: string, content: React.FC, title: string, permissions?: string[], menu: NavbarItemProps[] }> = ({ image, content, title, permissions, menu }) => {
    const { t } = useTranslation();
    const auth = useAuth();
    document.title = t(title);

    var toShow;

    if (auth.user) {
        if (permissions && permissions.length > 0) {
            if (auth.checkPermission(permissions)) {
                toShow = React.createElement(content);
            } else {
                toShow = <PermissionPage />;
            }
        } else {
            toShow = React.createElement(content);
        }
    } else {
        toShow = <AuthPage />;
    }

    return (
        <div
            className="min-h-screen bg-background flex flex-col items-center justify-center"
            style={{
                backgroundImage: `url(${image})`,
                backgroundRepeat: "no-repeat",
                backgroundPosition: "center",
                backgroundSize: "cover",
            }}
        >

            <div className=" absoulute w-full flex items-center justify-center" >
                {/* Navbar centered */}
                <div className="relative ml-auto">
                    <Navbar user={auth.user} menu={menu} />
                </div>
                {/* Authbar on the right */}
                <div className="ml-auto">
                    <Authbar />
                </div>
            </div>

            <div className="flex justify-center items-center w-full flex-1">
                {toShow}
            </div>

            {/* Google Fonts Link */}
            <link
                href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600&display=swap"
                rel="stylesheet"
            />
        </div>
    );
};

export default Layout;