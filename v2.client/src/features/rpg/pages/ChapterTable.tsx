import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';

import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box,
    CircularProgress,
    Collapse,
    IconButton,
    Table,
    TableBody,
    TableCell,
    TableRow,
    Typography,
    useMediaQuery,
    useTheme
} from "@mui/material";

import { useState } from "react";
import { useTranslation } from "react-i18next";

import { HeroTable } from "./HeroTable";
import { PlaceTable } from "./PlaceTable";
import { convertToDateStr } from '@/lib/utils';
import DMPage from './DMPage';

import SessionView from '@/features/rpg/components/sessionForm';
import { useModal, call, Operations } from '@/shared';
import OperationCell from '@/shared/components/operationCell';
import YesNoWindow from '@/shared/components/YesNoWindow';

import { Chapter, SessionDto, HeroDto, Hero, Story } from '../types';
import HeroForm from '../components/heroForm';
import PlaceForm from "@/features/rpg/components/PlaceForm";
import { useAuth } from '@/features/auth/context/authProvider';
import { ProgressFlow } from '../components/flow/ProgressFlow';

export type ChapterTableProps = {
    chapters: Chapter[]
};

export const ChapterTable: React.FC<ChapterTableProps> = ({ chapters }) => {
    const { t } = useTranslation();
    const modal = useModal();
    const { checkPermission } = useAuth();

    // 📱 RESPONSIVE
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    // ✅ FIX: per-row state (instead of global open)
    const [openRows, setOpenRows] = useState<Record<string, boolean>>({});
    const [loadingRow, setLoadingRow] = useState<string | null>(null);

    const [data, setData] = useState<Chapter[]>(chapters);

    const toggleRow = (chapter: Chapter) => {
        const isOpen = !!openRows[chapter.id];

        if (isOpen) {
            setOpenRows(prev => ({ ...prev, [chapter.id]: false }));
            return;
        }

        setLoadingRow(chapter.id);

        call<Chapter>(api => api.chaptersApi.getChapterById,{id:chapter.id})
            .then((res) => {
                setLoadingRow(null);

                setData(prev =>
                    prev.map(c => c.id === chapter.id ? res : c)
                );

                setOpenRows(prev => ({
                    ...prev,
                    [chapter.id]: true
                }));
            });
    };

    const refresh = (chapter: Chapter) => {
        call<Chapter>(api => api.chaptersApi.getChapterById,{id:chapter.id})
            .then((res) => {
                setData(prev =>
                    prev.map(c => c.id === chapter.id ? res : c)
                );
            });
    };

    const operations: Operations<Chapter>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => edit(o), hidden: (o) => !checkPermission(['rpg_write']) },
        { name: 'opt.publish', method: (o) => publishChapter(o), hidden: (o) => !checkPermission(['rpg_write']) || !o.draft },
        { name: 'rpg.chapter.start', method: (o) => startChapter(o) },
        { name: 'rpg.chapter.end', method: (o) => endChapter(o) },
        { name: "rpg.chapter.dmPage", method: (o) => dmPage(o) },
        { name: 'rpg.flow.flow_title', method: (o) => flow(o)},
        { name: 'rpg.hero.add', method: (o) => addHero(o), hidden: (o) => !checkPermission(['rpg_write']) },
        { name: 'rpg.place.add', method: (o) => addPlace(o), hidden: (o) => !checkPermission(['rpg_write']) },
        { name: 'opt.delete', method: (o) => del(o), hidden: (o) => !checkPermission(['rpg_write']) },
    ];

    const details = (o: Chapter) => {
        call<Chapter>(api => api.chaptersApi.getChapterById,{id:o.id})
            .then((res) => {
                modal.showModal(
                    <SessionView
                        data={res as unknown as SessionDto}
                        isChapter
                        isEdit={false}
                        onSave={() => { }}
                        onDelete={() => { }}
                    />
                );
            });
    };

    const edit = (o: Chapter) => {
        call<Chapter>(api => api.chaptersApi.getChapterById,{id:o.id})
            .then((res) => {
                modal.showModal(
                    <SessionView
                        data={res as unknown as SessionDto}
                        isChapter
                        isEdit
                        onSave={(s) => saveEdit(s, res)}
                        onDelete={() => del(res)}
                    />
                );
            });
    };

    const publishChapter = (o: Chapter) => {
                call<Chapter>(api => api.chaptersApi.updateChapterByIdPublish,{id:o.id})
    };

    const flow = (o: Chapter) => {

        if (!o.flow) {
            o.flow = { nodes: [], edges: [] };
        }

        modal.showModal(<ProgressFlow initialEdges={o.flow.edges}
            readonly={!checkPermission(['rpg_write'])}
            initialNodes={o.flow.nodes}
            onSave={({ nodes, edges }) => saveFlow(o, nodes, edges)} />)
    }

    const saveFlow = (chapter: Chapter, nodes, edges) => {
        chapter.flow = { nodes, edges };
        call(api => api.chaptersApi.updateChapterByIdFlow,{id:chapter.id, body:{ nodes, edges }})
            .then(() => refresh(chapter));
    }

    const saveEdit = (data: SessionDto, chapter: Chapter) => {
        call(api => api.chaptersApi.updateChapterById,{id:chapter.id, body:data})
            .then(() => refresh(chapter));
    };

    const addHero = (chapter: Chapter) => {
        const hero = { chapter: chapter.id } as HeroDto;
        modal.showModal(
            <HeroForm hero={hero} onSave={(o) => saveHero(o, chapter)} isEdit />
        );
    };

    const saveHero = (data: HeroDto, chapter: Chapter) => {
        call(api => api.heroesApi.create,data)
        .then(() => {
            modal.hideModal();
            refresh(chapter);
        });
    };

    const addPlace = (chapter: Chapter) => {
        const place = { chapter: chapter.id } as SessionDto;
        modal.showModal(
            <PlaceForm data={place} onSave={(o) => savePlace(o, chapter)} isNew />
        );
    };

    const savePlace = (data: SessionDto, chapter: Chapter) => {
        call(api => api.placesApi.createPlace,data).then(() => {
            modal.hideModal();
            refresh(chapter);
        });
    };

    const dmPage = (chapter: Chapter) => {
        call<Chapter>(api => api.chaptersApi.getChapterById,{id:chapter.id})
            .then((res) => {
                setData(prev =>
                    prev.map(c => c.id === chapter.id ? res : c)
                );

                modal.showModal(<DMPage chapter={res} />);
            });
    };

    const del = (chapter: Chapter) => {
        modal.showModal(
            <YesNoWindow
                message={t('entity.del_info')}
                yesMethod={() => delConfirm(chapter)}
                open
                onClose={modal.hideModal}
                noMethod={modal.hideModal}
            />
        );
    };

    const delConfirm = (chapter: Chapter) => {
        call<Chapter>(api => api.chaptersApi.deleteChapterById,{id:chapter.id}).then(() => {
            modal.hideModal();
            setData(prev => prev.filter(c => c.id !== chapter.id));
        });
    };

    const startChapter = (c: Chapter) => call<Chapter>(api => api.chaptersApi.updateChapterByIdStart,{id:c.id}).then(() => refresh(c));
    const endChapter = (c: Chapter) => call<Chapter>(api => api.chaptersApi.updateChapterByIdEnd,{id:c.id}).then(() => refresh(c));

    return (
        <Box sx={{ width: "100%", overflowX: "auto" }}>
            <Table size={isMobile ? "small" : "medium"}>
                <TableBody>
                    {data
                        .slice()
                        .sort((a, b) => a.order - b.order)
                        .map((chapter) => {
                            const isOpen = !!openRows[chapter.id];
                            const isLoading = loadingRow === chapter.id;

                            return (
                                <>
                                    <TableRow key={chapter.id}>
                                        <TableCell>
                                            <IconButton onClick={() => toggleRow(chapter)}>
                                                {isLoading
                                                    ? <CircularProgress size={18} />
                                                    : isOpen
                                                        ? <KeyboardArrowUpIcon />
                                                        : <KeyboardArrowDownIcon />
                                                }
                                            </IconButton>
                                        </TableCell>

                                        <TableCell>{chapter.title}</TableCell>
                                        <TableCell>
                                            {chapter.startDate
                                                ? convertToDateStr(chapter.startDate.toLocaleString())
                                                : '--'}
                                        </TableCell>
                                        <TableCell>
                                            {chapter.endDate
                                                ? convertToDateStr(chapter.endDate.toLocaleString())
                                                : '--'}
                                        </TableCell>

                                        <OperationCell operations={operations} data={chapter} />
                                    </TableRow>

                                    <TableRow>
                                        <TableCell colSpan={6} sx={{ p: 0 }}>
                                            <Collapse in={isOpen} timeout="auto" unmountOnExit>
                                                <Box sx={{ p: isMobile ? 1 : 2 }}>
                                                    <Typography variant="body2" sx={{ mb: 1 }}>
                                                        {chapter.description}
                                                    </Typography>

                                                    <Accordion>
                                                        <AccordionSummary expandIcon={<ArrowDownwardIcon />}>
                                                            <Typography>{t('rpg.story.heroes')}</Typography>
                                                        </AccordionSummary>
                                                        <AccordionDetails>
                                                            <HeroTable
                                                                heroes={chapter.heroes}
                                                                refresh={() => refresh(chapter)}
                                                                storyChapters={chapters}
                                                            />
                                                        </AccordionDetails>
                                                    </Accordion>

                                                    <Accordion>
                                                        <AccordionSummary expandIcon={<ArrowDownwardIcon />}>
                                                            <Typography>{t('rpg.story.places')}</Typography>
                                                        </AccordionSummary>
                                                        <AccordionDetails>
                                                            <PlaceTable
                                                                places={chapter.places}
                                                                refresh={() => refresh(chapter)}
                                                                chapters={chapters}
                                                            />
                                                        </AccordionDetails>
                                                    </Accordion>
                                                </Box>
                                            </Collapse>
                                        </TableCell>
                                    </TableRow>
                                </>
                            );
                        })}
                </TableBody>
            </Table>
        </Box>
    );
};