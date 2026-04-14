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
import { useModal, useApiConnect, Operations } from '@/shared';
import OperationCell from '@/shared/components/operationCell';
import YesNoWindow from '@/shared/components/YesNoWindow';

import { Chapter, SessionDto, HeroDto, Hero, Story } from '../types';
import HeroForm from '../components/heroForm';
import PlaceForm from "@/features/rpg/components/PlaceForm";

export type ChapterTableProps = {
    chapters: Chapter[]
};

export const ChapterTable: React.FC<ChapterTableProps> = ({ chapters }) => {
    const { t } = useTranslation();
    const modal = useModal();
    const api = useApiConnect();

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

        api.get<Chapter>('rpg_chapter_details', null, chapter.id)
            .then((res) => {
                setLoadingRow(null);

                setData(prev =>
                    prev.map(c => c.id === chapter.id ? res.data : c)
                );

                setOpenRows(prev => ({
                    ...prev,
                    [chapter.id]: true
                }));
            });
    };

    const refresh = (chapter: Chapter) => {
        api.get<Chapter>('rpg_chapter_details', null, chapter.id)
            .then((res) => {
                setData(prev =>
                    prev.map(c => c.id === chapter.id ? res.data : c)
                );
            });
    };

    const operations: Operations<Chapter>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => edit(o) },
        { name: 'rpg.chapter.start', method: (o) => startChapter(o) },
        { name: 'rpg.chapter.end', method: (o) => endChapter(o) },
        { name: "rpg.chapter.dmPage", method: (o) => dmPage(o) },
        { name: 'rpg.hero.add', method: (o) => addHero(o) },
        { name: 'rpg.place.add', method: (o) => addPlace(o) },
        { name: 'opt.delete', method: (o) => del(o) }
    ];

    const details = (o: Chapter) => {
        api.get<Chapter>('rpg_chapter_details', null, o.id)
            .then((res) => {
                modal.showModal(
                    <SessionView
                        data={res.data as unknown as SessionDto}
                        isChapter
                        isEdit={false}
                        onSave={() => {}}
                        onDelete={() => {}}
                    />
                );
            });
    };

    const edit = (o: Chapter) => {
        api.get<Chapter>('rpg_chapter_details', null, o.id)
            .then((res) => {
                modal.showModal(
                    <SessionView
                        data={res.data as unknown as SessionDto}
                        isChapter
                        isEdit
                        onSave={(s) => saveEdit(s, res.data)}
                        onDelete={() => del(res.data)}
                    />
                );
            });
    };

    const saveEdit = (data: SessionDto, chapter: Chapter) => {
        api.put('rpg_chapter_edit', data, null, chapter.id)
            .then(() => refresh(chapter));
    };

    const addHero = (chapter: Chapter) => {
        const hero = { chapter: chapter.id } as HeroDto;
        modal.showModal(
            <HeroForm hero={hero} onSave={(o) => saveHero(o, chapter)} isEdit />
        );
    };

    const saveHero = (data: HeroDto, chapter: Chapter) => {
        api.post('rpg_hero_new', data, null).then(() => {
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
        api.post('rpg_place_new', data, null).then(() => {
            modal.hideModal();
            refresh(chapter);
        });
    };

    const dmPage = (chapter: Chapter) => {
        api.get<Chapter>('rpg_chapter_details', null, chapter.id)
            .then((res) => {
                setData(prev =>
                    prev.map(c => c.id === chapter.id ? res.data : c)
                );

                modal.showModal(<DMPage chapter={res.data} />);
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
        api.del('rpg_chapter_del', null, chapter.id).then(() => {
            modal.hideModal();
            setData(prev => prev.filter(c => c.id !== chapter.id));
        });
    };

    const startChapter = (c: Chapter) => api.put('rpg_chapter_start', c, null, c.id).then(() => refresh(c));
    const endChapter = (c: Chapter) => api.put('rpg_chapter_end', c, null, c.id).then(() => refresh(c));

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