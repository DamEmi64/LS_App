import React, { useState } from "react";
import {
    Box,
    Button,
    Checkbox,
    FormControl,
    FormControlLabel,
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
import { randomUUID } from "crypto";
import { getDictionary, useDictionaryTranslation } from "@/lib/utils";

export const Filter: React.FC<FilterProps> = ({ filters, onChange }) => {
    const [values, setValues] = useState<FilterValue[]>([]);

    const translateDictionary = useDictionaryTranslation();

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
                            <Grid key={filter.field + '_filter'}>
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
                            <>
                                <DatePicker
                                    label={t(filter.name)+' ' + t('opt.from')} 
                                    key={filter.field + '_filterFrom'}
                                    value={range.from ? dayjs(range.from) : null}
                                    onChange={(val: Dayjs | null) =>
                                        handleFilterChange(
                                            filter.field + "From",
                                            val
                                        )
                                    }
                                />

                                <DatePicker
                                    label={t(filter.name)+' ' + t('opt.to')} 
                                    key={filter.field + '_filterTo'}
                                    value={range.to ? dayjs(range.to) : null}
                                    onChange={(val: Dayjs | null) =>
                                        handleFilterChange(
                                            filter.field + "To",
                                            val
                                        )
                                    }
                                />
                            </>
                        );

                    case "enum":
                        return (
                            <Grid key={filter.field + '_filter'} minWidth={200}>
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
                            <Grid key={filter.field + '_filter'}>
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
                            <Grid key={filter.field + '_filter'}>
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
                    case "boolean":
                        return (
                            <Grid key={filter.field + '_filter'}>
                                <FormControlLabel
                                    key={filter.field + '_filter_boolean'}
                                    control={
                                        <Checkbox
                                            checked={getValue(filter.field) === true}
                                            onChange={(e) =>
                                                handleFilterChange(
                                                    filter.field,
                                                    e.target.checked
                                                )
                                            }
                                        />
                                    }
                                    label={t(filter.name)}
                                />
                            </Grid>
                        );
                    case "dictionary":

                        var dictionary = getDictionary(filter.dictionary);
                        
                        return (
                            <Grid key={filter.field + '_filter'} minWidth={200}>
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
                                        {dictionary?.map((opt) => (
                                            <MenuItem
                                                key={opt.key}
                                                value={opt.key}
                                            >
                                                {translateDictionary(filter.dictionary, opt.key).title}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Grid>
                        );
                    default:
                        return null;
                
            }})}

            <Grid>
                <Button onClick={resetFilter}>
                    {t("filter_reset")}
                </Button>
            </Grid>
        </Grid>
    );
};