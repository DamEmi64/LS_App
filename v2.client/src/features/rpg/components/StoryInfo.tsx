import React from 'react';
import { Box, TextField, Typography, useTheme, Avatar, Grid, Button } from '@mui/material';
import { t } from 'i18next';
import { Story } from '../types';
import FileManager from '@/shared/components/fileManager';

type SessionInfoProps = {
    story: Story;
    edit?: (data: Story) => void;
    del?: (data: Story) => void;
};

const formatDate = (date: string | Date) => {
    if (date == null) {
        return '';
    }

    return typeof date === 'string' ? new Date(date).toLocaleString() : date;
};

export const StoryInfo: React.FC<SessionInfoProps> = ({ story, edit, del }) => {
    const theme = useTheme();
    const textColor = theme.palette.text.primary;

    return (
        <Grid container spacing={2} alignItems="flex-start">
            {/* Left side: Info */}
            <Grid>
                <Typography variant="h6" sx={{ color: textColor, mb: 2 }}>
                    {t('window.info')}
                </Typography>
                <TextField
                    label={t('rpg.story.title')}
                    value={story.title}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
                <TextField
                    label={t('rpg.story.description')}
                    value={story.description}
                    multiline
                    minRows={4}
                    maxRows={16}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
                <TextField
                    label={t('rpg.story.startDate')}
                    value={formatDate(story.startDate)}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
                <TextField
                    label={t('rpg.story.endDate')}
                    value={formatDate(story.endDate)}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
                <FileManager files={story.files || []}
                    editMode={false}
                    remove={async o => {}}
                    add= {async o => {}} />

                <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                    <Button type="button" onClick={edit ? () => edit(story) : undefined} style={{ padding: '8px 16px', background: theme.palette.primary.main, color: '#fff', border: 'none', borderRadius: 4, cursor: 'pointer' }}>
                        {t('opt.edit')}
                    </Button>
                    <Button type="button" onClick={del ? () => del(story) : undefined} style={{ padding: '8px 16px', background: theme.palette.error.main, color: '#fff', border: 'none', borderRadius: 4, cursor: 'pointer' }}>
                        {t('opt.delete')}
                    </Button>
                </Box>
            </Grid>
        </Grid>
    );
};

export default StoryInfo;
