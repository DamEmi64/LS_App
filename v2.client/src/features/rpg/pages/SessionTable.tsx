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
import { ExpandableTable } from "@/shared";
import React from "react";
import { ChapterTable } from "./ChapterTable";
import { useTranslation } from "react-i18next";
import SummaryGen from "@/features/rpg/components/summaryGen";
import { onChangeParams, TableColumn, ColumnType, Operations, FilterItem, FilterType, FilterValue } from "@/shared";
import YesNoWindow from "@/shared/components/YesNoWindow";
import SessionForm from "../components/sessionForm";
import ImportStory from "../components/importStory";
import { useAuth } from "@/features/auth/context/authProvider";
import {
    createChapter,
    createStory,
    deleteStory,
    endStory,
    exportStory,
    generateStorySummary,
    getStoryById,
    importStory,
    sendStoryToFirebase,
    startStory,
    updateStory,
    downloadStorySummary,
} from "../services/storyService";

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

    const saveNew = async (data: Story) => {
        await createStory(data);
        refresh();
    }

    // NEW: import modal
    const openImportModal = () => {
        modal.showModal(<ImportStory onSubmit={handleImport} />);
    }

    const handleImport = async (data: any) => {
        await importStory(data.file, data.converterType, data.externalUrl);
        modal.hideModal();
        refresh();
    }

    const details = async (data: any) => {
        const story = await getStoryById(data.id, draft);
        modal.showModal(<SessionInfo story={story} edit={editSession} del={del}></SessionInfo>);
    }

    const editSession = async (data: any) => {
        const story = await getStoryById(data.id, draft);
        modal.showModal(<StoryEdit story={story} toSave={(o) => saveEdit(o, data.id)} edit />);
    }

    const saveEdit = async (data: Story, id: string) => {
        await updateStory(data);
        refresh();
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

    const saveChapter = async (data: SessionDto) => {
        await createChapter(data);
        modal.hideModal();
        refresh();
    }

    const del = (data: any) => {
        modal.showModal(<YesNoWindow message={t('entity.del_info')} yesMethod={() => delConfirm(data)} open={true} onClose={modal.hideModal} noMethod={modal.hideModal} />);
    }

    const delConfirm = async (data: any) => {
        await deleteStory(data.id);
        refresh();
    }

    const startStory = async (data: any) => {
        await startStory(data.id);
        refresh();
    }

    const endStory = async (data: any) => {
        await endStory(data.id);
        refresh();
    }

    const generateSummary = (data: any) => {
        modal.showModal(<SummaryGen story={data} onProcess={generateSummaryConfirm} />);
    }

    const generateSummaryConfirm = async (data: Story, isPdf: boolean) => {
        await generateStorySummary(data, isPdf);
        modal.hideModal();
    }

    const sendToFirebase = (data: any) => {
        modal.showModal(<SummaryGen story={data} onProcess={sendToFirebaseConfirm} forFirebase />);
    }

    const sendToFirebaseConfirm = async (data: Story) => {
        await sendStoryToFirebase(data);
        modal.hideModal();
    }

    const exportData = async (data: any) => {
        try {
            await exportStory(data);
        } catch (error) {
            console.error('Download failed:', error);
        }
    }

    const downloadSummary = (data: Story) => downloadStorySummary(data);

    const columns: TableColumn<Story>[] = [
        { field: 'title', header: 'rpg.story.title', type: ColumnType.String, sortable: true },
        { field: 'startDate', header: 'rpg.story.startDate', type: ColumnType.Date, sortable: true },
        { field: 'endDate', header: 'rpg.story.endDate', type: ColumnType.Date, sortable: true },
    ];

    const filters: FilterItem[] = [
        { field: 'title', name: 'rpg.story.title', type: FilterType.String },
        { field: 'start', name: 'rpg.story.startDate', type: FilterType.DateRange },
        { field: 'end', name: 'rpg.story.endDate', type: FilterType.DateRange }
    ];

    const operations: Operations<Story>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => editSession(o), hidden: (o) => !checkPermission(['rpg-write']) },
        { name: 'rpg.chapter.add', method: (o) => addChapter(o), hidden: (o) => !checkPermission(['rpg-write']) || draft },
        { name: 'rpg.chapter.addDraft', method: (o) => addDraftChapter(o), hidden: (o) => !checkPermission(['rpg-write']) || !draft },
        { name: 'rpg.story.start', method: (o) => startStory(o) },
        { name: 'rpg.story.end', method: (o) => endStory(o) },
        { name: 'opt.export', method: (o) => exportData(o) },
        { name: 'rpg.story.gen_summary', method: (o) => generateSummary(o) },
        { name: 'rpg.story.download_summary', method: (o) => downloadSummary(o) },
        { name: 'rpg.story.send_firebase', method: (o) => sendToFirebase(o) },
        { name: 'opt.delete', method: (o) => del(o), hidden: (o) => !checkPermission(['rpg-write']) },
    ]

    return (
        <Grid sx={{ maxWidth: "100%", p: isMobile ? 1 : 3 }}>
            {/* HEADER */}
            <Grid sx={{ textAlign: "center", mb: 2 }}>
                <InputLabel sx={{ fontSize: isMobile ? "1.8rem" : "2.5rem", fontWeight: "bold" }}>
                    {t(draft ? 'rpg.draftTitle' : 'rpg.title')}
                </InputLabel>


                {checkPermission(['rpg-write']) && (<>
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
                <ExpandableTable
                    rows={data}
                    columns={columns}
                    operations={operations}
                    getRowId={(x) => x.id}
                    order={order}
                    orderBy={orderBy}
                    onSort={handleSort}
                    filters={filters}
                    onFilterChange={handleFilterChange}
                    renderExpanded={(row) => (
                        <>
                            <Typography
                                variant="body2"
                                sx={{ mb: 1 }}
                            >
                                {row.description}
                            </Typography>

                            <Accordion>
                                <AccordionSummary
                                    expandIcon={
                                        <ArrowDownwardIcon />
                                    }
                                >
                                    <Typography>
                                        {t('rpg.story.chapters')}
                                    </Typography>
                                </AccordionSummary>

                                <AccordionDetails>
                                    <ChapterTable
                                        chapters={row.chapters}
                                    />
                                </AccordionDetails>
                            </Accordion>
                        </>
                    )}
                />
            </Paper>
        </Grid>
    );
};