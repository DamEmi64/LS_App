import { useEffect, useState } from "react";
import { Box, FormControl, InputLabel, MenuItem, Select, Typography } from "@mui/material";
import BattlePage from "./BattlePage";
import { battleNpc } from "../types";
import { useSignalR } from "@/shared/hooks/use-signalR";
import ReactPlayer from "react-player";

const PlayerViewPage = () => {
    const [videoTitle, setVideoTitle] = useState<string>("");
    const [players, setPlayers] = useState<any[]>([]);
    const [background, setBackground] = useState<string>('');
    const [url, setUrl] = useState<string>("");

    const chapterId = new URLSearchParams(window.location.search).get('chapterId') || '';
    const { on } = useSignalR("rpg", undefined, chapterId ? { chapterId } : undefined);

    useEffect(() => {
        on("VideoChanged", (link) => {
            setVideoTitle(link?.title || '');
            setUrl(link?.url || '');
        });

        on("BattleStateChanged", (data: battleNpc[]) => {
            setPlayers(data);
        });

        on("BackgroundChanged", (bg: string) => {
            setBackground(bg);
        });
    }, [on]);

    return (
        <Box>
            <BattlePage players={players} background={background} readonly />
             <Typography variant="h6">{videoTitle && `Playing: ${videoTitle}`}</Typography>
                        {url && (
                            <ReactPlayer
                                width="100%"
                                height={"0%"}
                                src={url}
                                controls
                                loop={true}
                                playing={true}
                            />
                        )}
        </Box>
    );
};

export default PlayerViewPage;
