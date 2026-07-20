import React, { Fragment, ReactNode, useState } from "react";

import {
    Box,
    CircularProgress,
    Collapse,
    IconButton,
    LinearProgress,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TableSortLabel,
    useMediaQuery,
    useTheme
} from "@mui/material";

import KeyboardArrowUpIcon from "@mui/icons-material/KeyboardArrowUp";
import KeyboardArrowDownIcon from "@mui/icons-material/KeyboardArrowDown";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import DoNotDisturbAltIcon from "@mui/icons-material/DoNotDisturbAlt";

import OperationCell from "@/shared/components/operationCell";
import { ColumnType, ExpandableTableProps, Filter } from "@/shared";
import { t } from "i18next";
import { convertToDateStr, useDictionaryTranslation } from "@/lib/utils";

export function ExpandableTable<T>({
    rows,
    columns,
    getRowId,
    operations,
    renderExpanded,
    loadingRow,
    onToggle,
    orderBy,
    order,
    onSort,
    filters,
    onFilterChange
}: ExpandableTableProps<T>) {

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
    const translateDictionary = useDictionaryTranslation();

    const [openRows, setOpenRows] = useState<Record<string, boolean>>({});

    const toggleRow = (row: T) => {
        const id = getRowId(row);
        const isOpen = !!openRows[id];

        setOpenRows(prev => ({
            ...prev,
            [id]: !isOpen
        }));

        onToggle?.(row, !isOpen);
    };

    const showData = (value: any, type: ColumnType, dictionary?: string) => {
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

            case ColumnType.Dictionary:
                return value ? translateDictionary(dictionary, value).title : "";

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
        <Paper sx={{
            width: "100%",
            maxWidth: 1200,
            mx: "auto",
            p: isMobile ? 1 : 2,
            borderRadius: 2
        }}>
            <Box sx={{ width: "100%", overflowX: "auto" }}>
                {filters && onFilterChange && (
                    <Filter filters={filters} onChange={onFilterChange} />
                )}
                <Table size={isMobile ? "small" : "medium"}>
                    <TableHead>
                        <TableRow>

                            {renderExpanded && <TableCell width={60} />}

                            {columns.map(col => (
                                <TableCell
                                    key={String(col.field)}
                                    width={col.width}
                                >
                                    {col.sortable ? (
                                        <TableSortLabel
                                            active={orderBy === col.field}
                                            direction={
                                                orderBy === col.field
                                                    ? order
                                                    : "asc"
                                            }
                                            onClick={() =>
                                                onSort?.(String(col.field))
                                            }
                                        >
                                            {t(col.header)}
                                        </TableSortLabel>
                                    ) : (
                                        t(col.header)
                                    )}
                                </TableCell>
                            ))}

                            {operations && <TableCell />}
                        </TableRow>
                    </TableHead>

                    <TableBody>
                        {rows.map(row => {
                            const id = getRowId(row);

                            const isOpen = !!openRows[id];
                            const isLoading = loadingRow === id;

                            return (
                                <Fragment key={id}>
                                    <TableRow>

                                        {renderExpanded && (
                                            <TableCell>
                                                <IconButton
                                                    onClick={() => toggleRow(row)}
                                                >
                                                    {isLoading ? (
                                                        <CircularProgress size={18} />
                                                    ) : isOpen ? (
                                                        <KeyboardArrowUpIcon />
                                                    ) : (
                                                        <KeyboardArrowDownIcon />
                                                    )}
                                                </IconButton>
                                            </TableCell>
                                        )}

                                        {columns.map(col => (
                                            <TableCell key={String(col.field)}>
                                                {col.render
                                                    ? col.render(row)
                                                    : showData((row as any)[col.field], col.type, col.dictionary)}
                                            </TableCell>
                                        ))}

                                        {operations && (
                                            <OperationCell
                                                operations={operations}
                                                data={row}
                                            />
                                        )}
                                    </TableRow>

                                    {renderExpanded && (
                                        <TableRow>
                                            <TableCell
                                                colSpan={
                                                    columns.length +
                                                    (operations ? 2 : 1)
                                                }
                                                sx={{ p: 0 }}
                                            >
                                                <Collapse
                                                    in={isOpen}
                                                    timeout="auto"
                                                    unmountOnExit
                                                >
                                                    <Box p={2}>
                                                        {renderExpanded(row)}
                                                    </Box>
                                                </Collapse>
                                            </TableCell>
                                        </TableRow>
                                    )}
                                </Fragment>
                            );
                        })}
                    </TableBody>
                </Table>
            </Box>
        </Paper>

    );
}