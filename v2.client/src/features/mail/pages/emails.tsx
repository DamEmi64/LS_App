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
import { useApiConnect } from "@/shared/context/apiConnect";
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

const Emails: React.FC = () => {
    const { t } = useTranslation();
    const modal = useModal();
    const api = useApiConnect();
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
        const result = await api.get<Email>(
            "communication_email_details",
            null,
            email.id
        );

        modal.showModal(
            <EmailEdit
                email={result.data}
                onSave={editData}
                readonly
            />
        );
    };

    // 📤 SEND
    const send = async (email: Email) => {
        await api.put(
            "communication_email_send",
            email,
            null,
            email.id
        );
    };

    // ❌ DELETE
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
        await api.del("communication_email_del", null, email.id);
        refresh();
    };

    // ➕ CREATE
    const addData = async (email: Email) => {
        await api.post(
            "communication_email_new",
            email,
            null,
            email.id
        );

        modal.hideModal();
        refresh();
    };

    // ✏️ UPDATE
    const editData = async (email: Email) => {
        await api.put(
            "communication_email_edit",
            email,
            null,
            email.id
        );

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
        const query = new URLSearchParams({
            page: String(paramsObj.page ?? 0),
            pageSize: String(paramsObj.pageSize ?? 10),
            orderBy: paramsObj.orderBy ?? "",
            order: paramsObj.order ?? ""
        });

        paramsObj.filters?.forEach(f =>
            query.append(f.field, String(f.value))
        );

        const result = await api.get<Email[]>(
            "communication_email_data",
            { params: query }
        );

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