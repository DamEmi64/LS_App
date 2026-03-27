import React, { useEffect, useState } from "react";
import { Button, Grid, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useModal } from "@/shared/context/modal";
import { useApiConnect } from "@/shared/context/apiConnect";
import { useAuth } from "@/features/auth/context/authProvider";

import { DataTable } from "@/shared/components/datatable";
import { ColumnDef, ColumnType, FilterItem, FilterType, FilterValue, onChangeParams, Operations, TableData } from "@/shared";


import {Template } from "@/features/mail";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { TemplateEdit } from "../components/templateEdit";
import { TemplateGenData, TemplateGen } from "../components/templateGen";


const Templates: React.FC = () => {
  const { t } = useTranslation();
  const modal = useModal();
  const auth = useAuth();
  const api = useApiConnect();

  const [data, setData] = useState<TableData<Template>>({ data: [], total: 0 });
  const [page, setPage] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [orderBy, setOrderBy] = useState<string | null>(null);
  const [order, setOrder] = useState<"asc" | "desc">("asc");
  const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

  // Refresh table
  const refresh = () => {
    modal.hideModal();
    updateData({ page, pageSize, orderBy, order, filters: filterValues });
  };

  // Open modal to add new template
  const addTemplate = () => {
    const template = {} as Template;
    modal.showModal(<TemplateEdit template={template} onSave={editData} />);
  };

  // Open modal to edit template
  const edit = (template: Template) => {
    modal.showModal(<TemplateEdit template={template} onSave={editData} />);
  };

  // Show template details (readonly)
  const details = async (template: Template) => {
    const result = await api.get<Template>("communication_template_details", null, template.id);
    modal.showModal(<TemplateEdit template={result.data} onSave={editData} readonly />);
  };

  // Generate template
  const gen = (template: Template) => {
    const initialData: TemplateGenData = {
      template: template.id,
      sender: auth.user,
      recipients: []
    };
    modal.showModal(<TemplateGen initialData={initialData} onSubmit={genConfirm} />);
  };

  const genConfirm = async (data: TemplateGenData) => {
    await api.put<TemplateGenData>("communication_template_gen", data, null, data.template);
    modal.hideModal();
    refresh();
  };

  // Delete template
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
    await api.del<Template>("communication_template_del", null, template.id);
    modal.hideModal();
    refresh();
  };

  // Save template edits
  const editData = async (template: Template) => {
    await api.put<Template>("communication_template_edit", template, null, template.id);
    modal.hideModal();
    refresh();
  };

  // Fetch table data
  const updateData = async (paramsObj: onChangeParams): Promise<TableData<Template>> => {
    const { page, pageSize, orderBy, order, filters } = paramsObj;
    const query = new URLSearchParams({
      page: (page ?? 0).toString(),
      pageSize: (pageSize ?? 10).toString(),
      orderBy: orderBy ?? "",
      order: order ?? ""
    });

    filters?.forEach(f => query.append(f.field, f.value.toString()));

    const result = await api.get<Template[]>("communication_template_data", { params: query });
    const tableData: TableData<Template> = { data: result.data, total: result.total };
    setData(tableData);
    return tableData;
  };

  // Table columns
  const columns: ColumnDef[] = [
    { field: "subject", header: "communication.template.subject", type: ColumnType.String }
  ];

  // Table filters
  const filters: FilterItem[] = [
    { field: "subject", name: "communication.template.subject", type: FilterType.String }
  ];

  // Table row operations
  const operations: Operations<Template>[] = [
    { name: "opt.details", method: details },
    { name: "opt.edit", method: edit },
    { name: "communication.template.gen", method: gen },
    { name: "opt.delete", method: del }
  ];

  // Initial load
  useEffect(() => {
    updateData({ page: 0, pageSize: 10, orderBy: "", order: "asc", filters: [] });
  }, []);

  return (
    <Grid container sx={{ width: "100%", m: "auto", p: 2 }}>
      <Grid size={{ xs: 12 }} sx={{ display: "flex", flexDirection: "column", alignItems: "center", mb: 2 }}>
        <Typography sx={{ color: "white", fontSize: "2.5rem", fontWeight: "bold" }}>
          {t("communication.template.title")}
        </Typography>
        <Typography sx={{ color: "white", fontSize: "1rem", fontWeight: "bold" }}>
          {t("communication.template.description")}
        </Typography>
        <Button onClick={addTemplate} variant="outlined" sx={{ mt: 1 }}>
          {t("opt.add")}
        </Button>
      </Grid>

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