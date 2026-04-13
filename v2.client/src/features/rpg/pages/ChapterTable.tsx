import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import PlaceForm from "@/features/rpg/components/PlaceForm";
import { Accordion, AccordionDetails, AccordionSummary, Box, CircularProgress, Collapse, IconButton, Table, TableBody, TableCell, TableRow, Typography } from "@mui/material";
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

export type ChapterTableProps = {
    chapters: Chapter[]
}

export const ChapterTable: React.FC<ChapterTableProps> = ({ chapters }) => {
    const { t } = useTranslation();
    const modal = useModal();
    const api = useApiConnect();
    const [open, setOpen] = useState<boolean>();
    const [activeRow, setActiveRow] = useState<string>();
    const [loading, setLoading] = useState<boolean>(false);
    const [data, setData] = useState<Chapter[]>(chapters);

    const refresh = (chapter: Chapter) => {
        api.get<Chapter>('rpg_chapter_details', null, chapter.id).then((res) => {
            setLoading(false);
            setOpen(!open);
            var newData = data.map(c => c.id === chapter.id ? res.data : c);
            setData(newData);
        });
    }

    const refreshHeores = (chapter: Chapter) => {
        api.get<Chapter>('rpg_chapter_details', null, chapter.id).then((res) => {
            setLoading(false);
            setOpen(!open);
            var newData = data.map(c => c.id === chapter.id ? res.data : c);
            setData(newData);
        });
    }

    const handleRowClick = (chapter: Chapter) => {
        setActiveRow(chapter.id);
        if (open) {
            setOpen(!open);
            return;
        }

        setLoading(true);
        api.get<Chapter>('rpg_chapter_details', null, chapter.id).then((res) => {
            setLoading(false);
            setOpen(!open);
            var newData = data.map(c => c.id === chapter.id ? res.data : c);
            setData(newData);
        });
    };

    const dmPage = (chapter: Chapter) => {
        setLoading(true);
        api.get<Chapter>('rpg_chapter_details', null, chapter.id).then((res) => {
            setLoading(false);
            var newData = data.map(c => c.id === chapter.id ? res.data : c);
            setData(newData);
            chapter = res.data;
            modal.showModal(loading ? <CircularProgress /> : <DMPage chapter={chapter}></DMPage>)
        });
    }

    const operations: Operations<Chapter>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => edit(o) },
        { name: 'rpg.chapter.start', method: (o) => startChapter(o) },
        { name: 'rpg.chapter.end', method: (o) => endChapter(o) },
        { name: "rpg.chapter.dmPage", method: (o) => dmPage(o) },
        { name: 'rpg.hero.add', method: (o) => addHero(o) },
        { name: 'rpg.place.add', method: (o) => addPlace(o) },
        { name: 'opt.delete', method: (o) => del(o) }
    ]

    const show = (c: Chapter, isEdit: boolean) => {
       return <SessionView data={c as unknown as SessionDto} isChapter={true} isEdit={isEdit} onSave={(s) => saveEdit(s,c)} onDelete={(s) => del(c)} />
    }

    const details = (o: Chapter) => {
        setLoading(true);
        api.get<Chapter>('rpg_chapter_details', null, o.id).then((res) => {
            setLoading(false);
            o = res.data;
            modal.showModal(loading ? <CircularProgress /> : show(o, false));
        });
    }

    const addHero = (data: Chapter) => {
        var hero = {} as HeroDto;
        hero.chapter = data.id;
        modal.showModal(<HeroForm hero={hero} onSave={(o) => saveHero(o, data)} isEdit={true} />)
    }

    const saveHero = (data: HeroDto, chapter: Chapter) => {
        api.post<HeroDto>('rpg_hero_new', data, null)
            .then(() => {
                modal.hideModal();
                refresh(chapter);
            });
    }

    const addPlace = (data: Chapter) => {
        var place = {} as SessionDto;
        place.chapter = data.id;
        modal.showModal(<PlaceForm data={place} onSave={(o) => savePlace(o, data)} isNew={true}/>)
    }

    const savePlace = (data: SessionDto, chapter: Chapter) => {
        api.post<SessionDto>('rpg_place_new', data, null)
            .then(() => {
                modal.hideModal();
                refresh(chapter);
            });
    }

    const edit = (o: Chapter) => {
        setLoading(true);
        api.get<Chapter>('rpg_chapter_details', null, o.id).then((res) => {
            setLoading(false);
            o = res.data;
            modal.showModal(loading ? <CircularProgress /> : show(o, true));
        });
     }

    const saveEdit = (data: SessionDto, chapter: Chapter) => {
        api.put<SessionDto>('rpg_chapter_edit', data, null, chapter.id)
            .then(() => {
                modal.hideModal();
                refresh(chapter);
            });
    }

    const del = (data: Chapter) => {
        modal.showModal(<YesNoWindow message={t('entity.del_info')} yesMethod={() => delConfirm(data)} open={true} onClose={modal.hideModal} noMethod={modal.hideModal} />);
    }

    const delConfirm = (data: Chapter) => {
        api.del<Hero>('rpg_chapter_del', null, data.id)
            .then(() => {
                modal.hideModal();
                refresh(data);
            });
    }

    const startChapter = (data: any) => {
        api.put<Story>('rpg_chapter_start', data, null, data.id)
            .then(() => {
                refresh(data);
            });
    }

    const endChapter = (data: any) => {
        api.put<Story>('rpg_chapter_end', data, null, data.id)
            .then(() => {
                refresh(data);
            });
    }

    return <>
        <Table>
            <TableRow>
                <TableCell />
                <TableCell
                >
                    {t('rpg.other.title')}
                </TableCell>
                <TableCell
                >
                    {t('rpg.chapter.startDate')}
                </TableCell>
                <TableCell
                >
                    {t('rpg.chapter.endDate')}
                </TableCell>
            </TableRow>
            <TableBody>
                {data && (data.sort((a, b) => a.order - b.order).map((chapter: Chapter) => (
                    <>
                        <TableRow key={chapter.id}>
                            <TableCell>
                                <IconButton
                                    aria-label="expand row"
                                    size="small"
                                    onClick={() => handleRowClick(chapter)}
                                >
                                    {loading ? <CircularProgress /> : (open && activeRow === chapter.id) ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                                </IconButton>
                            </TableCell>
                            <TableCell>
                                {String(chapter.title)}
                            </TableCell>
                            <TableCell>
                                {chapter.startDate ? convertToDateStr(chapter.startDate.toLocaleString()) : '--'}
                            </TableCell>
                            <TableCell>
                                {chapter.endDate ? convertToDateStr(chapter.endDate.toLocaleString()) : '--'}
                            </TableCell>
                            <OperationCell operations={operations} data={chapter} />
                        </TableRow>
                        <TableRow>
                            <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
                                <Collapse in={(open && activeRow === chapter.id)} timeout="auto" unmountOnExit>
                                    <Box sx={{ margin: 1 }}>
                                        <Typography
                                            variant="body1"
                                            gutterBottom
                                            component="label"
                                        >{chapter.description}</Typography>
                                        <Accordion>
                                            <AccordionSummary
                                                expandIcon={<ArrowDownwardIcon />}
                                                aria-controls="panel1-content"
                                                id="panel1-header"
                                            >
                                                <Typography component="span">{t('rpg.story.heroes')}</Typography>
                                            </AccordionSummary>
                                            <AccordionDetails>
                                                <HeroTable heroes={chapter.heroes} refresh={() => refresh(chapter)} storyChapters={chapters} />
                                            </AccordionDetails>
                                        </Accordion>
                                        <Accordion>
                                            <AccordionSummary
                                                expandIcon={<ArrowDownwardIcon />}
                                                aria-controls="panel1-content"
                                                id="panel1-header"
                                            >
                                                <Typography component="span">{t('rpg.story.places')}</Typography>
                                            </AccordionSummary>
                                            <AccordionDetails>
                                                <PlaceTable places={chapter.places} refresh={() => refresh(chapter)} chapters={chapters} />
                                            </AccordionDetails>
                                        </Accordion>
                                    </Box>
                                </Collapse>
                            </TableCell>
                        </TableRow>
                    </>
                )))}
            </TableBody>
        </Table>
    </>;
}