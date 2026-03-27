import { FilterItem, FilterProps, FilterType} from '@/shared';
import { DataTable } from "@/shared/components/datatable";
import { useTranslation } from "react-i18next";

import { useApiConnect } from "@/shared/context/apiConnect";
import { useEffect, useState } from "react";
import { useModal } from "@/shared/context/modal";
import axios from "axios";
import ProcessInfo from "@/features/system/components/processInfo";
import { FormLabel, Grid } from "@mui/material";
import { Process } from "@/features/auth";
import { ColumnDef, ColumnType, onChangeParams, Operations, TableData } from "@/shared";

const Processes = () => {
    const { t } = useTranslation();
    const api = useApiConnect();
    const modal = useModal();

    const details = (data) => {
        api.get<Process>('process_details', null, data.id)
            .then(process => {
                modal.showModal(<ProcessInfo process={process.data}></ProcessInfo>);
            });
    }

    const restart = (data) => {
        api.post('process_restart', null, null, data.id);
    }

    const updateData = (paramsObj: onChangeParams) => {
        const { page, pageSize, orderBy, order, filters } = paramsObj;
        const params = new URLSearchParams({
            page: page?.toString() || '1',
            pageSize: pageSize?.toString() || '10',
            orderBy: orderBy || '',
            order: order || '',
        });

        (filters || []).forEach(filter => {
            params.append(filter.field, filter.value.toLocaleString());
        });

        return api.get<Process[]>("process_data", { params })
            .then(response => ({ data: response.data, total: response.total } as TableData<Process>));
    };

    const convertProcessStatus = (id: string) => {
        return t('dictionaries.processStatus.' + id);
    };

    const columns: ColumnDef[] = [
        { field: 'title', header: 'processes.name', type: ColumnType.String },
        { field: 'percentage', header: 'processes.percentage', type: ColumnType.Progress },
        { field: 'startDate', header: 'processes.startingDate', type: ColumnType.Date },
        { field: 'status', header: 'processes.status', type: ColumnType.Enum, toShow: convertProcessStatus }
    ];

    const filters: FilterItem[] = [
        { field: 'name', name: 'processes.name', type: FilterType.String },
        { field: 'from', name: 'processes.startingDateFrom', type: FilterType.Date },
        { field: 'to', name: 'processes.startingDateTo', type: FilterType.Date },
        {
            field: 'status', name: 'Status', type: FilterType.Enum, options: [
                { label: 'dictionaries.processStatus.New', value: 'New' },
                { label: 'dictionaries.processStatus.Executing', value: 'Executing' },
                { label: 'dictionaries.processStatus.Success', value: 'Success' },
                { label: 'dictionaries.processStatus.Failed', value: 'Failed' },
                { label: 'dictionaries.processStatus.Paused', value: 'Paused' }
            ]
        }
    ];

    const operations: Operations<Process>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'processes.restart', method: (o) => restart(o) }
    ]

    const [data, setData] = useState<TableData<Process>>({ data: [], total: 0 });

    // Always use updateData for initial load
    useEffect(() => {
        setTimeout(() => {
            updateData({ page: 0, pageSize: 10, orderBy: '', order: 'asc', filters: [] })
                .then(result => {
                    setData(result);
                });
        }, 2000);
    }, []);

    return (
        <Grid style={{ width: '100%', margin: 'auto', padding: '20px' }}>
            <Grid style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginBottom: '20px', flexDirection: "column" }}>
                <FormLabel style={{
                    color: 'white',
                    fontSize: '2.5rem',
                    fontWeight: 'bold',
                    display: 'inline-block'
                }}>
                    {t('processes.title')}
                </FormLabel>
                <FormLabel style={{
                    color: 'white',
                    fontSize: '1rem',
                    fontWeight: 'bold',
                    display: 'inline-block'
                }}>
                    {t('processes.description')}
                </FormLabel>
            </Grid>

            <DataTable
                columns={columns}
                filters={filters}
                onChange={updateData}
                data={data}
                setData={setData}
                operations={operations}
            />
        </Grid>
    );
};

export default Processes;