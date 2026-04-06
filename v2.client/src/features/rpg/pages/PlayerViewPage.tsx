import { useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import BattlePage from "./BattlePage";
import * as signalR from "@microsoft/signalr";
import { battleNpc } from "../types";
import { useSignalR } from "@/shared/hooks/use-signalR";

const PlayerViewPage = () => {
    const [videoTitle, setVideoTitle] = useState<string>("");
    const [players, setPlayers] = useState<any[]>([]);
    const [background, setBackground] = useState<string>('');

    const { on, connected } = useSignalR("rpghub");

    useEffect(() => {
        on("VideoChanged", (title: string) => {
            setVideoTitle(title);
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
            {/* 🔲 TOP BAR */}
            <Box
                sx={{
                    width: "100%",
                    backgroundColor: "black",
                    color: "white",
                    padding: 2,
                    textAlign: "center",
                }}
            >
                <Typography variant="h6">{videoTitle && `Playing: ${videoTitle}`}</Typography>
            </Box>

            {/* ♟️ READONLY BATTLE */}
            <BattlePage players={players} background={background} readonly />
        </Box>
    );
};

export default PlayerViewPage;