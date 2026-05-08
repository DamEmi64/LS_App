import React, { useState } from "react";

import {
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TableSortLabel,
    TablePagination,
    Paper,
    Box,
    LinearProgress,
    useTheme,
    useMediaQuery
} from "@mui/material";

import { Filter } from "@/shared/components/filter";
import OperationCell from "@/shared/components/operationCell";
import { t } from "i18next";

import { convertToDateStr } from "@/lib/utils";

import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import DoNotDisturbAltIcon from "@mui/icons-material/DoNotDisturbAlt";

import {
    onChangeParams,
    ColumnType,
    TableProps,
    FilterValue
} from "@/shared";

export const DataTable = <T,>({
    columns,
    filters = [],
    data,
    operations = [],
    setData,
    onChange
}: TableProps<T>) => {

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const [pageSize, setPageSize] = useState(10);
    const [orderBy, setOrderBy] = useState<string | null>(null);
    const [order, setOrder] = useState<"asc" | "desc">("asc");
    const [page, setPage] = useState(0);
    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

    const updateData = async (paramsObj: onChangeParams) => {
        const result = await onChange(paramsObj);
        setData(result);
    };

    const handleSort = (field: string) => {
        const isAsc = orderBy === field && order === "asc";
        const newOrder = isAsc ? "desc" : "asc";

        setOrderBy(field);
        setOrder(newOrder);

        updateData({
            page,
            pageSize,
            orderBy: field,
            order: newOrder,
            filters: filterValues
        });
    };

    // 📄 PAGE CHANGE FIXED
    const handleChangePage = (_: any, newPage: number) => {
        setPage(newPage);

        updateData({
            page: newPage,
            pageSize,
            orderBy,
            order,
            filters: filterValues
        });
    };

    const handleChangeRowsPerPage = (
        event: React.ChangeEvent<HTMLInputElement>
    ) => {
        const newSize = parseInt(event.target.value, 10);

        setPageSize(newSize);
        setPage(0);

        updateData({
            page: 0,
            pageSize: newSize,
            orderBy,
            order,
            filters: filterValues
        });
    };

    // 🔍 FILTERS
    const handleFilterChange = (newFilters: FilterValue[]) => {
        setFilterValues(newFilters);
        setPage(0);

        updateData({
            page: 0,
            pageSize,
            orderBy,
            order,
            filters: newFilters
        });
    };

    const showData = (value: any, type: ColumnType) => {
        switch (type) {
            case ColumnType.Date:
                return convertToDateStr(value);

            case ColumnType.Progress:
                return (
                    <LinearProgress
                        variant="determinate"
                        value={value}
                    />
                );

            case ColumnType.Boolean:
                return value ? (
                    <CheckCircleOutlineIcon color="success" />
                ) : (
                    <DoNotDisturbAltIcon color="error" />
                );

            default:
                return String(value ?? "");
        }
    };

    return (
        <Paper
            sx={{
                margin: "auto",
                p: isMobile ? 1 : 2
            }}
        >
            {/* FILTERS */}
            <Filter filters={filters} onChange={handleFilterChange} />

            {/* TABLE WRAPPER (IMPORTANT FOR MOBILE) */}
            <TableContainer sx={{
                overflowX: "auto",
                size: isMobile ? 'small' : 'medium',
                WebkitOverflowScrolling: "touch"
            }}>
                <Table
                    stickyHeader
                    sx={{
                        minWidth: 900,
                         width: "100%",
                    }}
                >

                    {/* HEADER */}
                    <TableHead>
                        <TableRow>
                            {columns.map((col) => (
                                <TableCell key={String(col.field)}>
                                    <TableSortLabel
                                        active={orderBy === col.field}
                                        direction={orderBy === col.field ? order : "asc"}
                                        onClick={() => handleSort(col.field)}
                                    >
                                        {t(col.header)}
                                    </TableSortLabel>
                                </TableCell>
                            ))}

                            <TableCell />
                        </TableRow>
                    </TableHead>

                    {/* BODY */}
                    <TableBody>
                        {data.data?.length > 0 ? (
                            data.data.map((row: any, idx) => (
                                <TableRow key={row.id ?? idx}>
                                    {columns.map((col) => (
                                        <TableCell key={String(col.field)}>
                                            {col.toShow && row[col.field] != null
                                                ? col.toShow(row[col.field])
                                                : showData(row[col.field], col.type)}
                                        </TableCell>
                                    ))}

                                    <OperationCell
                                        data={row}
                                        operations={operations}
                                    />
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell
                                    align="center"
                                    colSpan={columns.length + 1}
                                >
                                    {t("no_data")}
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            {/* PAGINATION */}
            <TablePagination
                component="div"
                count={data.total}
                page={page}
                onPageChange={handleChangePage}
                rowsPerPage={pageSize}
                onRowsPerPageChange={handleChangeRowsPerPage}
                rowsPerPageOptions={isMobile ? [5, 10] : [5, 10, 25, 50]}
                sx={{
                    "& .MuiTablePagination-toolbar": {
                        flexWrap: isMobile ? "wrap" : "nowrap"
                    }
                }}
            />
        </Paper>
    );
};