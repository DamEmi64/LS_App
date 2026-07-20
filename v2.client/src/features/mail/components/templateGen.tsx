import { UserData } from '@/features/auth';
import { Box, Button, Grid, IconButton, TextField, Typography, useTheme } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

export interface TemplateGenData {
    template: string;
    sender: UserData;
    recipients: UserData[];
}

interface TemplateGenProps {
    initialData?: TemplateGenData;
    onSubmit: (data: TemplateGenData) => void;
}

export const TemplateGen: React.FC<TemplateGenProps> = ({ initialData, onSubmit }) => {
    const { t } = useTranslation();
    const theme = useTheme();
    const textColor = theme.palette.text.primary;
    const [sender, setSender] = useState<UserData>(initialData?.sender || {} as UserData);
    const [recipients, setRecipients] = useState<UserData[]>(initialData?.recipients || []);

    const handleRecipientChange = (index: number, field: keyof UserData, value: string) => {
        const updated = [...recipients];
        updated[index] = { ...updated[index], [field]: value };
        setRecipients(updated);
    };

    const addRecipient = () => {
        setRecipients([...recipients, { id: 0, userId: '', login: '', email: '' } as UserData]);
    };

    const removeRecipient = (index: number) => {
        setRecipients(recipients.filter((_, i) => i !== index));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        const template = initialData?.template || '';
        onSubmit({ template, sender, recipients });
    };

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ maxWidth: 600, mx: 'auto', mt: 4 }}>
            <Typography variant="h5" gutterBottom>{t('communication.email.sender.title')}</Typography>
            <Grid container spacing={2}>
                <Grid>
                    <TextField
                        label={t('communication.email.sender.title')}
                        fullWidth
                        value={sender.login}
                        InputProps={{ style: { color: textColor } }}
                        onChange={(e) => setSender({ ...sender, login: e.target.value })}
                    />
                </Grid>
                <Grid>
                    <TextField
                        label="Email"
                        type="email"
                        fullWidth
                        value={sender.email}
                        InputProps={{ style: { color: textColor } }}
                        onChange={(e) => setSender({ ...sender, email: e.target.value })}
                    />
                </Grid>
            </Grid>

            <Typography variant="h5" gutterBottom sx={{ mt: 4 }}>
                {t('communication.email.recipient.title')}
            </Typography>

            {recipients.map((recipient, index) => (
                <Grid container spacing={2} key={index} alignItems="center" sx={{ mb: 1 }}>
                    <Grid size={{ xs: 12, md: 4 }}>
                        <TextField
                            label={t('communication.email.recipient.title')}
                            fullWidth
                            value={recipient.login}
                            InputProps={{ style: { color: textColor } }}
                            onChange={(e) =>
                                handleRecipientChange(index, 'login', e.target.value)
                            }
                        />
                    </Grid>
                    <Grid size={{ xs: 12, md: 4 }}>
                        <TextField
                            label="Email"
                            type="email"
                            fullWidth
                            value={recipient.email}
                            InputProps={{ style: { color: textColor } }}
                            onChange={(e) =>
                                handleRecipientChange(index, 'email', e.target.value)
                            }
                        />
                    </Grid>
                    <Grid size={{ xs: 12, md: 4 }}>
                        <IconButton onClick={() => removeRecipient(index)} aria-label="delete">
                            <DeleteIcon />
                        </IconButton>
                    </Grid>
                </Grid>
            ))}

            <Box sx={{ my: 2 }}>
                <Button variant="outlined" onClick={addRecipient}>
                    <AddIcon />
                </Button>
            </Box>

            <Button type="submit" variant="contained" color="primary">
                {t('opt.save')}
            </Button>
        </Box>
    );
};
