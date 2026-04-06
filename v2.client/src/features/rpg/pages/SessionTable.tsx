import { Filter } from "@/shared/components/filter";
import OperationCell from "@/shared/components/operationCell";
import { Paper, TableContainer, Table, TableHead, TableRow, TableCell, TableSortLabel, TableBody, IconButton, Collapse, Box, Accordion, AccordionSummary, Typography, AccordionDetails, TablePagination, Button, Grid, InputLabel, TextField } from "@mui/material";
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';


import { useState } from "react";
import { HeroDto, SessionDto, Story } from "@/features/rpg";
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

export type SessionTableProps = {
    updateData: (paramsObj: onChangeParams) => void;
    data: Story[],
    rowCount?: number;
    setRowCount?: (count: number) => void;
};

export const SessionTable: React.FC<SessionTableProps> = ({ updateData, data, rowCount, setRowCount }: SessionTableProps) => {
    const { t } = useTranslation();
    const modal = useModal();
    const api = useApiConnect();
    const [open, setOpen] = useState(false);
    const [pageSize, setPageSize] = useState(10);
    const [orderBy, setOrderBy] = useState<string | null>(null);
    const [order, setOrder] = useState<'asc' | 'desc'>('asc');
    const [page, setPage] = useState(0);
    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

    const refresh = () => {
        modal.hideModal();
        updateData({
            page,
            pageSize,
            orderBy,
            order,
            filters: filterValues,
        });
    }

    const handleSort = (field: string) => {
        setOrderBy(field);
        const isAsc = orderBy === field && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        updateData({
            page,
            pageSize,
            orderBy,
            order,
            filters: filterValues,
        });
    };

    const handleFilterChange = (newFilters: FilterValue[]) => {
        setFilterValues(newFilters);
        updateData({
            page,
            pageSize,
            orderBy,
            order,
            filters: newFilters,
        });
    }
    const handleChangePage = (
        event: React.MouseEvent<HTMLButtonElement> | null,
        newPage: number
    ) => {
        setPage(newPage);
        updateData({
            page,
            pageSize,
            orderBy,
            order,
            filters: filterValues,
        });
    };

    const handleChangeRowsPerPage = (
        event: React.ChangeEvent<HTMLInputElement>
    ) => {
        setPageSize(parseInt(event.target.value, 10));
        updateData({
            page: 0,
            pageSize: parseInt(event.target.value, 10),
            orderBy,
            order,
            filters: filterValues,
        });
    };

    const addSession = async (story: any): Promise<void> => {
        modal.showModal(<StoryEdit story={{} as Story} toSave={saveNew} />)
    }

    const saveNew = (data: Story) => {
        api.post<Story>('rpg_stories_new', data, null)
            .then(() => {
                refresh();
            });
    }

    const details = (data: any) => {
        api.get<Story>('rpg_stories_details', null, data.id).
            then(story => modal.showModal(<SessionInfo story={story.data} edit={editSession} del={del}></SessionInfo>))
    }

    const editSession = (data: any) => {
        modal.showModal(<StoryEdit story={data} toSave={(o) => saveEdit(o, data.id)} />)
    }

    const saveEdit = (data: Story, id: string) => {
        api.put<Story>('rpg_stories_edit', data, null, id)
            .then(() => {
                refresh();
            });
    }

    const addChapter = (data: Story) => {
        var chapter =  {} as SessionDto;
        chapter.story = data.id;
        modal.showModal(<SessionForm data={chapter} onSave={(o) => saveChapter(o)} isChapter={true} isNew={true} />)
    }

    const saveChapter = (data: SessionDto) => {
        api.post<SessionDto>('rpg_chapter_new', data, null)
            .then(() => {
                modal.hideModal();
                refresh();
            });
    }

    const del = (data: any) => {
        modal.showModal(<YesNoWindow message={t('entity.del_info')} yesMethod={() => delConfirm(data)} open={true} onClose={modal.hideModal} noMethod={modal.hideModal} />);
    }

    const delConfirm = (data: any) => {
        api.del<Story>('rpg_stories_del', null, data.id)
            .then(() => {
                refresh();
            });
    }

    const startStory = (data: any) => {
        api.put<Story>('rpg_stories_start', data, null, data.id)
            .then(() => {
                refresh();
            });
    }

    const endStory = (data: any) => {
        api.put<Story>('rpg_stories_end', data, null, data.id)
            .then(() => {
                refresh();
            });
    }

    const generateSummary = (data: any) => {
        modal.showModal(<SummaryGen story={data} onProcess={generateSummaryConfirm} />);
    }

    const generateSummaryConfirm = (data: Story, isPdf: boolean) => {
        api.put<Story>('rpg_stories_gen_summary', { id: data.id, title: data.title, description: data.description, chapters: data.chapters.map((x) => x.id), isPdf }, null, data.id)
            .then(() => {
                modal.hideModal();
            });
    }

    const sendToFirebase = (data: any) => {
        modal.showModal(<SummaryGen story={data} onProcess={sendToFirebaseConfirm} forFirebase />);
    }

    const sendToFirebaseConfirm = (data: Story) => {
        api.put<Story>('rpg_stories_firebase', { id: data.id, title: data.title, description: data.description, chapters: data.chapters.map((x) => x.id) }, null, data.id)
            .then(() => {
                modal.hideModal();
            });
    }

    const downloadSummary = (data: any) => {
    api.download('rpg_stories_download_summary', data.id)
        .then((response) => {
            // Extract content type from response headers
            const contentType = response.headers['content-type'] || 'application/octet-stream';

            // Extract filename from Content-Disposition header
            const disposition = response.headers['content-disposition'];
            let filename = data.title + '_summary' + (contentType === 'application/pdf' ? '.pdf' : '.html');

            if (disposition) {
                const match = disposition.match(/filename\*?=(?:UTF-8'')?([^;]+)/);
                if (match?.[1]) {
                    filename = decodeURIComponent(match[1].replace(/"/g, ''));
                }
            }

            // Create a Blob from the response data
            const blob = new Blob([response.data], { type: contentType });

            // Use file-saver to trigger download
            saveAs(blob, filename);
        })
        .catch((error) => {
            console.error('Download failed:', error);
        });

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
        { name: 'opt.details', method: (o) => details(o)},
        { name: 'opt.edit', method: (o) => editSession(o) },
        { name: 'rpg.chapter.add', method: (o) => addChapter(o) },
        { name: 'rpg.story.start', method: (o) => startStory(o) },
        { name: 'rpg.story.end', method: (o) => endStory(o) },
        { name: 'rpg.story.gen_summary', method: (o) => generateSummary(o) },
        { name: 'rpg.story.download_summary', method: (o) => downloadSummary(o) },
        { name: 'rpg.story.send_firebase', method: (o) => sendToFirebase(o) },
        { name: 'opt.delete', method: (o) => del(o) }
    ]

    return <>
        <Grid style={{ width: '100%', margin: 'auto', padding: '20px' }}>
            <Grid style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginBottom: '20px', flexDirection: "column" }}>
                <InputLabel style={{
                    color: 'white',
                    fontSize: '2.5rem',
                    fontWeight: 'bold',
                    display: 'inline-block'
                }}>
                    {t('rpg.title')}
                </InputLabel>
                <InputLabel style={{
                    color: 'white',
                    fontSize: '1rem',
                    fontWeight: 'bold',
                    display: 'inline-block'
                }}>
                    {t('rpg.description')}
                </InputLabel>
                <Button onClick={addSession} variant="outlined">
                    {t('opt.add')}
                </Button>
            </Grid>

            <Paper sx={{ width: '75%', overflow: 'hidden', margin: 'auto', padding: 2 }}>
                <Filter filters={filters} onChange={handleFilterChange} />
                <TableContainer>
                    <Table size="small" stickyHeader >
                        <TableHead>
                            <TableRow>
                                <TableCell></TableCell>
                                {columns.map((col) => (
                                    <TableCell
                                        key={String(col.field)}
                                        sortDirection={orderBy === col.field ? order : false}
                                    >
                                        <TableSortLabel
                                            active={orderBy === col.field}
                                            direction={orderBy === col.field ? order : 'asc'}
                                            onClick={() => handleSort(col.field)}
                                        >
                                            {t(col.header)}
                                        </TableSortLabel>
                                    </TableCell>
                                ))}
                                <TableCell></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {data && (data.map((row: Story) => (
                                <>
                                    <TableRow key={row.id}>
                                        <TableCell>
                                            <IconButton
                                                aria-label="expand row"
                                                size="small"
                                                onClick={() => setOpen(!open)}
                                            >
                                                {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                                            </IconButton>
                                        </TableCell>
                                        {columns.map((col) => (
                                            <TableCell key={String(col.field)}>
                                                {col.toShow && row[col.field] != null
                                                    ? col.toShow(row[col.field],)
                                                    : col.type === 'date'
                                                        ? convertToDateStr(row[col.field])
                                                        : String(row[col.field])}
                                            </TableCell>
                                        ))}
                                        <OperationCell operations={operations} data={row} />
                                    </TableRow>
                                    <TableRow>
                                        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
                                            <Collapse in={open} timeout="auto" unmountOnExit>
                                                <Box sx={{ margin: 1 }}>
                                                    <Typography
                                                        variant="body1"
                                                        gutterBottom
                                                        component="label"
                                                    >{row.description}</Typography>
                                                    <Accordion>
                                                        <AccordionSummary
                                                            expandIcon={<ArrowDownwardIcon />}
                                                            aria-controls="panel1-content"
                                                            id="panel1-header"
                                                        >
                                                            <Typography component="span">{t('rpg.story.chapters')}</Typography>
                                                        </AccordionSummary>
                                                        <AccordionDetails>
                                                            <ChapterTable chapters={row.chapters} />
                                                        </AccordionDetails>
                                                    </Accordion>
                                                </Box>
                                            </Collapse>
                                        </TableCell>
                                    </TableRow>
                                </>

                            )))}
                            {(!data || data.length == 0) && (
                                <TableRow>
                                    <TableCell align="center" colSpan={columns.length}>
                                        {t('no_data')}
                                    </TableCell>
                                </TableRow>
                            )}
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
    </>;
}