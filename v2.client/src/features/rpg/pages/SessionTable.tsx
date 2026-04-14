// ===================== SessionTable.tsx (FULL MODIFIED FILE) =====================
import { Filter } from "@/shared/components/filter";
import OperationCell from "@/shared/components/operationCell";
import { Paper, TableContainer, Table, TableHead, TableRow, TableCell, TableSortLabel, TableBody, IconButton, Collapse, Box, Accordion, AccordionSummary, Typography, AccordionDetails, TablePagination, Button, Grid, InputLabel, Menu, MenuItem, useMediaQuery, useTheme } from "@mui/material";
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';

import { useState } from "react";
import { HeroDto, SessionDto, Story, ImportDto } from "@/features/rpg";
import { useModal } from "@/shared/context/modal";
import StoryEdit from "@/features/rpg/components/StoryEdit";
import SessionInfo from "@/features/rpg/components/StoryInfo";
import { useApiConnect } from "@/shared/context/apiConnect";
import React from "react";
import { ChapterTable } from "./ChapterTable";
import { useTranslation } from "react-i18next";
import SummaryGen from "@/features/rpg/components/summaryGen";
import { convertToDateStr } from "@/lib/utils";
import { saveAs } from 'file-saver';
import { onChangeParams, ColumnDef, ColumnType, Operations, FilterItem, FilterType, FilterValue } from "@/shared";
import YesNoWindow from "@/shared/components/YesNoWindow";
import SessionForm from "../components/sessionForm";
import ImportStory from "../components/importStory";

export type SessionTableProps = {
    updateData: (paramsObj: onChangeParams) => void;
    data: Story[],
    rowCount?: number;
    setRowCount?: (count: number) => void;
};

export const SessionTable: React.FC<SessionTableProps> = ({ updateData, data, rowCount }: SessionTableProps) => {
    const { t } = useTranslation();
    const modal = useModal();
    const api = useApiConnect();
    const [openRows, setOpenRows] = useState<Record<string, boolean>>({});
    const [pageSize, setPageSize] = useState(10);
    const [orderBy, setOrderBy] = useState<string | null>(null);
    const [order, setOrder] = useState<'asc' | 'desc'>('asc');
    const [page, setPage] = useState(0);
    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    // NEW: dropdown state
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const openMenu = Boolean(anchorEl);

    const handleMenuClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => setAnchorEl(null);

    const refresh = () => {
        modal.hideModal();
        updateData({ page, pageSize, orderBy, order, filters: filterValues });
    }

    const toggleRow = (id: string) => {
        setOpenRows(prev => ({ ...prev, [id]: !prev[id] }));
    };

    const handleSort = (field: string) => {
        setOrderBy(field);
        const isAsc = orderBy === field && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        updateData({ page, pageSize, orderBy, order, filters: filterValues });
    };

    const handleFilterChange = (newFilters: FilterValue[]) => {
        setFilterValues(newFilters);
        updateData({ page, pageSize, orderBy, order, filters: newFilters });
    }

    const handleChangePage = (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
        setPage(newPage);
        updateData({ page, pageSize, orderBy, order, filters: filterValues });
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setPageSize(parseInt(event.target.value, 10));
        updateData({ page: 0, pageSize: parseInt(event.target.value, 10), orderBy, order, filters: filterValues });
    };

    const addSession = async () => {
        modal.showModal(<StoryEdit story={{} as Story} toSave={saveNew} />)
    }

    const saveNew = (data: Story) => {
        api.post<Story>('rpg_stories_new', data, null).then(() => refresh());
    }

    // NEW: import modal
    const openImportModal = () => {
        modal.showModal(<ImportStory onSubmit={handleImport} />);
    }

    const handleImport = (data: any) => {
        const formData = new FormData();

        if (data.file) formData.append("File", data.file);
        formData.append("ConverterType", data.converterType.toString());
        if (data.externalUrl) formData.append("ExternalUrl", data.externalUrl);

        api.post('rpg_stories_import', formData, {
            headers: { "Content-Type": "multipart/form-data" }
        }).then(() => {
            modal.hideModal();
            refresh();
        });
    }

    const details = (data: any) => {
        api.get<Story>('rpg_stories_details', null, data.id)
            .then(story => modal.showModal(<SessionInfo story={story.data} edit={editSession} del={del}></SessionInfo>))
    }

    const editSession = (data: any) => {
        modal.showModal(<StoryEdit story={data} toSave={(o) => saveEdit(o, data.id)} />)
    }

    const saveEdit = (data: Story, id: string) => {
        api.put<Story>('rpg_stories_edit', data, null, id).then(() => refresh());
    }

    const addChapter = (data: Story) => {
        var chapter = {} as SessionDto;
        chapter.story = data.id;
        modal.showModal(<SessionForm data={chapter} onSave={(o) => saveChapter(o)} isChapter={true} isNew={true} />)
    }

    const saveChapter = (data: SessionDto) => {
        api.post<SessionDto>('rpg_chapter_new', data, null).then(() => {
            modal.hideModal();
            refresh();
        });
    }

    const del = (data: any) => {
        modal.showModal(<YesNoWindow message={t('entity.del_info')} yesMethod={() => delConfirm(data)} open={true} onClose={modal.hideModal} noMethod={modal.hideModal} />);
    }

    const delConfirm = (data: any) => {
        api.del<Story>('rpg_stories_del', null, data.id).then(() => refresh());
    }

    const startStory = (data: any) => {
        api.put<Story>('rpg_stories_start', data, null, data.id).then(() => refresh());
    }

    const endStory = (data: any) => {
        api.put<Story>('rpg_stories_end', data, null, data.id).then(() => refresh());
    }

    const generateSummary = (data: any) => {
        modal.showModal(<SummaryGen story={data} onProcess={generateSummaryConfirm} />);
    }

    const generateSummaryConfirm = (data: Story, isPdf: boolean) => {
        api.put<Story>('rpg_stories_gen_summary', { id: data.id, title: data.title, description: data.description, chapters: data.chapters.map((x) => x.id), isPdf }, null, data.id)
            .then(() => modal.hideModal());
    }

    const sendToFirebase = (data: any) => {
        modal.showModal(<SummaryGen story={data} onProcess={sendToFirebaseConfirm} forFirebase />);
    }

    const sendToFirebaseConfirm = (data: Story) => {
        api.put<Story>('rpg_stories_firebase', { id: data.id, title: data.title, description: data.description, chapters: data.chapters.map((x) => x.id) }, null, data.id)
            .then(() => modal.hideModal());
    }

    const exportData = (data: any) => {
        api.download('rpg_stories_export', data.id)
            .then((response) => {
                const contentType = response.headers['content-type'] || 'application/octet-stream';
                const disposition = response.headers['content-disposition'];
                let filename = data.title + '.json';

                if (disposition) {
                    const match = disposition.match(/filename\*?=(?:UTF-8'')?([^;]+)/);
                    if (match?.[1]) filename = decodeURIComponent(match[1].replace(/"/g, ''));
                }

                const blob = new Blob([response.data], { type: contentType });
                saveAs(blob, filename);
            })
            .catch((error) => console.error('Download failed:', error));
    }

    const downloadSummary = (data: any) => {
        api.download('rpg_stories_download_summary', data.id)
            .then((response) => {
                const contentType = response.headers['content-type'] || 'application/octet-stream';
                const disposition = response.headers['content-disposition'];
                let filename = data.title + '_summary' + (contentType === 'application/pdf' ? '.pdf' : '.html');

                if (disposition) {
                    const match = disposition.match(/filename\*?=(?:UTF-8'')?([^;]+)/);
                    if (match?.[1]) filename = decodeURIComponent(match[1].replace(/"/g, ''));
                }

                const blob = new Blob([response.data], { type: contentType });
                saveAs(blob, filename);
            })
            .catch((error) => console.error('Download failed:', error));
    }

    const columns: ColumnDef[] = [
        { field: 'title', header: 'rpg.story.title', type: ColumnType.String },
        { field: 'startDate', header: 'rpg.story.startDate', type: ColumnType.Date },
        { field: 'endDate', header: 'rpg.story.endDate', type: ColumnType.Date },
    ];

    const filters: FilterItem[] = [
        { field: 'title', name: 'rpg.story.title', type: FilterType.String },
        { field: 'startDate', name: 'rpg.story.startDate', type: FilterType.Date },
        { field: 'endDate', name: 'rpg.story.endDate', type: FilterType.Date }
    ];

    const operations: Operations<Story>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => editSession(o) },
        { name: 'rpg.chapter.add', method: (o) => addChapter(o) },
        { name: 'rpg.story.start', method: (o) => startStory(o) },
        { name: 'rpg.story.end', method: (o) => endStory(o) },
        { name: 'opt.export', method: (o) => exportData(o) },
        { name: 'rpg.story.gen_summary', method: (o) => generateSummary(o) },
        { name: 'rpg.story.download_summary', method: (o) => downloadSummary(o) },
        { name: 'rpg.story.send_firebase', method: (o) => sendToFirebase(o) },
        { name: 'opt.delete', method: (o) => del(o) },
    ]

        return (
        <Grid sx={{ width: "100%", p: isMobile ? 1 : 3 }}>
            {/* HEADER */}
            <Grid sx={{ textAlign: "center", mb: 2 }}>
                <InputLabel sx={{ fontSize: isMobile ? "1.8rem" : "2.5rem", fontWeight: "bold" }}>
                    {t('rpg.title')}
                </InputLabel>

                <Button onClick={handleMenuClick} variant="outlined" sx={{ mt: 1 }}>
                    {t('opt.add')}
                </Button>

                <Menu anchorEl={anchorEl} open={openMenu} onClose={handleMenuClose}>
                    <MenuItem onClick={() => { handleMenuClose(); addSession(); }}>
                        {t('opt.create')}
                    </MenuItem>
                    <MenuItem onClick={() => { handleMenuClose(); openImportModal(); }}>
                        {t('opt.import')}
                    </MenuItem>
                </Menu>
            </Grid>

            {/* TABLE */}
            <Paper sx={{
                width: "100%",
                maxWidth: 1200,
                mx: "auto",
                p: isMobile ? 1 : 2,
                borderRadius: 2
            }}>
                <Filter filters={filters} onChange={handleFilterChange} />

                <TableContainer sx={{ overflowX: "auto" }}>
                    <Table size={isMobile ? "small" : "medium"} stickyHeader>

                        <TableHead>
                            <TableRow>
                                <TableCell />

                                {columns.map(col => (
                                    <TableCell key={String(col.field)}>
                                        <TableSortLabel
                                            active={orderBy === col.field}
                                            direction={orderBy === col.field ? order : "asc"}
                                            onClick={() => handleSort(col.field)}
                                        >
                                            {t(col.header)}
                                        </TableSortLabel>
                                    </TableCell>
                                ))}

                                <TableCell />
                            </TableRow>
                        </TableHead>

                        <TableBody>
                            {data?.map(row => {
                                const isOpen = !!openRows[row.id];

                                return (
                                    <React.Fragment key={row.id}>
                                        <TableRow>
                                            <TableCell>
                                                <IconButton onClick={() => toggleRow(row.id)}>
                                                    {isOpen ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                                                </IconButton>
                                            </TableCell>

                                            {columns.map(col => (
                                                <TableCell key={String(col.field)}>
                                                    {col.type === "date"
                                                        ? convertToDateStr(row[col.field])
                                                        : String(row[col.field] ?? "")}
                                                </TableCell>
                                            ))}

                                            <OperationCell operations={operations} data={row} />
                                        </TableRow>

                                        <TableRow>
                                            <TableCell colSpan={10} sx={{ p: 0 }}>
                                                <Collapse in={isOpen} timeout="auto" unmountOnExit>
                                                    <Box sx={{ p: isMobile ? 1 : 2 }}>
                                                        <Typography variant="body2" sx={{ mb: 1 }}>
                                                            {row.description}
                                                        </Typography>

                                                        <Accordion>
                                                            <AccordionSummary expandIcon={<ArrowDownwardIcon />}>
                                                                <Typography>{t('rpg.story.chapters')}</Typography>
                                                            </AccordionSummary>
                                                            <AccordionDetails>
                                                                <ChapterTable chapters={row.chapters} />
                                                            </AccordionDetails>
                                                        </Accordion>
                                                    </Box>
                                                </Collapse>
                                            </TableCell>
                                        </TableRow>
                                    </React.Fragment>
                                );
                            })}
                        </TableBody>
                    </Table>
                </TableContainer>

                <TablePagination
                    component="div"
                    count={rowCount}
                    page={page}
                    onPageChange={handleChangePage}
                    rowsPerPage={pageSize}
                    onRowsPerPageChange={handleChangeRowsPerPage}
                    rowsPerPageOptions={[5, 10, 25, 50]}
                />
            </Paper>
        </Grid>
    );
};