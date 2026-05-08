// ===================== SessionTable.tsx (FULL MODIFIED FILE) =====================
import { Filter } from "@/shared/components/filter";
import OperationCell from "@/shared/components/operationCell";
import { Paper, TableContainer, Table, TableHead, TableRow, TableCell, TableSortLabel, TableBody, IconButton, Collapse, Box, Accordion, AccordionSummary, Typography, AccordionDetails, TablePagination, Button, Grid, InputLabel, Menu, MenuItem, useMediaQuery, useTheme } from "@mui/material";
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';

import { useState } from "react";
import { SessionDto, Story } from "@/features/rpg";
import { useModal } from "@/shared/context/modal";
import StoryEdit from "@/features/rpg/components/StoryEdit";
import SessionInfo from "@/features/rpg/components/StoryInfo";
import { call, raw } from "@/shared";
import React from "react";
import { ChapterTable } from "./ChapterTable";
import { useTranslation } from "react-i18next";
import SummaryGen from "@/features/rpg/components/summaryGen";
import { convertToDateStr, download } from "@/lib/utils";
import { saveAs } from 'file-saver';
import { onChangeParams, ColumnDef, ColumnType, Operations, FilterItem, FilterType, FilterValue } from "@/shared";
import YesNoWindow from "@/shared/components/YesNoWindow";
import SessionForm from "../components/sessionForm";
import ImportStory from "../components/importStory";
import { useAuth } from "@/features/auth/context/authProvider";

export type SessionTableProps = {
    updateData: (paramsObj: onChangeParams) => void;
    data: Story[],
    rowCount?: number;
    setRowCount?: (count: number) => void;
    draft: boolean;
};

export const SessionTable: React.FC<SessionTableProps> = ({ updateData, data, rowCount, draft }: SessionTableProps) => {
    const { t } = useTranslation();
    const { checkPermission } = useAuth();
    const modal = useModal();
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
        call(api => api.storiesApi.create,{storyDto: {storyDto:data}}).then(() => refresh());
    }

    // NEW: import modal
    const openImportModal = () => {
        modal.showModal(<ImportStory onSubmit={handleImport} />);
    }

    const handleImport = (data: any) => {

        call(api => api.storiesApi.createImport,{file:data.file, converterType:data.converterType,externalUrl:data.externalUrl}).then(() => {
            modal.hideModal();
            refresh();
        });
    }

    const details = (data: any) => {
        call<Story>(api => draft ? api.storiesApi.getByIdDraft : api.storiesApi.getById, {id: data.id})
            .then(story => modal.showModal(<SessionInfo story={story} edit={editSession} del={del}></SessionInfo>))
    }

    const editSession = (data: any) => {
        call<Story>(api => draft ? api.storiesApi.getByIdDraft : api.storiesApi.getById, {id: data.id})
            .then(story =>  modal.showModal(<StoryEdit story={story} toSave={(o) => saveEdit(o, data.id)} edit/>))
    }

    const saveEdit = (data: Story, id: string) => {
        call(api => api.storiesApi.updateById,{id:data.id,storyDto:data}).then(() => refresh());
    }

    const addChapter = (data: Story) => {
        const chapter = {} as SessionDto;
        chapter.story = data.id;
        modal.showModal(<SessionForm data={chapter} onSave={(o) => saveChapter(o)} isChapter={true} isNew={true} />)
    }

    const addDraftChapter = (data: Story) => {
        const chapter = {} as SessionDto;
        chapter.story = data.id;
        chapter.draft = true;
        modal.showModal(<SessionForm data={chapter} onSave={(o) => saveChapter(o)} isChapter={true} isNew={true} />)
    }

    const saveChapter = (data: SessionDto) => {
        call(api => api.chaptersApi.create,{chapterDto:data}).then(() => {
            modal.hideModal();
            refresh();
        });
    }

    const del = (data: any) => {
        modal.showModal(<YesNoWindow message={t('entity.del_info')} yesMethod={() => delConfirm(data)} open={true} onClose={modal.hideModal} noMethod={modal.hideModal} />);
    }

    const delConfirm = (data: any) => {
        call(api => api.storiesApi.deleteById,{id:data.id}).then(() => refresh());
    }

    const startStory = (data: any) => {
        call(api => api.storiesApi.updateByIdStart,{id:data.id}).then(() => refresh());
    }

    const endStory = (data: any) => {
        call(api => api.storiesApi.updateByIdEnd,{id:data.id}).then(() => refresh());
    }

    const generateSummary = (data: any) => {
        modal.showModal(<SummaryGen story={data} onProcess={generateSummaryConfirm} />);
    }

    const generateSummaryConfirm = (data: Story, isPdf: boolean) => {
        call(api => api.storiesApi.updateByIdSummary, {id:data.id, summaryModel:{id: data.id, title: data.title, description: data.description, chapters: data.chapters.map((x) => x.id), isPdf}})
            .then(() => modal.hideModal());
    }

    const sendToFirebase = (data: any) => {
        modal.showModal(<SummaryGen story={data} onProcess={sendToFirebaseConfirm} forFirebase />);
    }

    const sendToFirebaseConfirm = (data: Story) => {
        call(api => api.storiesApi.updateByIdFirebase,{id:data.id, summaryModel:{id: data.id, title: data.title, description: data.description, chapters: data.chapters.map((x) => x.id) }})
            .then(() => modal.hideModal());
    }

    const exportData = (data: any) => {
        raw(api => api.storiesApi.getByIdExport,{id:data.id})
            .then((response) => {
                const contentType = response.headers['content-type'] || 'application/octet-stream';
                const disposition = response.headers['content-disposition'];
                let filename = data.title + '.json';

                if (disposition) {
                    const match = disposition.match(/filename\*?=(?:UTF-8'')?([^;]+)/);
                    if (match?.[1]) filename = decodeURIComponent(match[1].replace(/"/g, ''));
                }

                const blob = new Blob([response.data], { type: contentType.toLocaleString() });
                saveAs(blob, filename);
            })
            .catch((error) => console.error('Download failed:', error));
    }

    const downloadSummary = (data: Story) => download(data.summary,data.title);

    const columns: ColumnDef[] = [
        { field: 'title', header: 'rpg.story.title', type: ColumnType.String },
        { field: 'startDate', header: 'rpg.story.startDate', type: ColumnType.Date },
        { field: 'endDate', header: 'rpg.story.endDate', type: ColumnType.Date },
    ];

    const filters: FilterItem[] = [
        { field: 'title', name: 'rpg.story.title', type: FilterType.String },
        { field: 'start', name: 'rpg.story.startDate', type: FilterType.DateRange },
        { field: 'end', name: 'rpg.story.endDate', type: FilterType.DateRange }
    ];

    const operations: Operations<Story>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => editSession(o), hidden: (o) => !checkPermission(['rpg_write']) },
        { name: 'rpg.chapter.add', method: (o) => addChapter(o), hidden: (o) => !checkPermission(['rpg_write']) || draft },
        { name: 'rpg.chapter.addDraft', method: (o) => addDraftChapter(o), hidden: (o) => !checkPermission(['rpg_write']) || !draft },
        { name: 'rpg.story.start', method: (o) => startStory(o) },
        { name: 'rpg.story.end', method: (o) => endStory(o) },
        { name: 'opt.export', method: (o) => exportData(o) },
        { name: 'rpg.story.gen_summary', method: (o) => generateSummary(o) },
        { name: 'rpg.story.download_summary', method: (o) => downloadSummary(o) },
        { name: 'rpg.story.send_firebase', method: (o) => sendToFirebase(o) },
        { name: 'opt.delete', method: (o) => del(o), hidden: (o) => !checkPermission(['rpg_write']) },
    ]

    return (
        <Grid sx={{ maxWidth: "100%", p: isMobile ? 1 : 3 }}>
            {/* HEADER */}
            <Grid sx={{ textAlign: "center", mb: 2 }}>
                <InputLabel sx={{ fontSize: isMobile ? "1.8rem" : "2.5rem", fontWeight: "bold" }}>
                    {t(draft ? 'rpg.draftTitle' : 'rpg.title')}
                </InputLabel>


                {checkPermission(['rpg_write']) && (<>
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
                </>
                )}
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

            <TableContainer sx={{
                width: "100%",
                overflowX: "auto",
                size: isMobile ? 'small' : 'medium',
                WebkitOverflowScrolling: "touch"
            }}>
                    <Table
                        stickyHeader
                        sx={{
                            maxWidth:'100%',
                            WebkitOverflowScrolling: "touch"
                        }}
                    >
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