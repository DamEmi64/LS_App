import * as React from 'react';
import Box from '@mui/material/Box';
import Avatar from '@mui/material/Avatar';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import { useAuth } from '@/features/auth/context/authProvider';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';
import { useModal } from '@/shared/context/modal';
import ChangePasswordForm from '@/features/auth/components/changePasswordForm';
import LoginForm from '@/features/auth/components/loginForm';
import RegisterForm from '@/features/auth/components/registerForm';
import UserEdit from '@/features/auth/components/userEdit';
import UserProfile from '@/features/auth/components/userProfile';
import HouseIcon from '@mui/icons-material/House';
import SettingsWrapper from './settings/SettingsWrapper';
import { isNativeApp } from '@/shared/platform';

export default function navMenu() {
    const modal = useModal();
    const auth = useAuth();
    const { t } = useTranslation();
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const onDetails = () => {
        modal.showModal(<UserProfile auth={auth} onChangePassword={onChangePassword} onEdit={onEdit} />);
    }

    const onChangePassword = () => {
        modal.showModal(<ChangePasswordForm auth={auth} onClose={modal.hideModal} />);
    }

    const onEdit = () => {
        modal.showModal(<UserEdit auth={auth} onClose={modal.hideModal} />);
    }

    const onSettings = () => {
        modal.showModal(<SettingsWrapper/>);
    }

    const onLogin = () => {
        modal.showModal(<LoginForm auth={auth} onClose={modal.hideModal} />);
    }

    const onRegister = () => {
        modal.showModal(<RegisterForm auth={auth} onClose={modal.hideModal} />);
    }

    const onLogout = () => {
        auth.logout();
    }

    return (
        <React.Fragment>
            <Box sx={{ display: 'flex', alignItems: 'center', textAlign: 'center' }}>
                <Tooltip title="Account settings">
                    <IconButton
                        onClick={handleClick}
                        size="small"
                        sx={{ ml: 2 }}
                        aria-controls={open ? 'account-menu' : undefined}
                        aria-haspopup="true"
                        aria-expanded={open ? 'true' : undefined}
                    >
                        <Avatar sx={{ width: 32, height: 32 }}><HouseIcon /></Avatar>
                    </IconButton>
                </Tooltip>
            </Box>
            <Menu
                anchorEl={anchorEl}
                id="account-menu"
                open={open}
                onClose={handleClose}
                slotProps={{
                    paper: {
                        elevation: 0,
                        sx: {
                            overflow: 'visible',
                            filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
                            mt: 1.5,
                            '& .MuiAvatar-root': {
                                width: 32,
                                height: 32,
                                ml: -0.5,
                                mr: 1,
                            },
                            '&::before': {
                                content: '""',
                                display: 'block',
                                position: 'absolute',
                                top: 0,
                                right: 14,
                                width: 10,
                                height: 10,
                                bgcolor: 'background.paper',
                                transform: 'translateY(-50%) rotate(45deg)',
                                zIndex: 0,
                            },
                        },
                    },
                }}
                transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
            >
                {auth.user != null && (
                    <MenuItem onClick={onDetails}>
                        <Avatar /> {auth.user.login}
                    </MenuItem>)}
                {auth.user == null && !isNativeApp && (
                    <>
                        <MenuItem onClick={onLogin}>
                            {t('menu.login')}
                        </MenuItem>
                        <MenuItem onClick={onRegister}>
                            {t('menu.register')}
                        </MenuItem>
                    </>
                )}
                <Divider />
                <MenuItem onClick={onSettings}>
                    {t('menu.settings')}
                </MenuItem>

                {auth.checkPermission(['processes']) && (
                    <Link to='/processes'>
                        <MenuItem onClick={handleClose}>{t('menu.processes')}</MenuItem>
                    </Link>
                )}
                {auth.user != undefined && (
                    <MenuItem onClick={onLogout}>
                        {t('menu.logout')}
                    </MenuItem>)}
            </Menu>
        </React.Fragment>
    );
}
