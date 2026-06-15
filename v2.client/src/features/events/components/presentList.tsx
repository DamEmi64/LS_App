import React, { useState } from 'react';
import { Box, Button, Stack } from '@mui/material';
import {ColumnType, GridTable, TableColumn} from '@/shared';
import { EventParticipant } from '../types';
import { t } from 'i18next';


interface PresentListProps {
  participants: EventParticipant[];
  onSubmit: (updatedParticipants: EventParticipant[]) => void;
}

const PresentList: React.FC<PresentListProps> = ({ participants, onSubmit }) => {
  const [data, setData] = useState<EventParticipant[]>(participants);

const participantColumns: TableColumn<EventParticipant>[] = [
   {
      field: "login",
      header: "events.participants.login",
      type: ColumnType.String,
    },
    {
      field: "email",
      header: "events.participants.email",
      type: ColumnType.String,
    },
    {
      field: "present",
      header: "events.present",
      type: ColumnType.Boolean,
    },
  ];

  const handleCellEdit = (updatedRow: EventParticipant) => {
    setData(data.map(row => (row.id === updatedRow.id ? updatedRow : row)));
  };

  const handleSubmit = () => {
    onSubmit(data);
  };

  const handleCancel = () => {
    setData(participants);
  };

  return (
    <Box sx={{ width: '100%', p: 2 }}>
        <GridTable
          columns={participantColumns}
          data={{
            data: data || [],
            total: data?.length || 0,
          }}
          canDelete={false}
          canAdd={false}
          setData={(tableData) => setData(tableData.data)}
        />
      <Stack direction="row" spacing={2} sx={{ mt: 3 }}>
        <Button variant="contained" color="primary" onClick={handleSubmit}>
          {t('opt.save')}
        </Button>
        <Button variant="outlined" color="secondary" onClick={handleCancel}>
          {t('opt.cancel')}
        </Button>
      </Stack>
    </Box>
  );
};

export default PresentList;
