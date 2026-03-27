import React, { useEffect, useState } from "react";
import { Grid, FormLabel } from "@mui/material";
import SwitchSelector from "react-switch-selector";
import TileContainer from "@/shared/components/tileContainer";
import { useTranslation } from "react-i18next";
import { useApiConnect } from "@/shared/context/apiConnect";
import { useModal } from "@/shared/context/modal";
import FilesEdit from "@/features/files/components/filesEdit";
import FilesInfo from "@/features/files/components/filesInfo";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { FilterItem, FilterType, onChangeParams, Operations } from "@/shared";
import * as dictionaries from "@/app/dictionaries.json";
import { saveAs } from 'file-saver';
import { EditFile, File } from "@/features/files";

const Files: React.FC = () => {
  const api = useApiConnect();
  const modal = useModal();
  const { t } = useTranslation();

  const [data, setData] = useState<File[]>([]);
  const [changeParams, setChangeParams] = useState<onChangeParams>({
    page: 0,
    pageSize: 10,
    orderBy: "",
    order: "asc",
    filters: []
  });

  // Convert a File to EditFile for editing
  const toEditFile = (file: File): EditFile => {
    return {
      ...file,
      gameGenre: file.additionalData?.gameGenre,
      subject: file.additionalData?.subject,
      semester: file.additionalData?.semester,
      year: file.additionalData?.year,
      sourceType: file.sources?.[0]?.sourceType,
      links: file.sources?.map(s => s.link).join("\n")
    } as EditFile;
  };

  // Category switch
  const categories = [
    { label: <span>{t("files.all")}</span>, value: "" },
    { label: <span>{t("files.games")}</span>, value: "Games" },
    { label: <span>{t("files.docs")}</span>, value: "Docs" }
  ];

  const categoryChange = (value: string) => {
    const type = value ? dictionaries.FileTypes[value] : undefined;
    const filters = type ? [{ field: "fileType", value: type }] : [];
    updateData({ ...changeParams, page: 0, filters });
  };

  // API functions
  const addFile = async () => {
    modal.showModal(<FilesEdit file={{} as EditFile} toSave={saveNew} />);
  };

  const saveNew = (file: EditFile) => {
    api
      .post<EditFile>("files_add", file, null)
      .then(() => {
        modal.hideModal();
        refresh();
      });
  };

  const details = (file: File) => {
    api.get<File>("files_details", null, file.id).then(res => {
      modal.showModal(<FilesInfo file={res.data} edit={edit} del={del} />);
    });
  };

  const edit = (file: File) => {
    api.get<File>("files_details", null, file.id).then(res => {
      modal.showModal(
        <FilesEdit
          file={toEditFile(res.data)}
          toSave={edited => saveEdit(edited, file.id)}
        />
      );
    });
  };

  const saveEdit = (file: EditFile, id: string) => {
    api.put<EditFile>("files_details", file, null, id).then(() => {
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
    api.del<File>("files_details", null, file.id).then(() => {
      modal.hideModal();
      refresh();
    });
  };

  const showFile = (file: File) => {
    api.get("files_show", null, file.id);
  };

  const importFile = (file: File) => {
    api.put("files_import", null, null, file.id);
  };

  const exportFile = (file: File) => {
    api.download("files_export", file.id) 
           .then((response) => {
                // Extract content type from response headers
                const contentType = response.headers['content-type'] || 'application/octet-stream';

                // Create a Blob from the response data
                const blob = new Blob([response.data], { type: contentType });
    
                // Use file-saver to trigger download
                saveAs(blob, file.title);
            })
            .catch((error) => {
                console.error('Download failed:', error);
            });
  };

  // Table operations
  const operations: Operations<File>[] = [
    { name: "opt.details", method: details },
    { name: "opt.edit", method: edit },
    { name: "opt.delete", method: del },
    { name: "files.show_file", method: showFile },
    { name: "files.import", method: importFile },
    { name: "files.export", method: exportFile }
  ];

  // Filters
  const filters: FilterItem[] = [
    { field: "title", name: "files.name", type: FilterType.String },
    { field: "locaction", name: "files.location", type: FilterType.String }
  ];

  const refresh = () => updateData(changeParams);

  const updateData = async (paramsObj: onChangeParams) => {
    setChangeParams(paramsObj);
    const { page, pageSize, orderBy, order, filters } = paramsObj;

    const query = new URLSearchParams({
      page: (page ?? 0).toString(),
      pageSize: (pageSize ?? 10).toString(),
      orderBy: orderBy ?? "",
      order: order ?? ""
    });

    filters?.forEach(f => query.append(f.field, f.value.toString()));

    const result = await api.get<File[]>("files_data", { params: query });
    if (result) setData(result.data);
  };

  useEffect(() => {
    refresh();
  }, []);

  return (
    <Grid
      container
      sx={{
        width: "100%",
        minHeight: "100vh",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        p: 2
      }}
    >
      <Grid size={12} sx={{ mb: 2, textAlign: "center" }}>
        <FormLabel sx={{ color: "white", fontSize: "2.5rem", fontWeight: "bold" }}>
          {t("files.title")}
        </FormLabel>
      </Grid>

      <Grid size={12} sx={{ mb: 2, display: "flex", justifyContent: "center" }}>
        <SwitchSelector options={categories} onChange={categoryChange} />
      </Grid>

      <Grid size={12}>
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