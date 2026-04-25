import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import { useTranslation } from "react-i18next";
import { useEffect, useState } from "react";
import { useApiConnect } from "@/shared/context/apiConnect";
import { SessionTable } from "./SessionTable";
import { Chapter, Hero, HeroDto, Story } from "@/features/rpg";
import PlayerWindow from "@/features/rpg/components/PlayerWindow";
import { useParams, useSearchParams } from "react-router-dom";
import { Accordion, AccordionDetails, AccordionSummary, Button, Grid, Typography } from "@mui/material";
import { useModal } from "@/shared/context/modal";
import Dice from 'react-dice-roll';
import { ChapterSummary } from "@/features/rpg/components/chapterSummary";
import DiceBox from "@/features/rpg/components/dice/dice";
import { onChangeParams } from '@/shared';
import HeroForm from '../components/heroForm';

const PlayerPage = () => {
    const [hero, setHero] = useState<Hero>();
    const [chapter, setChapter] = useState<Chapter>();
    const {heroesApi, chaptersApi, call} = useApiConnect();

    const { t } = useTranslation();
    const [searchParams] = useSearchParams();

    const heroId = searchParams.get('heroId');
    const chapterId = searchParams.get('chapterId');
    const modal = useModal();

    const toDto = (data: Hero): HeroDto => {
        return {...data, playerData: data.playerData || '', skills: data.skills || []} as unknown as HeroDto;
    }

    const saveHero = (data: HeroDto) => {
        call(heroesApi,heroesApi.updateHeroeById, {id:data.id, body:data})
    }

    const updateData = async (paramsObj: onChangeParams) => {
        call(heroesApi,heroesApi.getHeroeById,{id:heroId})
        if (chapterId) {
            call<Chapter>(chaptersApi,chaptersApi.getChapterById,{id:chapterId}).then(data => setChapter(data));
        }
    }

    const HeroToDto = (data: Hero) => {
        let heroDto = {} as HeroDto;
        if (data) {
            heroDto = data as unknown as HeroDto;
            let i = 1;
            heroDto.skills.forEach(x => { x.skillId = x.id;  x.id = i; i++; });

        }
        else {
            heroDto = {} as HeroDto;
        }

        return heroDto;
    }

    // Always use updateData for initial load
    useEffect(() => {
        updateData({ page: 0, pageSize: 10, orderBy: '', order: 'asc', filters: [] });
    }, []);

    return (
        <>
            <Grid container direction={'row'}>
                <Grid style={{ width: '100vw' }}>
                    {chapter && (
                        <ChapterSummary chapter={chapter}></ChapterSummary>
                    )}
                </Grid>
                <Grid>
                    {hero && (
                        <Accordion style={{ flexGrow: 1, width: '100vw' }}>
                            <AccordionSummary
                                expandIcon={<ArrowDownwardIcon />}
                                aria-controls="panel1-content"
                                id="panel1-header"
                            >
                                <Typography component="span">{hero.firstName} {hero.lastName}</Typography>
                            </AccordionSummary>
                            <AccordionDetails>
                                <HeroForm hero={toDto(hero)}></HeroForm>
                            </AccordionDetails>
                        </Accordion>
                    )}
                </Grid>
                <Grid>
                    <DiceBox></DiceBox>
                </Grid>
                <Grid>
                    <PlayerWindow hero={HeroToDto(hero)} toSave={saveHero}></PlayerWindow>
                </Grid>
            </Grid>

        </>
    )
};

export default PlayerPage;