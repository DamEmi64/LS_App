import React from 'react';
import NavItem from './navItem';
import { useAuth } from '@/features/auth/context/authProvider';
import { Box } from '@mui/material';
import { NavbarItemProps } from '@/shared';

const Navbar: React.FC<{ menu: NavbarItemProps[]; user: any }> = ({ menu, user }) => {
    const { checkPermission } = useAuth();

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: { xs: 'column', md: 'row' }, // 🔥 KEY FIX
                gap: 1,
                px: 1,
                py: 0.5,
                width: '100%',
                alignItems: { xs: 'stretch', md: 'center' },
            }}
        >
            {menu.map((item, index) => {
                const hasAccess =
                    !item.permissions ||
                    user?.role === 'admin' ||
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
    );
};

export default Navbar;