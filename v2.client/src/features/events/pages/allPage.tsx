import React, { useEffect, useState } from "react";

import {
    Button,
    Grid,
    InputLabel,
    Typography,
    useMediaQuery,
    useTheme
} from "@mui/material";

import { useTranslation } from "react-i18next";

import EventComponent from "@/features/events/components/eventComponent";
import ReminderSetup from "@/features/events/components/reminderSetup";
import { EventBody, EventParticipant } from "@/features/events/types";
import { useModal } from "@/shared/context/modal";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { call, ColumnType, ExpandableTable, FilterItem, FilterType, FilterValue, onChangeParams, Operations, TableColumn } from "@/shared";
import { EventDto } from "@/shared/api/generated";
import { ResponseList } from "@/shared/api/extension";

const toEventBody = (event: EventDto): EventBody => ({
    title: event.title || "",
    eventDate: event.eventDate || "",
    description: event.description || "",
    image: event.image || "",
    imageContent: event.imageContent || "",
    category: event.category,
    participants: (event.participates || []).map<EventParticipant>((participant) => ({
        id: participant.id || participant.userId || "",
        login: participant.login || participant.email || "",
        email: participant.email || "",
    }))
});

const EventsPage: React.FC = () => {
    const { t } = useTranslation();
    const modal = useModal();

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const [data, setData] = useState<EventDto[]>([]);
    const [page, setPage] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const [orderBy, setOrderBy] = useState<string | null>(null);
    const [order, setOrder] = useState<"asc" | "desc">("asc");
    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);
    const [expandedData, setExpandedData] = useState<Record<string, EventDto>>({});
    const [loadingRow, setLoadingRow] = useState<string | null>(null);

    const updateData = async (paramsObj: onChangeParams) => {
        const query = {
            page: paramsObj.page?.toString() || "1",
            pageSize: paramsObj.pageSize?.toString() || "10",
            orderBy: paramsObj.orderBy || "",
            order: paramsObj.order || "desc",
        };

        (paramsObj.filters || []).forEach(filter => {
            query[filter.field] = filter.value?.toLocaleString?.() ?? filter.value;
        });

        const result = await call<ResponseList<EventDto>>(api => api.eventClient.get, query);

        setData(result.data || []);
        setFilterValues(paramsObj.filters ?? []);
    };

    const refresh = () => {
        modal.hideModal();
        updateData({ page, pageSize, orderBy, order, filters: filterValues });
    };

    const reloadExpanded = async (event: EventDto) => {
        if (!event.id) return;

        setLoadingRow(event.id);
        const freshEvent = await call<EventDto>(api => api.eventClient.getById, { id: event.id });

        setExpandedData(prev => ({
            ...prev,
            [event.id!]: freshEvent
        }));
        setLoadingRow(null);
    };

    const handleToggle = (event: EventDto, open: boolean) => {
        if (open) reloadExpanded(event);
    };

    const handleSort = (field: string) => {
        const nextOrder = orderBy === field && order === "asc" ? "desc" : "asc";

        setOrderBy(field);
        setOrder(nextOrder);
        updateData({ page, pageSize, orderBy: field, order: nextOrder, filters: filterValues });
    };

    const handleFilterChange = (filters: FilterValue[]) => {
        setFilterValues(filters);
        updateData({ page, pageSize, orderBy, order, filters });
    };

    const addEvent = () => {
        modal.showModal(
            <EventComponent
                event={{}}
                onSave={saveNew}
                isNew
            />
        );
    };

    const saveNew = (event: EventBody) => {
        call(api => api.eventClient.create, { eventDto: event }).then(refresh);
    };

    const edit = (event: EventDto) => {
        if (!event.id) return;

        call<EventDto>(api => api.eventClient.getById, { id: event.id })
            .then(freshEvent => modal.showModal(
                <EventComponent
                    event={toEventBody(freshEvent)}
                    onSave={(updatedEvent) => saveEdit(freshEvent, updatedEvent)}
                    isEdit
                />
            ));
    };

    const saveEdit = (event: EventDto, updatedEvent: EventBody) => {
        if (!event.id) return;

        call(api => api.eventClient.updateById, {
            id: event.id,
            eventDto: {
                ...event,
                title: updatedEvent.title,
                description: updatedEvent.description,
                eventDate: updatedEvent.eventDate,
                image: updatedEvent.image,
                imageContent: updatedEvent.imageContent,
                category: updatedEvent.category
            }
        }).then(refresh);
    };

    const del = (event: EventDto) => {
        modal.showModal(
            <YesNoWindow
                message={t("entity.del_info")}
                yesMethod={() => delConfirm(event)}
                noMethod={modal.hideModal}
                open
                onClose={modal.hideModal}
            />
        );
    };

    const delConfirm = (event: EventDto) => {
        if (!event.id) return;

        call(api => api.eventClient.deleteById, { id: event.id }).then(refresh);
    };

    const signIn = (event: EventDto) => {
        if (!event.id) return;

        call(api => api.eventClient.updateByIdSignIn, { id: event.id }).then(() => {
            refresh();
            reloadExpanded(event);
        });
    };

    const signOut = (event: EventDto) => {
        if (!event.id) return;

        call(api => api.eventClient.updateByIdSignOut, { id: event.id }).then(() => {
            refresh();
            reloadExpanded(event);
        });
    };

    const sendInvitation = (event: EventDto) => {
        if (!event.id) return;

        call(api => api.eventClient.createByIdInvitation, { id: event.id }).then(() => {
            refresh();
            reloadExpanded(event);
        });
    };

    const setReminder = (event: EventDto) => {
        if (!event.id) return;

        modal.showModal(
            <ReminderSetup
                onSubmit={(reminderDate) => saveReminder(event, reminderDate)}
                onCancel={modal.hideModal}
            />
        );
    };

    const saveReminder = (event: EventDto, reminderDate: Date | null) => {
        if (!event.id || !reminderDate) return;

        call(api => api.eventClient.createByIdReminder, {
            id: event.id,
            reminderDto: {
                reminderDate: reminderDate.toISOString()
            }
        }).then(() => {
            refresh();
            reloadExpanded(event);
        });
    };

    const removeReminder = (event: EventDto) => {
        if (!event.id) return;

        call(api => api.eventClient.deleteByIdReminder, { id: event.id }).then(() => {
            refresh();
            reloadExpanded(event);
        });
    };

    const columns: TableColumn<EventDto>[] = [
        { field: "title", header: "events.title", type: ColumnType.String, sortable: true },
        { field: "eventDate", header: "events.eventDate", type: ColumnType.Date, sortable: true },
        { field: "categoryId", header: "events.category", type: ColumnType.Dictionary, dictionary: "Event categories" },
    ];

    const filters: FilterItem[] = [
        { field: "title", name: "events.title", type: FilterType.String },
        { field: "date", name: "events.eventDate", type: FilterType.DateRange },
        { field: "category", name: "events.category", type: FilterType.Dictionary, dictionary: "Event categories" },
    ];

    const operations: Operations<EventDto>[] = [
        { name: "opt.edit", method: edit },
        { name: "opt.delete", method: del },
        { name: "events.signIn", method: signIn },
        { name: "events.signOut", method: signOut },
        { name: "events.sendInvitation", method: sendInvitation },
        { name: "events.setReminder", method: setReminder },
        { name: "events.removeReminder", method: removeReminder },
    ];

    useEffect(() => {
        updateData({ page: 0, pageSize: 10, orderBy: "", order: "asc", filters: [] });
    }, []);

    return (
        <Grid container sx={{ width: "100%", p: isMobile ? 1 : 3 }}>
            <Grid
                size={{ xs: 12 }}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                    mb: 2,
                    textAlign: "center"
                }}
            >
                <InputLabel sx={{ color: "white", fontSize: isMobile ? "1.8rem" : "2.5rem", fontWeight: "bold" }}>
                    {t("events.siteTitle")}
                </InputLabel>

                <Button onClick={addEvent} variant="outlined" sx={{ mt: 1 }} fullWidth={isMobile}>
                    {t("opt.add")}
                </Button>
            </Grid>

            <Grid size={{ xs: 12 }}>
                <ExpandableTable
                    rows={data}
                    columns={columns}
                    operations={operations}
                    getRowId={(event) => event.id || ""}
                    order={order}
                    orderBy={orderBy}
                    onSort={handleSort}
                    filters={filters}
                    onFilterChange={handleFilterChange}
                    loadingRow={loadingRow}
                    onToggle={handleToggle}
                    renderExpanded={(event) => {
                        const expandedEvent = event.id ? expandedData[event.id] : undefined;

                        return expandedEvent ? (
                            <EventComponent event={toEventBody(expandedEvent)} readonly />
                        ) : (
                            <Typography>{t("no_data")}</Typography>
                        );
                    }}
                />
            </Grid>
        </Grid>
    );
};

export default EventsPage;
