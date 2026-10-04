import React, { useMemo, useState } from 'react';
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
    ToggleButton,
    ToggleButtonGroup,
} from '@mui/material';
import ReactFlow, { Background, Controls, Edge, Handle, Node, NodeProps, Position } from 'reactflow';
import 'reactflow/dist/style.css';
import { convertToDateStr, useDictionaryTranslation } from "@/lib/utils";

// Import types from models/system.ts
import { Process, Job, ProcessError } from '@/features/system'
import { t } from 'i18next';

type ProcessInfoProps = {
    process: Process;
};

type ProcessBoundaryNodeData = { label: string; kind: 'start' | 'end' };

const ProcessBoundaryNode = ({ data }: NodeProps<ProcessBoundaryNodeData>) => (
    <Box sx={{ position: 'relative', px: 2, py: 1.25, minWidth: 180, borderRadius: 1, bgcolor: 'secondary.main', color: 'secondary.contrastText', textAlign: 'center', fontWeight: 600 }}>
        {data.kind === 'end' && <Handle type="target" position={Position.Left} />}
        {data.label}
        {data.kind === 'start' && <Handle type="source" position={Position.Right} />}
    </Box>
);

const nodeTypes = { processBoundary: ProcessBoundaryNode };

const ProcessInfo: React.FC<ProcessInfoProps> = ({ process }) => {
    const theme = useTheme();
    const translate = useDictionaryTranslation();
    const textColor = theme.palette.primary.main;
    const convertJobStatus = (id: string) => t('processes.processStatus.' + id);
    const convertOperation = (id: number) => translate('Operations', id).title;
    const getJobStatusColors = (status: string) => {
        switch (status?.toLowerCase()) {
            case 'success': return { background: theme.palette.success.main, color: theme.palette.success.contrastText };
            case 'failed': return { background: theme.palette.error.main, color: theme.palette.error.contrastText };
            case 'executing': return { background: theme.palette.info.main, color: theme.palette.info.contrastText };
            case 'paused': return { background: theme.palette.warning.main, color: theme.palette.warning.contrastText };
            default: return { background: theme.palette.action.selected, color: theme.palette.text.primary };
        }
    };
    const [jobView, setJobView] = useState<'list' | 'tree'>(() => {
        try {
            return localStorage.getItem('processInfo.jobView') === 'tree' ? 'tree' : 'list';
        } catch {
            return 'list';
        }
    });

    const jobs = useMemo(() => {
        const allJobs = new Map<string, Job>();
        const addJob = (job: Job) => {
            const key = String(job.id ?? job.jobId ?? '');
            if (!key || allJobs.has(key)) return;
            allJobs.set(key, job);
            job.children?.forEach(addJob);
        };
        process.jobs?.forEach(addJob);

        const jobKey = (job: Job) => String(job.id ?? job.jobId ?? '');
        const parentKey = (job: Job) => {
            if (!job.parent) return undefined;
            const parent = job.parent as Job | string;
            return typeof parent === 'string'
                ? parent
                : String(parent.id ?? parent.jobId ?? '');
        };
        const childrenByParent = new Map<string, Job[]>();
        allJobs.forEach((job) => {
            const parent = parentKey(job);
            if (parent && allJobs.has(parent)) {
                childrenByParent.set(parent, [...(childrenByParent.get(parent) ?? []), job]);
            }
        });
        // Nested children are authoritative when parent references are not available.
        allJobs.forEach((job) => {
            if (job.children?.length) childrenByParent.set(jobKey(job), job.children.filter(child => allJobs.has(jobKey(child))));
        });

        const roots = [...allJobs.values()].filter(job => !parentKey(job));
        const orderedRoots = roots.length ? roots : [...allJobs.values()];
        const nodes: Node[] = [];
        const edges: Edge[] = [];
        const depthCounts: number[] = [];
        const visited = new Set<string>();
        const rootIds: string[] = [];
        const visit = (job: Job, depth: number, parentId?: string) => {
            const id = jobKey(job);
            if (!id || visited.has(id)) return;
            visited.add(id);
            const row = depthCounts[depth] ?? 0;
            depthCounts[depth] = row + 1;
            const statusColors = getJobStatusColors(job.status);
            nodes.push({
                id,
                position: { x: depth * 320, y: row * 130 },
                data: { label: (
                    <Box sx={{ minWidth: 230, px: 1.25, py: 1, color: statusColors.color }}>
                        <Typography variant="subtitle2" sx={{ fontWeight: 600 }}>{job.name || t('jobs.name')}</Typography>
                        <Typography variant="body2">{convertOperation(job.operation)}</Typography>
                    </Box>
                ) },
                style: {
                    width: 260,
                    padding: 0,
                    border: `1px solid ${theme.palette.divider}`,
                    borderRadius: 8,
                    background: statusColors.background,
                },
            });
            if (parentId) edges.push({ id: `${parentId}-${id}`, source: parentId, target: id, type: 'smoothstep' });
            else if (!parentKey(job)) rootIds.push(id);
            const childJobs = childrenByParent.get(id) ?? [];
            childJobs
                .slice()
                .sort((a, b) => Number(a.jobId) - Number(b.jobId))
                .forEach(child => visit(child, depth + 1, id));
        };
        orderedRoots.slice().sort((a, b) => Number(a.jobId) - Number(b.jobId)).forEach(job => visit(job, 0));
        // Keep disconnected jobs visible if the API supplied incomplete parent links.
        allJobs.forEach(job => { if (!visited.has(jobKey(job))) visit(job, 0); });

        const leafIds = nodes
            .filter(node => !edges.some(edge => edge.source === node.id))
            .map(node => node.id);
        const startId = 'process-start';
        const endId = 'process-end';
        const rootRows = nodes.filter(node => rootIds.includes(node.id)).map(node => node.position.y);
        const leafRows = nodes.filter(node => leafIds.includes(node.id)).map(node => node.position.y);
        const centerRow = (rows: number[]) => rows.length ? rows.reduce((sum, row) => sum + row, 0) / rows.length : 0;
        const terminalDepth = (depthCounts.length || 1) * 320;
        nodes.unshift({
            id: startId,
            type: 'processBoundary',
            position: { x: -320, y: centerRow(rootRows) },
            data: { label: 'Process Start', kind: 'start' },
        });
        nodes.push({
            id: endId,
            type: 'processBoundary',
            position: { x: terminalDepth, y: centerRow(leafRows) },
            data: { label: 'Process End', kind: 'end' },
        });
        if (allJobs.size === 0) {
            edges.push({ id: `${startId}-${endId}`, source: startId, target: endId, type: 'smoothstep' });
        } else {
            rootIds.forEach(id => edges.push({ id: `${startId}-${id}`, source: startId, target: id, type: 'smoothstep' }));
            leafIds.forEach(id => edges.push({ id: `${id}-${endId}`, source: id, target: endId, type: 'smoothstep' }));
        }
        return { nodes, edges, rows: [...allJobs.values()].sort((a, b) => Number(a.jobId) - Number(b.jobId)) };
    }, [process.jobs, textColor, theme.palette.divider, theme.palette, translate]);

    const changeJobView = (_: React.MouseEvent<HTMLElement>, value: 'list' | 'tree' | null) => {
        if (!value) return;
        setJobView(value);
        try { localStorage.setItem('processInfo.jobView', value); } catch { /* preference storage is optional */ }
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
            <ToggleButtonGroup size="small" exclusive value={jobView} onChange={changeJobView} sx={{ mb: 1 }}>
                <ToggleButton value="list">{t('processes.jobViewList', 'List')}</ToggleButton>
                <ToggleButton value="tree">{t('processes.jobViewTree', 'Tree')}</ToggleButton>
            </ToggleButtonGroup>
            {jobView === 'tree' ? (
                <Box sx={{ height: { xs: '62vh', sm: '72vh' }, minHeight: { xs: 420, sm: 560 }, maxHeight: 800, width: '100%', border: 1, borderColor: 'divider', borderRadius: 1 }}>
                    <ReactFlow nodes={jobs.nodes} edges={jobs.edges} nodeTypes={nodeTypes} fitView nodesDraggable={false} nodesConnectable={false} elementsSelectable={false}>
                        <Background />
                        <Controls />
                    </ReactFlow>
                </Box>
            ) : (
            <TableContainer component={Paper}>
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ color: textColor }}>{t('jobs.jobId')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.name')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.operation')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.status')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.startDate')}</TableCell>
                            <TableCell sx={{ color: textColor }}>{t('jobs.endDate')}</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {jobs.rows.map((job: Job) => (
                            <TableRow key={job.id}>
                                <TableCell sx={{ color: textColor }}>{job.jobId}</TableCell>
                                <TableCell sx={{ color: textColor }}>{job.name}</TableCell>
                                <TableCell sx={{ color: textColor }}>{convertOperation(job.operation)}</TableCell>
                                <TableCell sx={{ color: textColor }}>{convertJobStatus(job.status)}</TableCell>
                                <TableCell sx={{ color: textColor }}>{job.startDate == null ? '-' : convertToDateStr(job.startDate.toString())}</TableCell>
                                <TableCell sx={{ color: textColor }}>{job.endDate == null ? '-' : convertToDateStr(job.endDate.toString())}</TableCell>
                            </TableRow>
                        ))}
                        {jobs.rows.length === 0 && (
                            <TableRow>
                                <TableCell colSpan={6} align="center" sx={{ color: textColor }}>
                                    {t('processes.noJobs')}
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
            )}
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
                                <TableCell colSpan={2} align="center" sx={{ color: textColor }}>
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
