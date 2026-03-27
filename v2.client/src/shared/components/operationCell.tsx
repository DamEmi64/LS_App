import { TableCell } from '@mui/material';
import React from 'react';
import { Operations } from '../types';
import { OperationMenu } from './operationMenu';

type OperationCellProps<T> = {
    operations: Operations<T>[],
    data: T
};

const OperationCell = <T,>({
    data,
    operations = [],

}: OperationCellProps<T>) => {
    return (
        <TableCell align="right">
            <OperationMenu data={data} operations={operations} />
        </TableCell>
    );
};

export default OperationCell;