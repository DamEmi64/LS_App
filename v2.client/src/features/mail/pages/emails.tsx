import React, { useEffect, useState } from "react";
import { Button, Grid, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useModal } from "@/shared/context/modal";
import { useApiConnect } from "@/shared/context/apiConnect";
import { useAuth } from "@/features/auth/context/authProvider";

import { DataTable } from "@/shared/components/datatable";
import { ColumnDef, ColumnType, FilterItem, FilterType, FilterValue, onChangeParams, Operations, TableData } from "@/shared";


import { EmailEdit } from "@/features/mail/components/emailEdit";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { Email } from "@/features/mail";

const Emails: React.FC = () => {
  const { t } = useTranslation();
  const modal = useModal();
  const api = useApiConnect();
  const auth = useAuth();

  const [data, setData] = useState<TableData<Email>>({ data: [], total: 0 });
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

  // Open modal to add new email
  const addEmail = () => {
    const email = {} as Email;
    email.sender = auth.user?.email || "";
    modal.showModal(<EmailEdit email={email} onSave={addData} />);
  };

  // Open modal to edit existing email
  const edit = (email: Email) => {
    modal.showModal(<EmailEdit email={email} onSave={editData} />);
  };

  // Show email details (readonly)
  const details = async (email: Email) => {
    const result = await api.get<Email>("communication_email_details", null, email.id);
    modal.showModal(<EmailEdit email={result.data} onSave={editData} readonly />);
  };

  // Send email
  const send = (email: Email) => {
    api.put<Email>("communication_email_send", email, null, email.id);
  };

  // Delete email
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
    await api.del<Email>("communication_email_del", null, email.id);
    refresh();
  };

  // Add new email
  const addData = async (email: Email) => {
    await api.post<Email>("communication_email_new", email, null, email.id);
    modal.hideModal();
    refresh();
  };

  // Edit existing email
  const editData = async (email: Email) => {
    await api.put<Email>("communication_email_edit", email, null, email.id);
    modal.hideModal();
    refresh();
  };

  // Fetch table data
  const updateData = async (paramsObj: onChangeParams): Promise<TableData<Email>> => {
    const { page, pageSize, orderBy, order, filters } = paramsObj;
    const query = new URLSearchParams({
      page: (page ?? 0).toString(),
      pageSize: (pageSize ?? 10).toString(),
      orderBy: orderBy ?? "",
      order: order ?? ""
    });

    filters?.forEach(f => query.append(f.field, f.value.toString()));

    const result = await api.get<Email[]>("communication_email_data", { params: query });
    const tableData: TableData<Email> = { data: result.data, total: result.total };
    setData(tableData);
    return tableData;
  };

  // Table columns
  const columns: ColumnDef[] = [
    { field: "subject", header: "communication.email.subject", type: ColumnType.String },
    { field: "sender", header: "communication.email.sender.title", type: ColumnType.String },
    { field: "recipient", header: "communication.email.recipient.title", type: ColumnType.String },
    { field: "sentDate", header: "communication.email.sendDate", type: ColumnType.Date }
  ];

  // Table filters
  const filters: FilterItem[] = [
    { field: "subject", name: "communication.email.subject", type: FilterType.String },
    { field: "sender", name: "communication.email.sender.title", type: FilterType.String },
    { field: "recipient", name: "communication.email.recipient.title", type: FilterType.String },
    { field: "sentDate", name: "communication.email.sendDate", type: FilterType.Date }
  ];

  // Row operations
  const operations: Operations<Email>[] = [
    { name: "opt.details", method: details },
    { name: "opt.edit", method: edit },
    { name: "communication.email.send", method: send },
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
          {t("communication.email.title")}
        </Typography>
        <Typography sx={{ color: "white", fontSize: "1rem", fontWeight: "bold" }}>
          {t("communication.email.description")}
        </Typography>
        <Button onClick={addEmail} variant="outlined" sx={{ mt: 1 }}>
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

export default Emails;