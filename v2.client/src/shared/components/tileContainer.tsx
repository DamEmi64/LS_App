import React, { useState } from 'react';
import { Grid, Card, CardMedia, CardContent, Typography, TablePagination, Paper, Tabs, Tab, Box, CardActionArea, CardActions, Menu, MenuItem, IconButton } from '@mui/material';
import { Filter } from './filter';

import { useTranslation } from 'react-i18next';
import addNew from '@/assets/addNew.png'
import { FilterValue, OperationMenu, Row, TileContainerProps } from '@/shared';



const TileContainer = <T extends Row,>({ updateData, filters, data, addData, operations }: TileContainerProps<T>) => {
    const { t } = useTranslation();
    const [pageSize, setPageSize] = useState(10);
    const [orderBy, setOrderBy] = useState<string | null>(null);
    const [order, setOrder] = useState<'asc' | 'desc'>('asc');
    const [page, setPage] = useState(0);
    const [rowCount, setRowCount] = useState(0);
    const [filterValues, setFilterValues] = useState<FilterValue[]>([]);

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

    const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const handleSort = (field: string) => {
        setOrderBy(field);
        const isAsc = orderBy === field && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        updateData({
            page,
            pageSize,
            orderBy,
            order,
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

    return (
        <div>
            <Paper sx={{ width: '75%', overflow: 'hidden', margin: 'auto', padding: 2 }}>
                <Filter filters={filters} onChange={handleFilterChange} />
            </Paper>
            <Grid
                container
                sx={{ justifyContent: 'center', alignItems: 'flex-start' }}
                spacing={5}
            >
                {data && data.map((item) => (
                    <Card
                        key={item.id}
                        sx={{
                            width: 300,   // fixed width
                            height: 350,  // fixed height
                            display: 'flex',
                            flexDirection: 'column',
                            justifyContent: 'space-between',
                            m: 2
                        }}
                    >
                        <CardMedia
                            component="img"
                            image={item.imageData || addNew}
                            alt={item.title}
                            sx={{ height: 250, objectFit: 'cover' }} // consistent image height
                        />
                        <CardContent sx={{ flexGrow: 1 }}>
                            <Grid display={'flex'} justifyContent="space-between" alignItems="center">
                                <Typography variant="h6" component="div" noWrap>
                                    {item.title}
                                </Typography>
                                <OperationMenu data={item} operations={operations} />
                            </Grid>
                        </CardContent>
                    </Card>
                ))}

                <CardActionArea sx={{ width: 300, height: 350, m: 2 }} onClick={addData}>
                    <Card sx={{ width: '100%', height: '100%', display: 'flex', flexDirection: 'column' }}>
                        <CardMedia
                            component="img"
                            sx={{ height: 250, objectFit: 'cover' }}
                            image={addNew}
                        />
                        <CardContent sx={{ flexGrow: 1 }}>
                            <Typography variant="h6" component="div">
                                {t('opt.add')}
                            </Typography>
                        </CardContent>
                    </Card>
                </CardActionArea>
            </Grid>

            <TablePagination
                component="div"
                count={data.length}
                page={page}
                onPageChange={handleChangePage}
                rowsPerPage={pageSize}
                onRowsPerPageChange={handleChangeRowsPerPage}
                rowsPerPageOptions={[5, 10, 25, 50]}
            />
        </div>
    );
}

export default TileContainer;