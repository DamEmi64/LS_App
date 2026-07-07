import { useTranslation } from "react-i18next";
import { useState } from "react";

import {
    Button,
    Checkbox,
    FormControl,
    FormControlLabel,
    Grid,
    InputLabel,
    MenuItem,
    Select,
    Switch,
    useMediaQuery,
    useTheme
} from "@mui/material";

import { useSignalR } from "@/shared/hooks/use-signalR";

import PlayerWindow from "@/features/rpg/components/PlayerWindow";
import { ChapterSummary } from "@/features/rpg/components/chapterSummary";
import LinkGen from "@/features/rpg/components/linkGen";
import DiceBox from "@/features/rpg/components/dice/dice";

import BattlePage from "./BattlePage";
import { battleNpc, Chapter, Hero, HeroDto } from "@/features/rpg";
import { ProgressFlow } from "../components/flow/ProgressFlow";
import {call} from "@/shared";

const DMPage: React.FC<{ chapter: Chapter }> = ({ chapter }) => {
    const { t } = useTranslation();
    const { send } = useSignalR("rpg");
    const [combatMode, setCombatMode] = useState(false);

    // 📱 RESPONSIVE
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
    const textColor = theme.palette.text.primary;
    const [hero, setHero] = useState<Hero | null>(null);
    const [withSummary, setWithSummary] = useState(false);
    const [query, setQuery] = useState<{ heroId: any; chapterId?: any }>();
    const [url, setUrl] = useState("");
    const [battleBg, setBattleBg] = useState("");

    const playerHeroes = chapter?.heroes?.filter(x => x.player) || [];

    const HeroToDto = (data: Hero) => {
        const heroDto = data as unknown as HeroDto;
        heroDto.playerData = data.playerData || heroDto.playerData;
        return heroDto;
    };

    const playersToBattleNpcs = (heroes: Hero[]): battleNpc[] =>
        heroes.map((hero, idx) => ({
            id: idx,
            title: `${hero.firstName} ${hero.lastName}`,
            health: "100",
            row: 0,
            column: 0,
            color: "blue"
        }));

    const heroChange = (id: string) => {
        const selected = chapter.heroes.find(x => x.id == id);
        setHero(selected || null);

        if (selected) {
            setQuery(
                withSummary
                    ? { heroId: selected.id, chapterId: chapter.id }
                    : { heroId: selected.id }
            );
        }
    };

    const summaryChange = (checked: boolean) => {
        setWithSummary(checked);

        if (hero) {
            setQuery(
                checked
                    ? { heroId: hero.id, chapterId: chapter.id }
                    : { heroId: hero.id }
            );
        }
    };

    const startChapter = () => call<Chapter>(api =>api.chaptersApi.updateByIdStart,{id:chapter.id});
    const endChapter = () => call<Chapter>(api => api.chaptersApi.updateByIdEnd,{id:chapter.id});

    const openPlayerView = () => {
        window.open('/rpg/playerView', '_blank');
        send('ChangeVideo', url || '');
        send('UpdateBattleState', playersToBattleNpcs(playerHeroes));
        send("BackgroundChanged", battleBg);
    };

    const onBackgroundChange = (bg: string) => {
        setBattleBg(bg);
        send("ChangeBackground", bg);
        send('UpdateBattleState', playersToBattleNpcs(playerHeroes));
    };

    const handleVideoChange = (value: string) => {
        setUrl(value);
        const selected = chapter.links?.find(l => l.url === value);
        send("ChangeVideo", selected || {});
    };

    return (
        <Grid
            container
            spacing={isMobile ? 2 : 3}
            sx={{ p: isMobile ? 1 : 2 }}
        >

            {/* ACTION BUTTONS */}
            <Grid size={{ xs: 12 }} sx={{ display: "flex", gap: 1, flexWrap: "wrap" }}>
                <Button variant="contained" onClick={startChapter}>
                    {t('rpg.chapter.start')}
                </Button>
                <Button variant="contained" onClick={endChapter}>
                    {t('rpg.chapter.end')}
                </Button>
                <Button variant="contained" onClick={openPlayerView}>
                    {t('rpg.other.output_player_view')}
                </Button>
            </Grid>

            <Grid size={{ xs: 12 }}>
                {chapter && <ChapterSummary chapter={chapter} />}
            </Grid>
            <Grid size={{xs: 12}}>
                <FormControlLabel
                    color={textColor}
                    label={t('rpg.story.battleMode')}
                    control={
                        <Switch
                            checked={combatMode}
                            onChange={(e) => setCombatMode(e.target.checked)}
                        />
                    }
                />
            </Grid>
            <Grid size={{ xs: 12, md: 6 }}>
                <FormControl fullWidth size={isMobile ? "small" : "medium"}>
                    <InputLabel>{t('rpg.story.hero')}</InputLabel>
                    <Select
                        value={hero?.id || ""}
                        label={t('rpg.story.hero')}
                        onChange={(e) => heroChange(e.target.value as string)}
                    >
                        <MenuItem value="">---</MenuItem>
                        {playerHeroes.map(h => (
                            <MenuItem key={h.id} value={h.id}>
                                {h.firstName} {h.lastName}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>

                <FormControlLabel
                    label={t('rpg.story.gen_summary')}
                    color={textColor}
                    control={
                        <Switch
                            checked={withSummary}
                            onChange={(e) => summaryChange(e.target.checked)}
                        />
                    }
                />

                {hero && (
                    <LinkGen
                        queryParams={query}
                        endpoint={`${window.location.protocol}//${window.location.host}/rpg/playerData`}
                    />
                )}
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
                <DiceBox />
            </Grid>

            <Grid size={{ xs: 12 }}>
                {chapter?.links?.length > 0 && (
                    <>
                        <FormControl fullWidth>
                            <InputLabel>{t('rpg.chapter.links')}</InputLabel>
                            <Select value={url} onChange={(e) => handleVideoChange(e.target.value as string)}>
                                <MenuItem value="">---</MenuItem>
                                {chapter.links.map(link => (
                                    <MenuItem key={link.url} value={link.url}>
                                        {link.title}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </>
                )}
            </Grid>
            
            <Grid size={{ xs: 12 }}>
                <Select
                    fullWidth
                    value={battleBg}
                    onChange={(e) => onBackgroundChange(e.target.value as string)}
                    displayEmpty
                    size={isMobile ? "small" : "medium"}
                >
                    <MenuItem value="">Default</MenuItem>
                    <MenuItem value="/maps/forest.png">Forest</MenuItem>
                    <MenuItem value="/maps/beach.png">Beach</MenuItem>
                    <MenuItem value="/maps/city.png">City</MenuItem>
                </Select>
                {combatMode && (<BattlePage
                    players={playersToBattleNpcs(playerHeroes)}
                    onChange={(data) => send("UpdateBattleState", data)}
                    background={battleBg}
                />)}
                {!combatMode && (<ProgressFlow initialEdges={chapter.flow?.edges || []} initialNodes={chapter.flow?.nodes || []} onSave={ o => call(api => api.chaptersApi.updateByIdFlow,{id: chapter.id, flowDto:o})}/>)}
                
            </Grid>

            {/* PLAYER WINDOW */}
            <Grid size={{ xs: 12 }}>
                {hero && <PlayerWindow hero={HeroToDto(hero)} />}
            </Grid>

        </Grid>
    );
};

export default DMPage;
