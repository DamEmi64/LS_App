import React from 'react';
import {
    Typography,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    Box,
    useTheme,
    TextField,
    Grid,
} from '@mui/material';
import { convertToDateStr, useDictionaryTranslation } from "@/lib/utils";

// Import types from models/system.ts
import { Process, Job, ProcessError } from '@/features/system'
import { t } from 'i18next';

type ProcessInfoProps = {
    process: Process;
};

const ProcessInfo: React.FC<ProcessInfoProps> = ({ process }) => {
    const theme = useTheme();
    const translate = useDictionaryTranslation();
    const textColor = theme.palette.mode === 'dark' ? theme.palette.grey[100] : theme.palette.text.primary;

    const convertJobStatus = (id: string) => {
        return t('processes.processStatus.' + id);
    };

    const convertOperation = (id: number) => {
        return translate('Operations',id).title;
    };

    return (
        <Grid>
            <Typography variant="h6" gutterBottom sx={{ color: textColor }}>
                {t('window.info')}
            </Typography>
            <Box mb={2} display="flex" flexDirection="column" gap={2}>
                <TextField
                    label={t('processes.name')}
                    value={process.title}
                    InputProps={{ readOnly: true }}
                    variant="outlined"
                    size="small"
                    sx={{ input: { color: textColor } }}
                />
                <TextField
                    label={t('processes.status')}
                    value={t(convertJobStatus(process.status))}
                    InputProps={{ readOnly: true }}
                    variant="outlined"
                    size="small"
                    sx={{ input: { color: textColor } }}
                />
            </Box>
            <Typography variant="subtitle1" gutterBottom sx={{ color: textColor }}>
                {t('processes.jobs')}
            </Typography>
            <TableContainer component={Paper}>
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ color: textColor }}>{t('jobs.jobId')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.name')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.operation')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.status')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.requestData')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.startDate')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.endDate')}</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {process.jobs.sort((a, b) => (a.jobId as unknown as number) - (b.jobId as unknown as number)).map((job: Job) => (
                            <TableRow key={job.id}>
                                <TableCell sx={{ color: textColor }}>{job.jobId}</TableCell>
                                <TableCell sx={{ color: textColor }}>{job.name}</TableCell>
                                <TableCell sx={{ color: textColor }}>{convertOperation(job.operation)}</TableCell>
                                <TableCell sx={{ color: textColor }}>{convertJobStatus(job.status)}</TableCell>
                                <TableCell sx={{ color: textColor }}>{convertToDateStr(job.requestDate.toString())}</TableCell>
                                <TableCell sx={{ color: textColor }}>{job.startDate == null ? '-' : convertToDateStr(job.startDate.toString())}</TableCell>
                                <TableCell sx={{ color: textColor }}>{job.endDate == null ? '-' : convertToDateStr(job.endDate.toString())}</TableCell>
                            </TableRow>
                        ))}
                        {process.jobs.length === 0 && (
                            <TableRow>
                                <TableCell colSpan={5} align="center" sx={{ color: textColor }}>
                                    {t('processes.noJobs')}
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
            <Typography variant="subtitle1" gutterBottom sx={{ color: textColor }}>
                {t('processes.errors')}
            </Typography>
            <TableContainer component={Paper}>
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ color: textColor }}>{t('jobs.jobId')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('processes.errorMessage')}</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {process.errors && process.errors.map((error: ProcessError) => (
                            <TableRow key={error.id}>
                                <TableCell sx={{ color: textColor }}>{error.jobId}</TableCell>
                                <TableCell sx={{ color: textColor }}>{error.message}</TableCell>
                            </TableRow>
                        ))}
                        {(!process.errors || process.errors.length === 0) && (
                            <TableRow>
                                <TableCell colSpan={5} align="center" sx={{ color: textColor }}>
                                    {t('no_data')}
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
        </Grid>
    );
};

export default ProcessInfo;