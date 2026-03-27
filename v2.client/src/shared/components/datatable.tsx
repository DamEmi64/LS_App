import React, { useState } from 'react';
import {
    Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
    TableSortLabel, TablePagination, Paper, Box,
    LinearProgress,
} from '@mui/material';

import { Filter } from '@/shared/components/filter';
import OperationCell from '@/shared/components/operationCell';
import { t } from 'i18next';
import { convertToDateStr } from '@/lib/utils';
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';
import DoNotDisturbAltIcon from '@mui/icons-material/DoNotDisturbAlt';
import { onChangeParams, ColumnType,TableProps, Row, FilterValue } from '@/shared';

export const DataTable = <T,>({
    columns,
    filters = [],
    data,
    operations = [],
    setData,
    onChange,
}: TableProps<T>) => {
    const [pageSize, setPageSize] = useState(10);
    const [orderBy, setOrderBy] = useState<string | null>(null);
    const [order, setOrder] = useState<'asc' | 'desc'>('asc');
    const [page, setPage] = useState(0);
    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);
    const updateData = (paramsObj: onChangeParams) => {
        onChange(paramsObj).then(x => {
            setData(x);
        });
    }

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [menuRowId, setMenuRowId] = useState<string | number | null>(null);

    const open = Boolean(anchorEl);

    const handleClick = (event: React.MouseEvent<HTMLElement>, rowId: string | number) => {
        setAnchorEl(event.currentTarget);
        setMenuRowId(rowId);
    };

    const handleClose = () => {
        setAnchorEl(null);
        setMenuRowId(null);
    };

    const handleSort = (field: string) => {
        setOrderBy(field);
        const isAsc = orderBy === field && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        updateData({
            page,
            pageSize,
            orderBy: field,
            order: isAsc ? 'desc' : 'asc',
            filters: filterValues,
        });
    };

    const handleChangePage = (
        event: React.MouseEvent<HTMLButtonElement> | null,
        newPage: number
    ) => {
        setPage(newPage);
        updateData({
            page,
            pageSize,
            orderBy,
            order,
            filters: filterValues,
        });
    };

    const handleChangeRowsPerPage = (
        event: React.ChangeEvent<HTMLInputElement>
    ) => {
        setPageSize(parseInt(event.target.value, 10));
        updateData({
            page: 0,
            pageSize: parseInt(event.target.value, 10),
            orderBy,
            order,
            filters: filterValues,
        });
    };

    const handleFilterChange = (newFilters: FilterValue[]) => {
        setFilterValues(newFilters);
        updateData({
            page,
            pageSize,
            orderBy,
            order,
            filters: newFilters,
        });
    };

    const showData = (data: any, type: ColumnType) => {
        let show;

        switch (type) {
            case ColumnType.Date:
                show = convertToDateStr(data);
                break;
            case ColumnType.Progress:
                show = (<LinearProgress variant="determinate" value={data} title={`${String(data)} %`} />);
                break;
            case ColumnType.Boolean:
                show = data ? (<CheckCircleOutlineIcon/>) : (<DoNotDisturbAltIcon />);
                break;
            default:
                show = String(data);
                break;
        }

        return show;
    }

    return (
        <Paper sx={{ width: '75%', overflow: 'hidden', margin: 'auto', padding: 2 }}>
            <Filter filters={filters} onChange={handleFilterChange} />
            <TableContainer>
                <Table size="small" stickyHeader >
                    <TableHead>
                        <TableRow>
                            {columns.map((col) => (
                                <TableCell
                                    key={String(col.field)}
                                    sortDirection={orderBy === col.field ? order : false}
                                >
                                    <TableSortLabel
                                        active={orderBy === col.field}
                                        direction={orderBy === col.field ? order : 'asc'}
                                        onClick={() => handleSort(col.field)}
                                    >
                                        {t(col.header)}
                                    </TableSortLabel>
                                </TableCell>
                            ))}
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data.data && (data.data.map((row) => (
                            <TableRow>
                                {columns.map((col) => (
                                    <TableCell key={String(col.field)}>
                                        {col.toShow && row[col.field] != null
                                            ? col.toShow(row[col.field])
                                            : showData(row[col.field],col.type)}
                                    </TableCell>
                                ))}
                                <OperationCell data={row} operations={operations} />
                            </TableRow>
                        )))}
                        {data.data.length === 0 && (
                            <TableRow>
                                <TableCell align="center" colSpan={columns.length}>
                                    {t('no_data')}
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
            <TablePagination
                component="div"
                count={data.total}
                page={page}
                onPageChange={handleChangePage}
                rowsPerPage={pageSize}
                onRowsPerPageChange={handleChangeRowsPerPage}
                rowsPerPageOptions={[5, 10, 25, 50]}
            />
        </Paper>
    );
}