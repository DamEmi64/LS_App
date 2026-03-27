import React, { useEffect, useState } from "react";
import { Button, Grid, InputLabel } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useModal } from "@/shared/context/modal";
import { useApiConnect } from "@/shared/context/apiConnect";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { DataTable } from "@/shared/components/datatable";
import { AutomatForm } from "@/features/automation/components/AutomatForm";

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

import { Automat, Task } from "@/models/Automations";

const List: React.FC = () => {
  const { t } = useTranslation();
  const modal = useModal();
  const api = useApiConnect();

  // Table state
  const [data, setData] = useState<TableData<Automat>>({ data: [], total: 0 });
  const [page, setPage] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [orderBy, setOrderBy] = useState<string | null>(null);
  const [order, setOrder] = useState<"asc" | "desc">("asc");
  const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

  // Fetch data
  const updateData = async (paramsObj: onChangeParams): Promise<TableData<Automat>> => {
    const { page, pageSize, orderBy, order, filters } = paramsObj;
    const query = new URLSearchParams({
      page: (page ?? 0).toString(),
      pageSize: (pageSize ?? 10).toString(),
      orderBy: orderBy ?? "",
      order: order ?? ""
    });

    filters?.forEach(f => query.append(f.field, f.value.toString()));

    const result = await api.get<Automat[]>("automation_data", { params: query });
    setData({ data: result.data, total: result.total });

    return { data: result.data, total: result.total };
  };

  useEffect(() => {
    updateData({ page: 0, pageSize: 10, orderBy: "", order: "asc", filters: [] });
  }, []);

  const refresh = () => {
    modal.hideModal();
    updateData({ page, pageSize, orderBy, order, filters: filterValues });
  };

  // CRUD operations
  const addAutomat = () => {
    modal.showModal(<AutomatForm initialData={{} as Automat} onSubmit={saveNew} />);
  };

  const saveNew = (automat: Automat) => {
    api.post<Automat>("automation_new", automat, null).then(refresh);
  };

  const editAutomat = (automat: Automat) => {
    modal.showModal(
      <AutomatForm initialData={automat} onSubmit={edited => saveEdit(edited, automat.id)} />
    );
  };

  const saveEdit = (automat: Automat, id: string) => {
    api.put<Automat>("automation_edit", automat, null, id).then(refresh);
  };

  const turnOnOff = (automat: Automat) => {
    const url = automat.active ? "automation_turn_off" : "automation_turn_on";
    api.put<Automat>(url, automat, null, automat.id).then(refresh);
  };

  const editTask = (automat: Automat, taskId: string, task: Task) => {
    const index = automat.tasks.findIndex(t => t.id === taskId);
    if (index >= 0) automat.tasks[index] = task;
    else automat.tasks.push(task);

    api.put<Automat>("automation_edit", automat, null, automat.id).then(refresh);
  };

  const del = (automat: Automat) => {
    modal.showModal(
      <YesNoWindow
        message={t("entity.del_info")}
        yesMethod={() => delConfirm(automat)}
        noMethod={modal.hideModal}
        open
        onClose={modal.hideModal}
      />
    );
  };

  const delConfirm = (automat: Automat) => {
    api.del<Automat>("automation_del", null, automat.id).then(refresh);
  };

  // Columns, filters, operations
  const columns: ColumnDef[] = [
    { field: "title", header: "automations.title", type: ColumnType.String },
    { field: "description", header: "automations.description", type: ColumnType.String },
    { field: "active", header: "automations.active", type: ColumnType.Boolean }
  ];

  const filters: FilterItem[] = [
    { field: "title", name: "automations.title", type: FilterType.String },
    { field: "frequency", name: "automations.frequency", type: FilterType.Date },
    { field: "active", name: "automations.active", type: FilterType.Boolean }
  ];

  const operations: Operations<Automat>[] = [
    { name: "opt.edit", method: editAutomat },
    { name: "automations.turnOnOff", method: turnOnOff },
    { name: "opt.delete", method: del }
  ];

  return (
    <Grid container sx={{ width: "100%", m: "auto", p: 2 }}>
      <Grid size={12}
        sx={{ display: "flex", flexDirection: "column", alignItems: "center", mb: 2 }}
      >
        <InputLabel sx={{ color: "white", fontSize: "2.5rem", fontWeight: "bold" }}>
          {t("automations.site_title")}
        </InputLabel>
        <InputLabel sx={{ color: "white", fontSize: "1rem", fontWeight: "bold" }}>
          {t("automations.subtitle")}
        </InputLabel>
        <Button variant="outlined" onClick={addAutomat}>
          {t("opt.add")}
        </Button>
      </Grid>

      <Grid  size={12}>
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

export default List;