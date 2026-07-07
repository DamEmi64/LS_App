import React from 'react';
import { Box, TextField, Typography, useTheme, Avatar, Grid, Button } from '@mui/material';
import { File } from '@/features/files';
import { t } from 'i18next';
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';
import { ImageProvider } from '@/shared/components/imageProvider';
import { useDictionaryTranslation } from '@/lib/utils';

type FilesInfoProps = {
    file: File;
    edit?: (data: File) => void;
    del?: (data: File) => void;
};

const FILE_TYPE_GAME = 100;
const FILE_TYPE_STUDY = 102;

const formatDate = (date: string | Date) => {
    const d = typeof date === 'string' ? new Date(date) : date;
    return d?.toLocaleString();
};

export const FilesInfo: React.FC<FilesInfoProps> = ({ file, edit, del }) => {

    const getDictionaryTranslation = useDictionaryTranslation();
    const theme = useTheme();
    const textColor = theme.palette.text.primary;

    const showGameGenre = file.fileType === FILE_TYPE_GAME;
    const showStudyFields = file.fileType === FILE_TYPE_STUDY;

    return (
        <Grid container spacing={2} alignItems="flex-start">
            {/* Left side: Info */}
            <Grid size={{ xs: 12, md: 8 }}>
                <Typography variant="h6" sx={{ color: textColor, mb: 2 }}>
                    {t('window.info')}
                </Typography>
                <TextField
                    label={t('files.name')}
                    value={file.title}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
                <TextField
                    label={t('files.locaction')}
                    value={file.locaction}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
                <TextField
                    label={t('files.fileType')}
                    value={file.fileType}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
                <TextField
                    label={t('entity.insDate')}
                    value={formatDate(file.insDate)}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
                <TextField
                    label={t('entity.upDate')}
                    value={formatDate(file.upDate)}
                    fullWidth
                    InputProps={{ readOnly: true, style: { color: textColor } }}
                    margin="dense"
                    variant="outlined"
                />
            </Grid>
            {/* Right side: Image */}
            <Grid size={{ xs: 12, md: 4 }}>
                {file.image && (
                        <ImageProvider
                            imageId={file.image}
                            readonly={true}
                            saveImage={(data) => {}}
                        />
                )}
            </Grid>

            {file.additionalData && (
                <Grid size={{ xs: 12 }}>
                    <Typography variant="subtitle1" sx={{ color: textColor }}>
                        {t('files.additionalData')}
                    </Typography>
                    <Grid container spacing={1}>
                        {showGameGenre && (
                            <Grid size={{ xs: 12, md: 4 }}>
                                <TextField
                                    label={t('files.gameGenre')}
                                    value={getDictionaryTranslation('gameGenres', file.additionalData.gameGenre).title}
                                    fullWidth
                                    InputProps={{ readOnly: true, style: { color: textColor } }}
                                    margin="dense"
                                    variant="outlined"
                                />
                            </Grid>
                        )}
                        {showStudyFields && (
                            <>
                                <Grid>
                                    <TextField
                                        label="Subject"
                                        value={file.additionalData.subject ?? ''}
                                        fullWidth
                                        InputProps={{ readOnly: true, style: { color: textColor } }}
                                        margin="dense"
                                        variant="outlined"
                                    />
                                </Grid>
                                <Grid>
                                    <TextField
                                        label="Year"
                                        value={file.additionalData.year ?? ''}
                                        fullWidth
                                        InputProps={{ readOnly: true, style: { color: textColor } }}
                                        margin="dense"
                                        variant="outlined"
                                    />
                                </Grid>
                                <Grid>
                                    <TextField
                                        label="Semester"
                                        value={file.additionalData.semester ?? ''}
                                        fullWidth
                                        InputProps={{ readOnly: true, style: { color: textColor } }}
                                        margin="dense"
                                        variant="outlined"
                                    />
                                </Grid>
                            </>
                        )}
                    </Grid>
                </Grid>
            )}

            {file.sources && (
                <Grid size={{ xs: 12 }}>
                    <Typography variant="subtitle1" sx={{ color: textColor, mt: 2 }}>
                        {t('files.sources')}
                    </Typography>
                    {file.sources.length === 0 ? (
                        <Typography variant="body2" sx={{ color: textColor }}>
                            {t('no_data')}
                        </Typography>
                    ) : (
                        file.sources.map((src) => (
                            <Box key={src.id} sx={{ mb: 1 }}>
                                <TextField
                                    label="Source Type"
                                    value={getDictionaryTranslation('File source type', src.sourceType).title}
                                    sx={{ mr: 2, width: 200 }}
                                    InputProps={{ readOnly: true, style: { color: textColor } }}
                                    margin="dense"
                                    variant="outlined"
                                />
                                <TextField
                                    label="Link"
                                    value={src.link}
                                    sx={{ width: 600 }}
                                    InputProps={{ readOnly: true, style: { color: textColor } }}
                                    margin="dense"
                                    variant="outlined"
                                />
                                {src.imported && (<CheckCircleOutlineIcon sx={{ width: 20, color: 'green' }} />)}
                            </Box>
                        ))
                    )}
                </Grid>
            )}

            <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                <Button type="button" onClick={edit ? () => edit(file) : undefined} style={{ padding: '8px 16px', background: theme.palette.primary.main, color: '#fff', border: 'none', borderRadius: 4, cursor: 'pointer' }}>
                    {t('opt.edit')}
                </Button>
                <Button type="button" onClick={del ? () => del(file) : undefined} style={{ padding: '8px 16px', background: theme.palette.error.main, color: '#fff', border: 'none', borderRadius: 4, cursor: 'pointer' }}>
                    {t('opt.delete')}
                </Button>
            </Box>
        </Grid>
    );
};

export default FilesInfo;
