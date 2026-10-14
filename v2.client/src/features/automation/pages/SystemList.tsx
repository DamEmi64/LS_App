import React, { useEffect, useState } from "react";
import { Button, Grid, InputLabel } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useModal } from "@/shared/context/modal";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import DoNotDisturbAltIcon from "@mui/icons-material/DoNotDisturbAlt";

import { DataTable } from "@/shared/components/datatables/datatable";
import { activateAutomat, deactivateAutomat, loadAutomations, updateConfiguration } from "../services/systemAutomationService";
import { SystemConfigurationForm } from "../components/SystemConfigurationForm";

import {
  TableColumn,
  ColumnType,
  onChangeParams,
  Operations,
  TableData
} from "@/shared";
import { SystemAutomat } from "../types";


const List: React.FC = () => {
  const { t } = useTranslation();
  const modal = useModal();

  // Table state
  const [data, setData] = useState<TableData<SystemAutomat>>({ data: [], total: 0 });
  const [page, setPage] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [orderBy, setOrderBy] = useState<string | null>(null);
  const [order, setOrder] = useState<"asc" | "desc">("asc");

  // Fetch data
  const updateData = async (paramsObj: onChangeParams): Promise<TableData<SystemAutomat>> => {
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

    const result = await loadAutomations(query);
    setData({ data: result.data, total: result.total });

    return { data: result.data, total: result.total };
  };

  useEffect(() => {
    updateData({ page: 0, pageSize: 10, orderBy: "", order: "asc", filters: [] });
  }, []);

  const refresh = () => {
    modal.hideModal();
    updateData({ page, pageSize, orderBy, order, filters: [] });
  };

  const turnOnOff = async (automat: SystemAutomat) => {
    const id = automat.id;
    if (automat.active) {
      await deactivateAutomat(id);
    }
    else {
      await activateAutomat(id);
    }

    refresh();
  };

  const editConfiguration = (automat: SystemAutomat) => {
    modal.showModal(
      <SystemConfigurationForm
        automat={automat}
        onCancel={modal.hideModal}
        onSave={async configuration => {
          await updateConfiguration(automat.id, configuration);
          refresh();
        }}
      />
    );
  };


  // Columns, filters, operations
  const columns: TableColumn<SystemAutomat>[] = [
    { field: "key", header: "automations.system.key", type: ColumnType.Number },
    { field: "configuration", header: "automations.system.configuration", type: ColumnType.String },
    { field: "active", header: "automations.active", type: ColumnType.Boolean,
        render: (row) =>(
            <Button onClick={() => turnOnOff(row)}>
                {row.active ? <CheckCircleOutlineIcon color="success"/> : <DoNotDisturbAltIcon color="error" />}
            </Button>
        )     
    }
];

  const operations: Operations<SystemAutomat>[] = [
    { name: "opt.edit", method: editConfiguration },
    { name: "automations.turnOnOff", method: turnOnOff }
  ];

  return (
    <Grid container sx={{ width: "100%", m: "auto", p: 2 }}>
      <Grid size={12}
        sx={{ display: "flex", flexDirection: "column", alignItems: "center", mb: 2 }}
      >
        <InputLabel sx={{ color: "white", fontSize: "2.5rem", fontWeight: "bold" }}>
          {t("automations.system.title")}
        </InputLabel>
        <InputLabel sx={{ color: "white", fontSize: "1rem", fontWeight: "bold" }}>
          {t("automations.subtitle")}
        </InputLabel>
      </Grid>

      <Grid size={12}>
        <DataTable
          columns={columns}
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
