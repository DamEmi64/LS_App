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
    Button,
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
import { useEffect } from "react";
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
import EndMeetingDialog from '../components/EndMeetingDialog';
import StartMeetingDialog from '../components/StartMeetingDialog';
import { getMeetingRecordingState, isMeetingRecordingAvailable, openMeetingRecording, startMeetingRecording, stopMeetingRecording, transcribeMeetingRecording } from '../services/meetingRecorder';
import { generateMeetingSummary } from '../services/meetingSummary';
import { appStorage } from '@/shared/storage/appStorage';
import FiberManualRecordIcon from '@mui/icons-material/FiberManualRecord';

export type ChapterTableProps = {
    chapters: Chapter[]
};

const meetingTranscriptStorageKey = (chapterId: string, recordingPath: string) =>
    `rpgMeetingTranscript:${chapterId}:${recordingPath}`;

export const ChapterTable: React.FC<ChapterTableProps> = ({ chapters }) => {
    const { t } = useTranslation();
    const modal = useModal();
    const { checkPermission } = useAuth();

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));


    const [openRows, setOpenRows] = useState<Record<string, boolean>>({});
    const [loadingRow, setLoadingRow] = useState<string | null>(null);
    const [isEndMeetingDialogOpen, setIsEndMeetingDialogOpen] = useState(false);
    const [startTarget, setStartTarget] = useState<Chapter | null>(null);
    const [endTarget, setEndTarget] = useState<Chapter | null>(null);
    const [recordingChapterId, setRecordingChapterId] = useState<string | null>(null);
    const [savedRecordingChapterId, setSavedRecordingChapterId] = useState<string | null>(null);
    const [savedRecordingPath, setSavedRecordingPath] = useState<string | null>(null);
    const [generatingSummary, setGeneratingSummary] = useState(false);
    const [meetingTranscript, setMeetingTranscript] = useState('');
    const [transcriptionError, setTranscriptionError] = useState('');
    const [generatedSummary, setGeneratedSummary] = useState('');
    const [transcriptionPhase, setTranscriptionPhase] = useState<'download' | 'transcribing'>('download');
    const [transcriptionProgress, setTranscriptionProgress] = useState(0);

    const [data, setData] = useState<Chapter[]>(chapters);

    useEffect(() => {
        getMeetingRecordingState().then(state => {
            if (state.recording && state.meetingId) setRecordingChapterId(state.meetingId);
        }).catch(() => undefined);

        const saved = data.find(chapter => appStorage.get(`rpgMeetingRecording:${chapter.id}`));
        if (saved) {
            setSavedRecordingChapterId(saved.id);
            setSavedRecordingPath(appStorage.get(`rpgMeetingRecording:${saved.id}`));
        }
    }, []);

    const toggleRow = (chapter: Chapter) => {
        const isOpen = !!openRows[chapter.id];

        if (isOpen) {
            setOpenRows(prev => ({ ...prev, [chapter.id]: false }));
            return;
        }

        setLoadingRow(chapter.id);

        call<Chapter>(api => api.chaptersApi.getById, { id: chapter.id })
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
        call<Chapter>(api => api.chaptersApi.getById, { id: chapter.id })
            .then((res) => {
                setData(prev =>
                    prev.map(c => c.id === chapter.id ? res : c)
                );
            });
    };

    const operations: Operations<Chapter>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => edit(o), hidden: (o) => !checkPermission(['rpg-write']) },
        { name: 'opt.publish', method: (o) => publishChapter(o), hidden: (o) => !checkPermission(['rpg-write']) || !o.draft },
        { name: 'rpg.chapter.start', method: (o) => setStartTarget(o) },
        { name: 'rpg.chapter.end', method: (o) => openEndMeeting(o) },
        { name: "rpg.chapter.dmPage", method: (o) => dmPage(o) },
        { name: 'rpg.flow.flow_title', method: (o) => flow(o) },
        { name: 'rpg.hero.add', method: (o) => addHero(o), hidden: (o) => !checkPermission(['rpg-write']) },
        { name: 'rpg.place.add', method: (o) => addPlace(o), hidden: (o) => !checkPermission(['rpg-write']) },
        { name: 'opt.delete', method: (o) => del(o), hidden: (o) => !checkPermission(['rpg-write']) },
    ];

    const details = (o: Chapter) => {
        call<Chapter>(api => api.chaptersApi.getById, { id: o.id })
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
        call<Chapter>(api => api.chaptersApi.getById, { id: o.id })
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
        call<Chapter>(api => api.chaptersApi.updateByIdPublish, { id: o.id })
    };

    const flow = (o: Chapter) => {

        if (!o.flow) {
            o.flow = { nodes: [], edges: [] };
        }

        modal.showModal(<ProgressFlow initialEdges={o.flow.edges}
            readonly={!checkPermission(['rpg-write'])}
            initialNodes={o.flow.nodes}
            onSave={({ nodes, edges }) => saveFlow(o, nodes, edges)} />)
    }

    const saveFlow = (chapter: Chapter, nodes, edges) => {
        chapter.flow = { nodes, edges };
        call(api => api.chaptersApi.updateByIdFlow, { id: chapter.id, flowDto: { nodes, edges } })
            .then(() => refresh(chapter));
    }

    const saveEdit = (data: SessionDto, chapter: Chapter) => {
        call(api => api.chaptersApi.updateById, { id: chapter.id, chapterDto: data })
            .then(() => refresh(chapter));
    };

    const addHero = (chapter: Chapter) => {
        const hero = { chapter: chapter.id } as HeroDto;
        modal.showModal(
            <HeroForm hero={hero} onSave={(o) => saveHero(o, chapter)} isEdit />
        );
    };

    const saveHero = (data: HeroDto, chapter: Chapter) => {
        call(api => api.heroesApi.create, { heroDto: data })
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
        call(api => api.placesApi.create, { placeDto: data }).then(() => {
            modal.hideModal();
            refresh(chapter);
        });
    };

    const dmPage = (chapter: Chapter) => {
        call<Chapter>(api => api.chaptersApi.getById, { id: chapter.id })
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
        call<Chapter>(api => api.chaptersApi.deleteById, { id: chapter.id }).then(() => {
            modal.hideModal();
            setData(prev => prev.filter(c => c.id !== chapter.id));
        });
    };

    const startChapter = async (c: Chapter, record: boolean) => {
        if (record && !isMeetingRecordingAvailable()) {
            throw new Error('Device recording is available in the Android app only.');
        }
        await call<Chapter>(api => api.chaptersApi.updateByIdStart, { id: c.id });
        if (record) {
            await startMeetingRecording(c.id);
            const previousRecordingPath = appStorage.get(`rpgMeetingRecording:${c.id}`);
            if (previousRecordingPath) appStorage.remove(meetingTranscriptStorageKey(c.id, previousRecordingPath));
            appStorage.remove(`rpgMeetingTranscript:${c.id}`);
            appStorage.remove(`rpgMeetingRecording:${c.id}`);
            await appStorage.flush();
            setRecordingChapterId(c.id);
            setSavedRecordingChapterId(null);
            setSavedRecordingPath(null);
        }
        refresh(c);
    };

    const saveStoppedRecording = async (chapter: Chapter) => {
        const state = await getMeetingRecordingState();
        if (!state.recording || state.meetingId !== chapter.id) return;
        const recording = await stopMeetingRecording();
        appStorage.set(`rpgMeetingRecording:${chapter.id}`, recording.path);
        setRecordingChapterId(null);
        setSavedRecordingChapterId(chapter.id);
        setSavedRecordingPath(recording.path);
    };

    const stopRecording = async () => {
        if (!recordingChapterId) return;
        const chapter = data.find(item => item.id === recordingChapterId);
        if (!chapter) return;
        await saveStoppedRecording(chapter);
    };

    const openEndMeeting = async (chapter: Chapter) => {
        try {
            setGeneratingSummary(true);
            setTranscriptionError('');
            setMeetingTranscript('');
            setGeneratedSummary('');
            setTranscriptionPhase('download');
            setTranscriptionProgress(0);
            setEndTarget(chapter);
            setIsEndMeetingDialogOpen(true);
            await saveStoppedRecording(chapter);
            const savedPath = appStorage.get(`rpgMeetingRecording:${chapter.id}`);
            if (savedPath) {
                setSavedRecordingChapterId(chapter.id);
                setSavedRecordingPath(savedPath);
                const transcriptKey = meetingTranscriptStorageKey(chapter.id, savedPath);
                const cachedTranscript = appStorage.get(transcriptKey) ?? '';
                if (cachedTranscript) {
                    setMeetingTranscript(cachedTranscript);
                    setGeneratedSummary(generateMeetingSummary(cachedTranscript));
                } else {
                    const result = await transcribeMeetingRecording(savedPath, progress => {
                        setTranscriptionPhase(progress.phase);
                        setTranscriptionProgress(progress.progress);
                    });
                    setMeetingTranscript(result.transcript);
                    appStorage.set(transcriptKey, result.transcript);
                    await appStorage.flush();
                    setGeneratedSummary(generateMeetingSummary(result.transcript));
                }
            } else {
                setEndTarget(chapter);
            }
        } catch (error) {
            console.error('Unable to stop the local meeting recording.', error);
            setEndTarget(chapter);
            setIsEndMeetingDialogOpen(true);
            setTranscriptionError(error instanceof Error ? error.message : String(error));
        } finally {
            setGeneratingSummary(false);
        }
    };

    const endChapter = async (c: Chapter, summary: string) => {
        await call<Chapter>(api => api.chaptersApi.updateByIdEnd, { id: c.id, body: summary });
        setIsEndMeetingDialogOpen(false);
        setEndTarget(null);
        refresh(c);
    };

    return (
        <Box sx={{ width: "100%", overflowX: "auto" }}>
            {recordingChapterId && (
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, p: 1.5, mb: 1, borderRadius: 1, bgcolor: 'error.light', color: 'error.contrastText' }}>
                    <FiberManualRecordIcon fontSize="small" />
                    <Typography sx={{ flex: 1 }}>
                        {t('rpg.meeting.recordingNow', 'Recording on this device')}
                        {' — '}{data.find(item => item.id === recordingChapterId)?.title ?? ''}
                    </Typography>
                    <Button color="inherit" variant="outlined" onClick={stopRecording}>
                        {t('rpg.meeting.stopRecording', 'Stop recording')}
                    </Button>
                </Box>
            )}
            {savedRecordingChapterId && !recordingChapterId && (
                <Typography variant="body2" sx={{ mb: 1, color: 'success.main' }}>
                    {t('rpg.meeting.recordingSaved', 'Recording saved on this device')}
                </Typography>
            )}
            {savedRecordingPath && (
                <Button size="small" onClick={() => openMeetingRecording(savedRecordingPath)} sx={{ mb: 1 }}>
                    {t('rpg.meeting.playRecording', 'Play saved recording')}
                </Button>
            )}
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

                                    <TableRow key={chapter.id + '_1'}>
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
            <StartMeetingDialog
                open={!!startTarget}
                onClose={() => setStartTarget(null)}
                onConfirm={(record) => startTarget ? startChapter(startTarget, record) : Promise.resolve()}
            />
            <EndMeetingDialog
                open={isEndMeetingDialogOpen && !!endTarget}
                onClose={() => { setIsEndMeetingDialogOpen(false); setEndTarget(null); }}
                onConfirm={(summary) => endTarget ? endChapter(endTarget, summary) : Promise.resolve()}
                initialSummary={generatedSummary || (endTarget?.sessions?.[Math.max(0, (endTarget.sessions?.length ?? 0) - 1)]?.summary ?? '')}
                transcript={meetingTranscript}
                transcriptionError={transcriptionError}
                generatingSummary={generatingSummary}
                generationStatus={transcriptionPhase === 'download'
                    ? `${t('rpg.meeting.downloadingModel', 'Downloading offline speech model')} (${transcriptionProgress}%)`
                    : `${t('rpg.meeting.transcribingRecording', 'Transcribing on this device')} (${transcriptionProgress}%)`}
                downloadingModel={generatingSummary && transcriptionPhase === 'download'}
            />
        </Box>
    );
};
