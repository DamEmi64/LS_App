import { useTranslation } from "react-i18next";
import { useState } from "react";
import { useApiConnect } from "@/shared/context/apiConnect";
import PlayerWindow from "@/features/rpg/components/PlayerWindow";
import { Button, FormControl, FormControlLabel, Grid, InputLabel, MenuItem, Select, Switch, useTheme } from "@mui/material";
import { ChapterSummary } from "@/features/rpg/components/chapterSummary";
import LinkGen from "@/features/rpg/components/linkGen";
import DiceBox from "@/features/rpg/components/dice/dice";
import ReactPlayer from 'react-player';
import { Chapter, Hero, HeroDto } from "@/features/rpg";

const DMPage: React.FC<{ chapter: Chapter }> = ({ chapter }) => {
    const api = useApiConnect();

    const { t } = useTranslation();
    const theme = useTheme();
    const textColor = theme.palette.mode === 'dark' ? theme.palette.text.primary : theme.palette.text.secondary;
    const [hero, setHero] = useState<Hero | null>(null);
    const [withSummary, setWithSummary] = useState<boolean>(false);
    const [query, setQuery] = useState<{ heroId, chapterId?}>();
    const [url, setUrl] = useState<string>('');

    const openBattleHelper = () => {
        var url = `${window.location.protocol}//${window.location.host}/rpg/battle`;
        window.open(url,'_blank');
    }

    const playerHeroes = chapter.heroes.filter(x => x.player);

    const HeroToDto = (data: Hero) => {
        var heroDto = data as unknown as HeroDto;

        if (heroDto.playerData) {
            heroDto.playerData = data.playerData;
        }

        return heroDto;
    }



    const startChapter = () => {
        api.put<Chapter>('rpg_chapter_start', null, null, chapter.id);
    }

    const endChapter = () => {
        api.put<Chapter>('rpg_chapter_end', null, null, chapter.id);
    }

    const summaryChange = (check: boolean) => {
        setWithSummary(check);
        if (hero) {
            if (check) {
                setQuery({ heroId: hero.id, chapterId: chapter.id });
            }
            else {
                setQuery({ heroId: hero.id });
            }
        }
    }

    const heroChange = (e: string) => {
        var hero = chapter.heroes.find(x => x.id == e);
        setHero(hero);

        if (hero) {
            if (withSummary) {
                setQuery({ heroId: hero.id, chapterId: chapter.id });
            }
            else {
                setQuery({ heroId: hero.id });
            }
        }
    }

    return (
        <>
            <Grid container direction={'row'} spacing={10} rowSpacing={2}>
                <Grid size={{ xs: 12 }}>
                    <Button
                        type="button"
                        variant="contained"
                        color="primary"
                        onClick={startChapter}>
                        {t('rpg.chapter.start')}
                    </Button>
                    <Button
                        type="button"
                        variant="contained"
                        color="primary"
                        onClick={endChapter}>
                        {t('rpg.chapter.end')}
                    </Button>
                    <Button
                        type="button"
                        variant="contained"
                        color="primary"
                        onClick={openBattleHelper}>
                            {t('rpg.other.battleLink')}
                    </Button>
                </Grid>
                <Grid size={{ xs: 12 }}>
                    {chapter && (
                        <ChapterSummary chapter={chapter}></ChapterSummary>
                    )}
                </Grid>
                <Grid size={{ xs: 12, md: 6 }} direction={'column'}>
                    <Grid>
                        <FormControl fullWidth>
                            <InputLabel id="demo-simple-select-label">{t('rpg.story.hero')}</InputLabel>
                            <Select
                                labelId="demo-simple-select-label"
                                id="demo-simple-select"
                                label={t('rpg.story.hero')}
                                onChange={(e) => heroChange(e.target.value as string)}
                            >
                                <MenuItem value="">---</MenuItem>
                                {playerHeroes && playerHeroes.map((hero) => (
                                    <MenuItem value={hero.id}>
                                        {hero.firstName} {hero.lastName}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                        <FormControlLabel
                            style={{ color: textColor }}
                            label={t('rpg.story.gen_summary')}
                            control={<Switch
                                checked={withSummary}
                                onChange={e => summaryChange(e.target.checked)}
                                style={{ color: textColor }}
                            />}
                        />
                    </Grid>
                    {hero && (
                        <LinkGen queryParams={query} endpoint={`${window.location.protocol}//${window.location.host}/rpg/playerData`} ></LinkGen>
                    )}
                </Grid>
                <Grid size={{ xs: 12, md: 6 }} direction={'column'}>
                    <DiceBox></DiceBox>
                </Grid>
            </Grid>
            <Grid>
                {
                    chapter.links && (<>
                        <FormControl fullWidth>
                            <InputLabel id="demo-simple-select-label">{t('rpg.chapter.links')}</InputLabel>
                            <Select
                                value={url}
                                onChange={(e) => setUrl(e.target.value)}
                            >
                                <MenuItem value="">---</MenuItem>
                                {chapter.links.map(link =>
                                    <MenuItem value={link.url}>{link.title}</MenuItem>
                                )}
                            </Select>
                        </FormControl>
                        {url && (<ReactPlayer width={'100%'} height={innerWidth / 3} src={url} controls={true} />)}
                    </>)
                }
            </Grid>
            <Grid>
                {hero && (
                    <PlayerWindow hero={HeroToDto(hero)}></PlayerWindow>
                )
                }

            </Grid>
        </>
    )
};

export default DMPage;