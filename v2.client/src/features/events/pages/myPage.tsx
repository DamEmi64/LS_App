import React, { useEffect, useMemo, useState } from "react";

import {
    Box,
    CircularProgress,
    Grid,
    Paper,
    Typography,
    useMediaQuery,
    useTheme
} from "@mui/material";
import { EventCalendar } from "@mui/x-scheduler/event-calendar";
import { SchedulerEvent } from "@mui/x-scheduler/models";
import dayjs from "dayjs";
import { useTranslation } from "react-i18next";

import { EventDto } from "@/shared/api/generated";
import { ResponseList } from "@/shared/api/extension";
import { call } from "@/shared";

const toSchedulerEvent = (event: EventDto, index: number): SchedulerEvent | null => {
    if (!event.eventDate) return null;

    const start = dayjs(event.eventDate);

    if (!start.isValid()) return null;

    return {
        id: event.id || `${event.title}-${event.eventDate}-${index}`,
        title: event.title || "",
        description: event.description || undefined,
        start: start.toISOString(),
        end: start.add(1, "hour").toISOString(),
        readOnly: true,
        color: "teal"
    };
};

const MyEventsPage: React.FC = () => {
    const { t } = useTranslation();
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const [events, setEvents] = useState<EventDto[]>([]);
    const [loading, setLoading] = useState(false);

    const schedulerEvents = useMemo(
        () => events
            .map(toSchedulerEvent)
            .filter((event): event is SchedulerEvent => event !== null),
        [events]
    );

    useEffect(() => {
        const loadEvents = async () => {
            setLoading(true);

            try {
                const result = await call<ResponseList<EventDto>>(
                    api => api.eventClient.getMe,
                    { page: 0, pageSize: 500 }
                );

                setEvents(result.data || []);
            } finally {
                setLoading(false);
            }
        };

        loadEvents();
    }, []);

    return (
        <Grid container sx={{ width: "100%", p: isMobile ? 1 : 3 }}>
            <Grid
                size={{ xs: 12 }}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                    mb: 1,
                    textAlign: "center"
                }}
            >
                <Typography
                    component="h1"
                    sx={{
                        color: "white",
                        fontSize: isMobile ? "1.8rem" : "2.5rem",
                        fontWeight: "bold"
                    }}
                >
                    {t("events.mySiteTitle")}
                </Typography>
            </Grid>

            <Grid size={{ xs: 12 }}>
                <Paper
                    sx={{
                        borderRadius: 1,
                        height: isMobile ? "calc(100vh - 150px)" : "calc(100vh - 170px)",
                        minHeight: 520,
                        overflow: "hidden",
                        position: "relative"
                    }}
                >
                    {loading && (
                        <Box
                            sx={{
                                alignItems: "center",
                                bgcolor: "background.paper",
                                display: "flex",
                                inset: 0,
                                justifyContent: "center",
                                opacity: 0.8,
                                position: "absolute",
                                zIndex: 2
                            }}
                        >
                            <CircularProgress />
                        </Box>
                    )}

                    <EventCalendar
                        events={schedulerEvents}
                        defaultView="month"
                        eventCreation={false}
                        areEventsDraggable={false}
                        areEventsResizable={false}
                        readOnly
                        defaultPreferences={{
                            isSidePanelOpen: !isMobile,
                            showEmptyDaysInAgenda: false,
                            showWeekends: true
                        }}
                        sx={{
                            height: "100%",
                            width: "100%"
                        }}
                    />

                    {!loading && schedulerEvents.length === 0 && (
                        <Box
                            sx={{
                                bottom: 24,
                                left: 24,
                                position: "absolute",
                                right: 24,
                                zIndex: 1
                            }}
                        >
                            <Typography color="text.secondary">
                                {t("events.noSignedEvents")}
                            </Typography>
                        </Box>
                    )}
                </Paper>
            </Grid>
        </Grid>
    );
};

export default MyEventsPage;
