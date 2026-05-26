import React, { useState } from "react";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";
import { Box, Button, Typography } from "@mui/material";
import { t } from "i18next";

type Props = {
  onSubmit: (date: Date | null) => void;
  onCancel?: () => void;
  initialDate?: Date | null;
  submitLabel?: string;
};

const ReminderSetup: React.FC<Props> = ({
  onSubmit,
  onCancel,
  initialDate = null,
}) => {
  const [date, setDate] = useState<Date | null>(initialDate);

  const handleSubmit = (e?: React.FormEvent) => {
    e?.preventDefault();
    onSubmit(date);
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <Box
        component="form"
        onSubmit={handleSubmit}
        sx={{ display: "flex", flexDirection: "column", gap: 3, minWidth: { xs: 260, sm: 360 } }}
      >
        <Typography variant="h6">Set reminder</Typography>
        <DateTimePicker
          label="Reminder"
          value={date}
          onChange={(newVal) => setDate(newVal as Date)}
          slotProps={{
            textField: {
              fullWidth: true,
              required: true,
            },
          }}
        />
        <Box sx={{ display: "flex", justifyContent: "flex-end", gap: 2 }}>
          {onCancel && (
            <Button type="button" variant="outlined" onClick={onCancel}>
              {t('opt.cancel')}
            </Button>
          )}
          <Button type="submit" variant="contained" color="primary" disabled={!date}>
            {t('opt.save')}
          </Button>
        </Box>
      </Box>
    </LocalizationProvider>
  );
};

export default ReminderSetup;
