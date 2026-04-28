import React, { useState } from "react";
import {
    Box,
    Button,
    FormControl,
    Grid,
    InputLabel,
    MenuItem,
    Select,
    TextField,
    Typography
} from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { t } from "i18next";
import dayjs, { Dayjs } from "dayjs";

import { FilterProps, FilterValue } from "../types";

export const Filter: React.FC<FilterProps> = ({ filters, onChange }) => {
    const [values, setValues] = useState<FilterValue[]>([]);

    const getValue = (field: string) =>
        values.find(v => v.field === field)?.value ?? null;

    const handleFilterChange = (
        field: string,
        value: any
    ) => {
        const newValues = [...values];

        let normalizedValue = value;

        if (dayjs.isDayjs(value)) {
            normalizedValue = value.toDate().toISOString();
        } else if (value instanceof Date) {
            normalizedValue = value.toISOString();
        }

        const index = newValues.findIndex(v => v.field === field);

        if (index >= 0) {
            newValues[index] = { field, value: normalizedValue };
        } else {
            newValues.push({ field, value: normalizedValue });
        }

        setValues(newValues);
        onChange(newValues);
    };

    const resetFilter = () => {
        setValues([]);
        onChange([]);
    };

    return (
        <Grid container spacing={2} p={2}>
            {filters.map((filter) => {
                switch (filter.type) {

                    case "date":
                        return (
                            <Grid key={filter.field}>
                                <DatePicker
                                    label={t(filter.name)}
                                    value={
                                        getValue(filter.field)
                                            ? dayjs(getValue(filter.field))
                                            : null
                                    }
                                    onChange={(val: Dayjs | null) =>
                                        handleFilterChange(
                                            filter.field,
                                            val
                                        )
                                    }
                                />
                            </Grid>
                        );

                    case "dateRange":
                        const range = {
                            from: getValue(filter.field + "From"),
                            to: getValue(filter.field + "To"),
                        };

                        return (
                            <Grid key={filter.field}>
                                <Box
                                    sx={{
                                        border: "1px solid #ccc",
                                        borderRadius: 2,
                                        padding: 2,
                                        display: "flex",
                                        flexDirection: "column",
                                        gap: 2,
                                        minWidth: 280
                                    }}
                                >
                                    <Typography variant="caption" sx={{ opacity: 0.7 }}>
                                        {t(filter.name)}
                                    </Typography>

                                    <DatePicker
                                        value={range.from ? dayjs(range.from) : null}
                                        onChange={(val: Dayjs | null) =>
                                            handleFilterChange(
                                                filter.field + "From",
                                                val
                                            )
                                        }
                                    />

                                    <DatePicker
                                        value={range.to ? dayjs(range.to) : null}
                                        onChange={(val: Dayjs | null) =>
                                            handleFilterChange(
                                                filter.field + "To",
                                                val
                                            )
                                        }
                                    />
                                </Box>
                            </Grid>
                        );

                    case "enum":
                        return (
                            <Grid key={filter.field}>
                                <FormControl fullWidth>
                                    <InputLabel>{t(filter.name)}</InputLabel>
                                    <Select
                                        value={getValue(filter.field) ?? ""}
                                        label={t(filter.name)}
                                        onChange={(e) =>
                                            handleFilterChange(
                                                filter.field,
                                                e.target.value
                                            )
                                        }
                                    >
                                        <MenuItem value="">---</MenuItem>
                                        {filter.options?.map((opt) => (
                                            <MenuItem
                                                key={opt.value}
                                                value={opt.value}
                                            >
                                                {t(opt.label)}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Grid>
                        );

                    case "string":
                        return (
                            <Grid key={filter.field}>
                                <TextField
                                    label={t(filter.name)}
                                    value={getValue(filter.field) ?? ""}
                                    onChange={(e) =>
                                        handleFilterChange(
                                            filter.field,
                                            e.target.value
                                        )
                                    }
                                />
                            </Grid>
                        );

                    case "number":
                        return (
                            <Grid key={filter.field}>
                                <TextField
                                    type="number"
                                    label={t(filter.name)}
                                    value={getValue(filter.field) ?? ""}
                                    onChange={(e) =>
                                        handleFilterChange(
                                            filter.field,
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </Grid>
                        );

                    default:
                        return null;
                }
            })}

            <Grid>
                <Button onClick={resetFilter}>
                    {t("filter_reset")}
                </Button>
            </Grid>
        </Grid>
    );
};