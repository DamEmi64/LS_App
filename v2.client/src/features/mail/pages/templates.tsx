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

import { Template } from "@/features/mail";
import YesNoWindow from "@/shared/components/YesNoWindow";

import { TemplateEdit } from "../components/templateEdit";
import { TemplateGenData, TemplateGen } from "../components/templateGen";
import { ResponseList } from "@/shared/api/extension";
import {call} from "@/shared";

const Templates: React.FC = () => {
    const { t } = useTranslation();
    const modal = useModal();
    const auth = useAuth();

    // 📱 RESPONSIVE
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const [data, setData] = useState<TableData<Template>>({
        data: [],
        total: 0
    });

    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

    // 🔄 REFRESH
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

    // ➕ ADD
    const addTemplate = () => {
        modal.showModal(
            <TemplateEdit
                template={{} as Template}
                onSave={addData}
                style={{ minWidth: isMobile ? "90vw" : "500px" }}  
            />
        );
    };

    // ✏️ EDIT
    const edit = (template: Template) => {
        modal.showModal(
            <TemplateEdit
                template={template}
                onSave={editData}
                style={{ minWidth: isMobile ? "90vw" : "500px" }}
            />
        );
    };

    // 👁 DETAILS
    const details = async (template: Template) => {
        const result = await call<Template>(api => api.templatesApi.getById,{id:template.id})

        modal.showModal(
            <TemplateEdit
                template={result}
                onSave={editData}
                readonly
            />
        );
    };

    // ⚙️ GENERATE
    const gen = (template: Template) => {
        const initialData: TemplateGenData = {
            template: template.id,
            sender: auth.user,
            recipients: []
        };

        modal.showModal(
            <TemplateGen
                initialData={initialData}
                onSubmit={genConfirm}
            />
        );
    };

    const genConfirm = async (data: TemplateGenData) => {
        await call(api => api.templatesApi.updateByIdGenerate,{id:data.template,emailGenerationDto:data});

        modal.hideModal();
        refresh();
    };

    // ❌ DELETE
    const del = (template: Template) => {
        modal.showModal(
            <YesNoWindow
                message={t("entity.del_info")}
                yesMethod={() => delConfirm(template)}
                noMethod={modal.hideModal}
                open
                onClose={modal.hideModal}
            />
        );
    };

    const delConfirm = async (template: Template) => {
        await call(api => api.templatesApi.updateById,{id:template.id, template});

        modal.hideModal();
        refresh();
    };

    // ➕ CREATE
    const addData = async (template: Template) => {
        await call(api => api.templatesApi.create,{template});
        modal.hideModal();
        refresh();
    };

    // ✏️ UPDATE
    const editData = async (template: Template) => {
        await call(api => api.templatesApi.updateById,{id:template.id, template});

        modal.hideModal();
        refresh();
    };

    // 📡 DATA FETCH
    const updateData = async (
        paramsObj: onChangeParams
    ): Promise<TableData<Template>> => {
        const query = {
            page: paramsObj.page?.toString() || '1',
            pageSize: paramsObj.pageSize?.toString() || '10',
            orderBy: paramsObj.orderBy || '',
            order: paramsObj.order || 'desc',
        };

        (paramsObj.filters || []).forEach(filter => {
            query[filter.field] = filter.value.toLocaleString();
        });

        const result = await call<ResponseList<Template>>(api => api.templatesApi.get,query);

        const tableData: TableData<Template> = {
            data: result.data,
            total: result.total
        };

        setData(tableData);
        setFilterValues(paramsObj.filters ?? []);

        return tableData;
    };

    // 📊 TABLE CONFIG
    const columns: TableColumn<Template>[] = [
        {
            field: "subject",
            header: "communication.template.subject",
            type: ColumnType.String
        }
    ];

    const filters: FilterItem[] = [
        {
            field: "subject",
            name: "communication.template.subject",
            type: FilterType.String
        }
    ];

    const operations: Operations<Template>[] = [
        { name: "opt.details", method: details },
        { name: "opt.edit", method: edit },
        { name: "communication.template.gen", method: gen },
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
                    {t("communication.template.title")}
                </Typography>

                <Typography
                    sx={{
                        color: "white",
                        fontSize: isMobile ? "0.9rem" : "1rem"
                    }}
                >
                    {t("communication.template.description")}
                </Typography>

                <Button
                    onClick={addTemplate}
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

export default Templates;