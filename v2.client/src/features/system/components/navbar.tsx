import React from 'react';
import NavItem from './navItem';
import { useAuth } from '@/features/auth/context/authProvider';
import { Box } from '@mui/material';
import { NavbarItemProps } from '@/shared';
import { UserData } from '@/features/auth';
import { useTranslation } from 'react-i18next';

const Navbar: React.FC<{ menu: NavbarItemProps[]; user: UserData, isDrawer?: boolean }> = ({ menu, user, isDrawer }) => {
    const { t } = useTranslation();

    return (
        <Box
            sx={{
                position: 'sticky',
                top: 0,
                zIndex: 1000,
                width: '100%',
                backdropFilter: 'blur(8px)',
                borderBottom: '1px solid rgba(0,0,0,0.08)',
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: isDrawer ? 'column' : 'row',
                    gap: 1,
                    px: 1,
                    py: 0.5,

                    // 👇 MOBILE SCROLL FIX
                    overflowX: 'auto',
                    overflowY: 'hidden',
                    WebkitOverflowScrolling: 'touch',
                    scrollbarWidth: 'none',

                    '&::-webkit-scrollbar': {
                        display: 'none',
                    },
                }}
            >
                {menu.map((item, index) => {
                    const hasAccess =
                        !item.permissions ||
                        (user?.role === 'admin') ||
                        (user?.permissions &&
                            item.permissions.some(p => user.permissions.includes(p)));

                    if (!hasAccess) return null;

                    return (
                        <Box
                            key={index}
                            sx={{
                                flex: { md: '0 0 auto' },
                                width: { xs: '100%', md: 'auto' },
                            }}
                        >
                            <NavItem {...item} />
                        </Box>
                    );
                })}
            </Box>
        </Box>
    );
};

export default Navbar;