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
    const { heroesApi, chaptersApi, call } = useApiConnect();

    const { t } = useTranslation();
    const [searchParams] = useSearchParams();

    const heroId = searchParams.get('heroId');
    const chapterId = searchParams.get('chapterId');
    const modal = useModal();

    const toDto = (data: Hero): HeroDto => ({
        ...data,
        playerData: data.playerData || '',
        skills: data.skills || []
    } as unknown as HeroDto);

    const saveHero = (data: HeroDto) => {
        call(heroesApi, heroesApi.updateById, { id: data.id, body: data });
    };

    const updateData = async () => {
        if (heroId) {
            call<Hero>(heroesApi, heroesApi.getById, { id: heroId })
                .then(setHero);
        }

        if (chapterId) {
            call<Chapter>(chaptersApi, chaptersApi.getChapterById, { id: chapterId })
                .then(setChapter);
        }
    };

    const HeroToDto = (data?: Hero) => {
        if (!data) return {} as HeroDto;

        const heroDto = data as unknown as HeroDto;
        let i = 1;

        heroDto.skills.forEach(x => {
            x.skillId = x.id;
            x.id = i++;
        });

        return heroDto;
    };

    useEffect(() => {
        updateData();
    }, []);

    return (
        <Grid container spacing={2} padding={2}>
            
            {/* Chapter */}
            {chapter && (
                <Grid size={{xs:12}}>
                    <ChapterSummary chapter={chapter} />
                </Grid>
            )}

            {/* Hero Accordion */}
            {hero && (
                <Grid size={{xs:12}}>
                    <Accordion>
                        <AccordionSummary expandIcon={<ArrowDownwardIcon />}>
                            <Typography>
                                {hero.firstName} {hero.lastName}
                            </Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            <HeroForm hero={toDto(hero)} />
                        </AccordionDetails>
                    </Accordion>
                </Grid>
            )}

            {/* Dice */}
            <Grid size={{xs:12}}>
                <DiceBox />
            </Grid>

            {/* Player Window */}
            <Grid size={{xs:12}}>
                <PlayerWindow 
                    hero={HeroToDto(hero)} 
                    toSave={saveHero} 
                />
            </Grid>

        </Grid>
    );
};

export default PlayerPage;