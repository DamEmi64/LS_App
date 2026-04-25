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
import { Automat, AutomatTask } from "../types";
import { ResponseList } from "@/shared/api/extension";


const List: React.FC = () => {
  const { t } = useTranslation();
  const modal = useModal();
  const { automationApi, call: mapResponse } = useApiConnect();

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
    const query = {
      page: page?.toString() || '1',
      pageSize: pageSize?.toString() || '10',
      orderBy: orderBy || '',
      order: order || 'desc',
    };

    (filters || []).forEach(filter => {
      query[filter.field] = filter.value.toLocaleString();
    });

    const result = await mapResponse<ResponseList<Automat>>(automationApi, automationApi.getAutomation, query);
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
    automationApi.createAutomation(automat as unknown).then(refresh);
  };

  const editAutomat = (automat: Automat) => {
    modal.showModal(
      <AutomatForm initialData={automat} onSubmit={edited => saveEdit(edited, automat.id)} />
    );
  };

  const saveEdit = (automat: Automat, id: string) => {
    mapResponse(automationApi, automationApi.updateAutomationById, { id, body: automat }).then(refresh);
  };

  const turnOnOff = (automat: Automat) => {

    const id = automat.id;
    if (automat.active) {
      mapResponse(automationApi, automationApi.updateAutomationByIdTurnoff, { id }).then(refresh);
    }
    else {
      mapResponse(automationApi, automationApi.updateAutomationByIdTurnon, { id }).then(refresh);
    }
  };

  const editTask = (automat: Automat, taskId: string, task: AutomatTask) => {
    const index = automat.tasks.findIndex(t => t.id === taskId);
    if (index >= 0) automat.tasks[index] = task;
    else automat.tasks.push(task);

    mapResponse(automationApi, automationApi.getAutomationById, { id: automat.id, automat }).then(refresh);
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
    mapResponse(automationApi, automationApi.updateAutomationById, { id: automat.id }).then(refresh);
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

      <Grid size={12}>
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