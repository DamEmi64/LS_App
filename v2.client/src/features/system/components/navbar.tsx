import React from 'react';
import NavItem from './navItem';
import { useTranslation } from 'react-i18next';
import { UserData } from '@/features/auth';
import { Box } from '@mui/material';
import { NavbarItemProps } from '../types';

const Navbar: React.FC<{ menu: NavbarItemProps[], user: UserData }> = ({ menu, user }) => {
    const { t } = useTranslation();
    return (
        <Box>
            <Box
                style={{
                    background:
                        "linear-gradient(180deg, #FFF 0%, rgba(153, 153, 153, 0.00) 49.52%)",
                    display: 'flex',
                    flexDirection: 'row',
                }}>
                {menu.map((item, index) => {
                    if (!item.permissions) {
                        return <NavItem key={index} {...item} />;
                    }

                    if (user) {
                        if (user.permissions) {
                            for (let i = 0; i < item.permissions.length; i++) {
                                if (user.permissions.includes(item.permissions[i]) || user.role == 'admin') {
                                    return <NavItem key={index} {...item} />;
                                }
                            }
                        }
                    }
                })}

            </Box>

        </Box>
    );
}

export default Navbar;