import { Box, Button, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { useAuth } from '@/features/auth/context/authProvider';
import LoginForm from '@/features/auth/components/loginForm';
import RegisterForm from '@/features/auth/components/registerForm';
import { useModal } from '@/shared/context/modal';

const AccountSettings: React.FC = () => {
    const auth = useAuth();
    const modal = useModal();
    const { t } = useTranslation();

    if (auth.user) {
        return (
            <Box display="flex" flexDirection="column" gap={2}>
                <Typography>{auth.user.login}</Typography>
                <Button variant="outlined" onClick={() => void auth.logout()}>
                    {t('menu.logout')}
                </Button>
            </Box>
        );
    }

    return (
        <Box display="flex" flexDirection="column" gap={2}>
            <Button
                variant="contained"
                onClick={() => modal.showSubModal(<LoginForm auth={auth} onClose={modal.hideSubModal} />)}
            >
                {t('menu.login')}
            </Button>
            <Button
                variant="outlined"
                onClick={() => modal.showSubModal(<RegisterForm auth={auth} onClose={modal.hideSubModal} />)}
            >
                {t('menu.register')}
            </Button>
        </Box>
    );
};

export default AccountSettings;
