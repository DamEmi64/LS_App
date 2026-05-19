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

import { DataTable } from "@/shared/components/datatable";

import {
    ColumnDef,
    ColumnType,
    FilterItem,
    FilterType,
    FilterValue,
    onChangeParams,
    Operations,
    TableData
} from "@/shared";

import { EmailEdit } from "@/features/mail/components/emailEdit";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { Email } from "@/features/mail";
import { ResponseList } from "@/shared/api/extension";

const Emails: React.FC = () => {
    const { t } = useTranslation();
    const modal = useModal();
    const auth = useAuth();

    // 📱 RESPONSIVE
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const [data, setData] = useState<TableData<Email>>({
        data: [],
        total: 0
    });

    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

    // 🔄 ADD EMAIL
    const addEmail = () => {
        const email: Email = {
            sender: auth.user?.email || ""
        } as Email;

        modal.showModal(
            <EmailEdit email={email} onSave={addData} />
        );
    };

    // ✏️ EDIT EMAIL
    const edit = (email: Email) => {
        modal.showModal(
            <EmailEdit email={email} onSave={editData} />
        );
    };

    // 👁 DETAILS
    const details = async (email: Email) => {
        call<Email>(api => api.emailsApi.get, { id: email.id })
            .then(res => modal.showModal(
                <EmailEdit
                    email={res}
                    onSave={editData}
                    readonly
                />
            ))
    };

    const send = async (email: Email) => {
        call(api => api.emailsApi.updateByIdSend,{id:email.id});

    };

    const del = (email: Email) => {
        modal.showModal(
            <YesNoWindow
                message={t("entity.del_info")}
                yesMethod={() => delConfirm(email)}
                noMethod={modal.hideModal}
                open
                onClose={modal.hideModal}
            />
        );
    };

    const delConfirm = async (email: Email) => {
        call(api => api.emailsApi.deleteById,{id:email.id}).then(refresh);
    };

    const addData = async (email: Email) => {
        await call(api => api.emailsApi.create,{email});
        modal.hideModal();
        refresh();
    };

    const editData = async (email: Email) => {
        await call(api => api.emailsApi.updateById,{id:email.id, email});

        modal.hideModal();
        refresh();
    };

    // 🔄 REFRESH WRAPPER
    const refresh = () => {
        modal.hideModal();

        updateData({
            page: 0,
            pageSize: 10,
            orderBy: "",
            order: "asc",
            filters: filterValues
        });
    };

    // 📡 TABLE DATA
    const updateData = async (
        paramsObj: onChangeParams
    ): Promise<TableData<Email>> => {
        const query = {
        page: paramsObj.page?.toString() || '1',
        pageSize: paramsObj.pageSize?.toString() || '10',
        orderBy: paramsObj.orderBy || '',
        order: paramsObj.order || 'desc',
        };

        (paramsObj.filters || []).forEach(filter => {
        query[filter.field] = filter.value.toLocaleString();
        });

        const result = await call<ResponseList<Email>>(api => api.emailsApi.get,query);


        const tableData: TableData<Email> = {
            data: result.data,
            total: result.total
        };

        setData(tableData);
        setFilterValues(paramsObj.filters ?? []);

        return tableData;
    };

    // 📊 COLUMNS
    const columns: ColumnDef[] = [
        { field: "subject", header: "communication.email.subject", type: ColumnType.String },
        { field: "sender", header: "communication.email.sender.title", type: ColumnType.String },
        { field: "recipient", header: "communication.email.recipient.title", type: ColumnType.String },
        { field: "sentDate", header: "communication.email.sendDate", type: ColumnType.Date }
    ];

    // 🔍 FILTERS
    const filters: FilterItem[] = [
        { field: "subject", name: "communication.email.subject", type: FilterType.String },
        { field: "sender", name: "communication.email.sender.title", type: FilterType.String },
        { field: "recipient", name: "communication.email.recipient.title", type: FilterType.String },
        { field: "sentDate", name: "communication.email.sendDate", type: FilterType.Date }
    ];

    // ⚙️ OPERATIONS
    const operations: Operations<Email>[] = [
        { name: "opt.details", method: details },
        { name: "opt.edit", method: edit },
        { name: "communication.email.send", method: send },
        { name: "opt.delete", method: del }
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
                    {t("communication.email.title")}
                </Typography>

                <Typography
                    sx={{
                        color: "white",
                        fontSize: isMobile ? "0.9rem" : "1rem"
                    }}
                >
                    {t("communication.email.description")}
                </Typography>

                <Button
                    onClick={addEmail}
                    variant="outlined"
                    sx={{ mt: 1 }}
                    fullWidth={isMobile}
                >
                    {t("opt.add")}
                </Button>
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

export default Emails;