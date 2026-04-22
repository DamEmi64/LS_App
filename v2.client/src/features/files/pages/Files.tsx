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

import { FilterItem, FilterType, onChangeParams, Operations } from "@/shared";

import { saveAs } from "file-saver";

import { EditFile, File } from "@/features/files";
import { getDictionary, useDictionaryTranslation } from "@/lib/utils";

const Files: React.FC = () => {
    const api = useApiConnect();
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

    // ✅ FIXED types mapping
    const types = getDictionary('File types').map((item) => ({
        key: item.key,
        value: item.key,
        label: getDictionaryTranslation('File types', item.key).title
    }));

    // ✅ FIXED categories (concat bug)
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
        api.post("files_add", file, null).then(() => {
            modal.hideModal();
            refresh();
        });
    };

    const details = (file: File) => {
        api.get<File>("files_details", null, file.id).then(res => {
            modal.showModal(
                <FilesInfo file={res.data} edit={edit} del={del} />
            );
        });
    };

    const edit = (file: File) => {
        api.get<File>("files_details", null, file.id).then(res => {
            modal.showModal(
                <FilesEdit
                    file={toEditFile(res.data)}
                    toSave={(edited) => saveEdit(edited, file.id)}
                />
            );
        });
    };

    const saveEdit = (file: EditFile, id: string) => {
        api.put("files_details", file, null, id).then(() => {
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
        api.del("files_details", null, file.id).then(() => {
            modal.hideModal();
            refresh();
        });
    };

    const showFile = (file: File) => api.get("files_show", null, file.id);
    const importFile = (file: File) => api.put("files_import", null, null, file.id);

    const exportFile = (file: File) => {
        api.download("files_export", file.id)
            .then((response) => {
                const contentType =
                    response.headers["content-type"] || "application/octet-stream";

                const blob = new Blob([response.data], { type: contentType });

                saveAs(blob, file.title);
            })
            .catch(console.error);
    };

    const operations: Operations<File>[] = [
        { name: "opt.details", method: details },
        { name: "opt.edit", method: edit },
        { name: "opt.delete", method: del },
        { name: "files.show_file", method: showFile },
        { name: "files.import", method: importFile },
        { name: "files.export", method: exportFile }
    ];

    const filters: FilterItem[] = [
        { field: "title", name: "files.name", type: FilterType.String },
        { field: "location", name: "files.location", type: FilterType.String }
    ];

    const updateData = async (paramsObj: onChangeParams) => {
        setChangeParams(paramsObj);

        const query = new URLSearchParams({
            page: String(paramsObj.page ?? 0),
            pageSize: String(paramsObj.pageSize ?? 10),
            orderBy: paramsObj.orderBy ?? "",
            order: paramsObj.order ?? ""
        });

        paramsObj.filters?.forEach(f =>
            query.append(f.field, String(f.value))
        );

        const result = await api.get<File[]>("files_data", { params: query });
        if (result) setData(result.data);
    };

    const refresh = () => updateData(changeParams);

    useEffect(() => {
        refresh();
    }, []);

    return (
        <Grid container sx={{ width: "100%", minHeight: "100vh", flexDirection: "column", alignItems: "center", p: isMobile ? 1 : 2 }}>
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