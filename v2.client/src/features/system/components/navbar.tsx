import React from 'react';
import NavItem from './navItem';
import { useTranslation } from 'react-i18next';
import { UserData } from '@/features/auth';
import { Box } from '@mui/material';
import { NavbarItemProps } from '@/shared';

const Navbar: React.FC<{ menu: NavbarItemProps[]; user: UserData }> = ({ menu, user }) => {
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
                    flexDirection: 'row',
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
                                flex: '0 0 auto',
                                color: item.href === window.location.pathname ? 'primary.main' : 'text.primary',
                                minWidth: { xs: 90, sm: 'auto' },
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