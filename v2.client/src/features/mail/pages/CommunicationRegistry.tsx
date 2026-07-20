import React, { useEffect, useState } from "react";

import {
    Button,
    Grid,
    Typography,
    useMediaQuery,
    useTheme
} from "@mui/material";

import { useTranslation } from "react-i18next";
import { useModal } from "@/shared/context/modal";
import { call } from "@/shared";
import { useAuth } from "@/features/auth/context/authProvider";

import { DataTable } from "@/shared/components/datatables/datatable";

import {
    TableColumn,
    ColumnType,
    FilterItem,
    FilterType,
    FilterValue,
    onChangeParams,
    Operations,
    TableData
} from "@/shared";
import { Email, Registry } from "../types";
import { EmailEdit } from "../components/emailEdit";
import { ResponseList } from "@/shared/api/extension";

const CommunicationRegistry: React.FC = () => {
    const { t } = useTranslation();
    const modal = useModal();
    const auth = useAuth();

    // 📱 RESPONSIVE
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const [data, setData] = useState<TableData<Registry>>({
        data: [],
        total: 0
    });

    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

    const convertToEmailStructure = (registry: Registry) : Email => {
        return {
            id: registry.id,
            insDate: registry.insDate,
            subject: registry.title,
            body: registry.message,
            sender: registry.from,
            recipient: registry.to
        } as Email;
    }

    const details = async (registry: Registry) => {
        call<Registry>(api => api.communicationHistoryClient.getById, { id: registry.id })
            .then(res => modal.showModal(
                <EmailEdit
                    email={convertToEmailStructure(res)}
                    onSave={() => {}}   
                    readonly
                />
            ))
    };


    const updateData = async (
        paramsObj: onChangeParams
    ): Promise<TableData<Registry>> => {
        const query = {
        page: paramsObj.page?.toString() || '1',
        pageSize: paramsObj.pageSize?.toString() || '10',
        orderBy: paramsObj.orderBy || '',
        order: paramsObj.order || 'desc',
        };

        (paramsObj.filters || []).forEach(filter => {
        query[filter.field] = filter.value.toLocaleString();
        });

        const result = await call<ResponseList<Registry>>(api => api.communicationHistoryClient.get,query);


        const tableData: TableData<Registry> = {
            data: result.data,
            total: result.total
        };

        setData(tableData);
        setFilterValues(paramsObj.filters ?? []);

        return tableData;
    };

    const columns: TableColumn<Registry>[] = [
        { field: "title", header: "communication.email.subject", type: ColumnType.String },
        { field: "from", header: "communication.email.sender.title", type: ColumnType.String },
        { field: "to", header: "communication.email.recipient.title", type: ColumnType.String },
        { field: "insDate", header: "communication.email.sendDate", type: ColumnType.Date }
    ];

    const filters: FilterItem[] = [
        { field: "subject", name: "communication.email.subject", type: FilterType.String },
        { field: "from", name: "communication.email.sender.title", type: FilterType.String },
        { field: "to", name: "communication.email.recipient.title", type: FilterType.String },
        { field: "sentDate", name: "communication.email.sendDate", type: FilterType.DateRange }
    ];

    // ⚙️ OPERATIONS
    const operations: Operations<Registry>[] = [
        { name: "opt.details", method: details }
    ];

    // 🚀 INIT
    useEffect(() => {
        updateData({
            page: 0,
            pageSize: 10,
            orderBy: "",
            order: "asc",
            filters: []
        });
    }, []);

    return (
        <Grid
            container
            sx={{
                width: "100%",
                p: isMobile ? 1 : 2
            }}
        >
            {/* HEADER */}
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
                <Typography
                    sx={{
                        color: "white",
                        fontSize: isMobile ? "1.8rem" : "2.5rem",
                        fontWeight: "bold"
                    }}
                >
                    {t("communication.registry.title")}
                </Typography>

                <Typography
                    sx={{
                        color: "white",
                        fontSize: isMobile ? "0.9rem" : "1rem"
                    }}
                >
                    {t("communication.registry.description")}
                </Typography>
            </Grid>

            {/* TABLE */}
            <Grid size={{ xs: 12 }}>
                <DataTable
                    columns={columns}
                    filters={filters}
                    onChange={updateData}
                    data={data}
                    setData={setData}
                    operations={operations}
                />
            </Grid>
        </Grid>
    );
};

export default CommunicationRegistry;