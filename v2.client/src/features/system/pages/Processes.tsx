import { FilterItem, FilterProps, FilterType } from '@/shared';
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
import { useDictionaryTranslation } from '@/lib/utils';
import { ResponseList } from '@/shared/api/extension';

const Processes = () => {
    const { t } = useTranslation();
    const { processApi, call } = useApiConnect();
    const modal = useModal();

    const details = (data: Process) => {
        call<Process>(processApi, processApi.getProcesById, { id: data.id })
            .then(process => {
                modal.showModal(<ProcessInfo process={process}></ProcessInfo>);
            });
    }

    const updateData = (paramsObj: onChangeParams) => {
        const query = {
            page: paramsObj.page?.toString() || '1',
            pageSize: paramsObj.pageSize?.toString() || '10',
            orderBy: paramsObj.orderBy || '',
            order: paramsObj.order || 'desc',
        };

        (paramsObj.filters || []).forEach(filter => {
            query[filter.field] = filter.value.toLocaleString();
        });

        return call<ResponseList<Process>>(processApi, processApi.getProcesData, query)
            .then(response => ({ data: response.data, total: response.total } as TableData<Process>));
    };

    const convertProcessStatus = (id: string) => {
        if (id == 'New') return t('processes.processStatus.New');
        if (id == 'Executing') return t('processes.processStatus.Executing');
        if (id == 'Success') return t('processes.processStatus.Success');
        if (id == 'Failed') return t('processes.processStatus.Failed');
        if (id == 'Paused') return t('processes.processStatus.Paused');
        return id;
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
                { label: 'processes.processStatus.New', value: 'New' },
                { label: 'processes.processStatus.Executing', value: 'Executing' },
                { label: 'processes.processStatus.Success', value: 'Success' },
                { label: 'processes.processStatus.Failed', value: 'Failed' },
                { label: 'processes.processStatus.Paused', value: 'Paused' }
            ]
        }
    ];

    const operations: Operations<Process>[] = [
        { name: 'opt.details', method: (o) => details(o) }
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