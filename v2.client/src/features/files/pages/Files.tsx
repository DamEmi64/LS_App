import React, { useEffect, useState } from "react";
import {
    Grid,
    FormLabel,
    useTheme,
    useMediaQuery
} from "@mui/material";

import SwitchSelector from "react-switch-selector";
import TileContainer from "@/shared/components/tileContainer";

import { useTranslation } from "react-i18next";
import { useApiConnect } from "@/shared/context/apiConnect";
import { useModal } from "@/shared/context/modal";

import FilesEdit from "@/features/files/components/filesEdit";
import FilesInfo from "@/features/files/components/filesInfo";
import YesNoWindow from "@/shared/components/YesNoWindow";

import { call, FilterItem, FilterType, onChangeParams, Operations, raw } from "@/shared";

import { saveAs } from "file-saver";

import { EditFile, File } from "@/features/files";
import { getDictionary, useDictionaryTranslation } from "@/lib/utils";
import { ResponseList } from "@/shared/api/extension";

const Files: React.FC = () => {
    const modal = useModal();
    const { t } = useTranslation();

    const getDictionaryTranslation = useDictionaryTranslation();
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const [data, setData] = useState<File[]>([]);

    const [changeParams, setChangeParams] = useState<onChangeParams>({
        page: 0,
        pageSize: 10,
        orderBy: "",
        order: "asc",
        filters: []
    });

    const types = getDictionary('File types').map((item) => ({
        key: item.key,
        value: item.key,
        label: getDictionaryTranslation('File types', item.key).title
    }));

    const categories = [
        { key: '', value: '', label: '*' },
        ...types
    ];

    const categoryChange = (value: string) => {
        const type = value ? categories.find((c) => c.value === value) : undefined;

        const filters = type && type.key
            ? [{ field: "fileType", value: type.key }]
            : [];

        updateData({
            ...changeParams,
            page: 0,
            filters
        });
    };

    const toEditFile = (file: File): EditFile => ({
        ...file,
        gameGenre: file.additionalData?.gameGenre,
        subject: file.additionalData?.subject,
        semester: file.additionalData?.semester,
        year: file.additionalData?.year,
        sourceType: file.sources?.[0]?.sourceType,
        links: file.sources?.map(s => s.link).join("\n")
    } as EditFile);

    // 📡 CRUD (unchanged)
    const addFile = async () => {
        modal.showModal(<FilesEdit file={{} as EditFile} toSave={saveNew} />);
    };

    const saveNew = (file: EditFile) => {
        call(api => api.filesApi.createFile, file).then(() => {
            modal.hideModal();
            refresh();
        });
    };

    const details = (file: File) => {
        call<File>(api => api.filesApi.getFileById, { id: file.id }).then(res => {
            modal.showModal(
                <FilesInfo file={res} edit={edit} del={del} />
            );
        });
    };

    const edit = (file: File) => {
        call<File>(api => api.filesApi.getFileById, { id: file.id }).then(res => {
            modal.showModal(
                <FilesEdit
                    file={toEditFile(res)}
                    toSave={(edited) => saveEdit(edited, file.id)}
                />
            );
        });
    };

    const saveEdit = (file: EditFile, id: string) => {
        call(api => api.filesApi.updateFileById, { id, body: file }).then(() => {
            modal.hideModal();
            refresh();
        });
    };

    const del = (file: File) => {
        modal.showModal(
            <YesNoWindow
                message={t("entity.del_info")}
                yesMethod={() => delConfirm(file)}
                noMethod={modal.hideModal}
                open
                onClose={modal.hideModal}
            />
        );
    };

    const delConfirm = (file: File) => {
        call(api => api.filesApi.deleteFileById, { id: file.id }).then(() => {
            modal.hideModal();
            refresh();
        });
    };

    const importFile = (file: File) => call(api => api.filesApi.updateFileByIdImport, { id: file.id, body: file });

    const exportFile = (file: File) => {
        raw(api => api.filesApi.getFileByIdExport, { id: file.id })
            .then((response) => {
                const contentType =
                    response.headers["content-type"] || "application/octet-stream";

                const blob = new Blob([response.data], { type: contentType.toLocaleString() });

                saveAs(blob, file.title);
            })
            .catch(console.error);
    };

    const operations: Operations<File>[] = [
        { name: "opt.details", method: details },
        { name: "opt.edit", method: edit },
        { name: "opt.delete", method: del },
        { name: "files.import", method: importFile },
        { name: "files.export", method: exportFile }
    ];

    const filters: FilterItem[] = [
        { field: "title", name: "files.name", type: FilterType.String },
        { field: "location", name: "files.location", type: FilterType.String }
    ];

    const updateData = async (paramsObj: onChangeParams) => {
        setChangeParams(paramsObj);

        const query = {
            page: paramsObj.page?.toString() || '1',
            pageSize: paramsObj.pageSize?.toString() || '10',
            orderBy: paramsObj.orderBy || '',
            order: paramsObj.order || 'desc',
        };

        (paramsObj.filters || []).forEach(filter => {
            query[filter.field] = filter.value.toLocaleString();
        });

        call<ResponseList<File>>(filesApi, filesApi.getFile, query).then(res => setData(res.data));
    };

    const refresh = () => updateData(changeParams);

    useEffect(() => {
        refresh();
    }, []);

    return (
        <Grid container sx={{ width: "100%", flexDirection: "column", alignItems: "center", p: isMobile ? 1 : 2 }}>
            <Grid size={12} sx={{ textAlign: "center", mb: 2 }}>
                <FormLabel sx={{ color: "white", fontSize: isMobile ? "1.8rem" : "2.5rem", fontWeight: "bold" }}>
                    {t("files.title")}
                </FormLabel>
            </Grid>

            <Grid size={12} sx={{ mb: 2, display: "flex", justifyContent: "center", overflowX: "auto" }}>
                <SwitchSelector options={categories} onChange={categoryChange} />
            </Grid>

            <Grid size={12} sx={{ width: "100%", maxWidth: 1200 }}>
                <TileContainer
                    data={data}
                    updateData={updateData}
                    filters={filters}
                    addData={addFile}
                    operations={operations}
                />
            </Grid>
        </Grid>
    );
};

export default Files;