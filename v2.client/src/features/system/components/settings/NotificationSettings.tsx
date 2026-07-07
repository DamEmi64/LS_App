import React from 'react';
import { TextField, FormControlLabel, Switch, Select, MenuItem, InputLabel, FormControl, Box, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { useConfiguration } from '@/shared/context/configuration';

const NotificationSettings: React.FC = () => {
    const { useVariable } = useConfiguration();
    const { t } = useTranslation();
    const [labelColor] = useVariable('labelColor');
    const [position, setPosition] = useVariable('toastPosition');
    const [autoClose, setAutoCLose] = useVariable('toastAutoClose');
    const [active, setActive] = useVariable('toastActive');

    return (
        <>
            <Box>
                <Typography variant="h6" gutterBottom sx={{ color: labelColor }}>
                    {t('notify.notification')}
                </Typography>
                <FormControlLabel
                    control={
                        <Switch
                            checked={active == 'true'}
                            onChange={e => setActive(e.target.checked.toString())}
                            color="primary"
                        />
                    }
                    label={t('notify.active')}
                    sx={{ color: labelColor }}
                />
                <br />
                <br />
                {(active == 'true' && (
                    <FormControl fullWidth>
                        <TextField
                            type='number'
                            value={autoClose}
                            onChange={e => setAutoCLose(e.target.value)}
                            color="primary"
                            label={t('notify.autoClose')}
                        />
                    </FormControl>
                )
                )}
                <br />
                <br />
                <FormControl fullWidth>
                    <InputLabel id="notification-position-select-label" sx={{ color: labelColor }}>
                        {t('notify.position')}
                    </InputLabel>
                    <Select
                        labelId="notification-position-select-label"
                        value={position}
                        label={t('notify.position')}
                        onChange={e => setPosition(e.target.value as string)}
                        sx={{ color: labelColor }}
                    >
                        <MenuItem key={'bottom-left'} value={'bottom-left'}>{t('notify.bottomLeft')}</MenuItem>
                        <MenuItem key={'top-right'} value={'top-right'}>{t('notify.topRight')}</MenuItem>
                        <MenuItem key={'bottom-right'} value={'bottom-right'}>{t('notify.bottomRight')}</MenuItem>
                        <MenuItem key={'top-left'} value={'top-left'}>{t('notify.topLeft')}</MenuItem>
                    </Select>
                </FormControl>

            </Box>
        </>
    );
};

export default NotificationSettings;
