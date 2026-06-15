import React, { useState, useEffect } from 'react';
import {
    IconButton,
    Paper,
    Stack
} from '@mui/material';

import { GridTableProps, ColumnType, TableData, TableColumn } from '@/shared';
import {
    DataGrid,
    GridAddIcon,
    GridColDef,
    GridDeleteIcon,
    GridEventListener,
    GridRowId
} from '@mui/x-data-grid';
import { useTranslation } from 'react-i18next';

export const GridTable = <T,>({
    columns,
    data,
    setData,
    canDelete = true,
    canAdd = true,
    readonly = false
}: GridTableProps<T>) => {

    const [rows, setRows] = useState<any[]>(data.data || []);
    const [isInternalUpdate, setIsInternalUpdate] = useState(false);

    const { t } = useTranslation();

    // 🔥 Sync with parent ONLY when change is external
    useEffect(() => {
        if (!isInternalUpdate) {
            setRows(data.data || []);
        } else {
            setIsInternalUpdate(false);
        }
    }, [data.data]);

    const getColumntType = (type: ColumnType) => {
        switch (type) {
            case ColumnType.Date:
                return 'datetime';
            case ColumnType.Enum:
                return 'singleSelect';
            case ColumnType.Boolean:
                return 'boolean';
            case ColumnType.Number:
                return 'number';
            default:
                return 'string';
        }
    };

    const toColumnGridDef = (data: TableColumn<T>[]) => {
        const cols = data.map((val) => ({
            field: val.field,
            headerName: t(val.header),
            editable: !readonly,
            type: getColumntType(val.type),
            valueOptions: val.options,
            flex: 1
        })) as GridColDef[];

        if (!canDelete) return cols;

        return [
            ...cols,
            {
                field: "actions",
                headerName: "",
                width: 80,
                sortable: false,
                filterable: false,
                renderCell: (params) => (
                    <IconButton
                        color="error"
                        onClick={() => handleDeleteRow(params.id)}
                    >
                        <GridDeleteIcon />
                    </IconButton>
                ),
            },
        ];
    };

    const upsertArrayItem = (array: any[], newItem: any, key = "id") => {
        const index = array.findIndex(item => item[key] === newItem[key]);

        if (index !== -1) {
            return array.map((item, i) =>
                i === index ? newItem : item
            );
        }

        return [...array, newItem];
    };

    const handleRowUpdate = (newRow: any) => {
        const updatedRows = upsertArrayItem(rows, newRow);

        setIsInternalUpdate(true);
        setRows(updatedRows);

        const newData: TableData<any> = {
            total: updatedRows.length,
            data: updatedRows
        };

        setData?.(newData);

        return newRow;
    };

    const handleRowEditStop: GridEventListener<'rowEditStop'> = () => {
        setData?.({ total: rows.length, data: rows });
    };

    const handleAddRow = () => {
        const newId =
            rows.length > 0
                ? Math.max(...rows.map((r) => Number(r.id) || 0)) + 1
                : 1;

        const emptyRow: any = { id: newId };

        columns.forEach((col) => {
            if (col.field !== "id") {
                emptyRow[col.field] = "";
            }
        });

        const updatedRows = [...rows, emptyRow];

        setIsInternalUpdate(true);
        setRows(updatedRows);

        const newData: TableData<any> = {
            total: updatedRows.length,
            data: updatedRows
        };

        setData?.(newData);
    };

    const handleDeleteRow = (id: GridRowId) => {
        const updatedRows = rows.filter((row) => row.id !== id);

        setIsInternalUpdate(true);
        setRows(updatedRows);

        const newData: TableData<any> = {
            total: updatedRows.length,
            data: updatedRows
        };

        setData?.(newData);
    };

    return (
        <Paper sx={{ overflow: 'hidden', margin: 'auto', padding: 2 }}>
            {!readonly && canAdd && (
                <Stack direction="row" spacing={1} sx={{ mb: 1 }}>
                    <IconButton size="small" color='success' onClick={handleAddRow}>
                        <GridAddIcon />
                    </IconButton>
                </Stack>
            )}

            <DataGrid
                columns={toColumnGridDef(columns)}
                rows={rows}
                getRowId={(row) => row.id}
                initialState={{
                    pagination: {
                        paginationModel: { pageSize: 5, page: 0 },
                    },
                }}
                pageSizeOptions={[5, 10, 25, 50, 100]}
                editMode="cell"
                processRowUpdate={readonly ? undefined : handleRowUpdate}
                onRowEditStop={readonly ? undefined : handleRowEditStop}
            />
        </Paper>
    );
};
