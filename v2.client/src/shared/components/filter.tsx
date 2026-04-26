import React, { useState } from "react";
import { Button, FormControl, Grid, InputLabel, MenuItem, Select, TextField } from "@mui/material";
import { t } from "i18next";
import { FilterProps, FilterValue } from "../types";

export const Filter: React.FC<FilterProps> = ({ filters, onChange }) => {
    const [values, setValues] = useState<FilterValue[]>([]);

    const handleFilterChange = (field: string, value: string | number | Date | null) => {
        if (values.find(v => v.field === field)) {
            values.find(v => v.field === field)!.value = value;
        } else {
            values.push({ field, value });
        }
        setValues([...values]);
        onChange(values);
    };

    const resetFilter = () => {
        setValues([]);
        onChange([]);
    }
    // Calculate width based on number of filters (max 5 per row)
    const itemsPerRow = Math.min(filters.length, 5) + 1;
    const itemWidth = `${100 / itemsPerRow - 1}%`;

    return (
        <Grid
            style={{
                display: "flex",
                gap: 10,
                flexWrap: "wrap",
                flexDirection: "row",
                padding: 10,
                border: "1px solid #ccc",
                borderRadius: 5,
            }}
        >
            {filters.map((filter) => {
                const commonStyle = { display: "flex", gap: 5, width: itemWidth, minWidth: 120, flex: `1 1 ${itemWidth}` };
                switch (filter.type) {
                    case "enum":
                        return (
                            <Grid key={filter.field}>
                                <FormControl>
                                    <InputLabel id="demo-simple-select-label">{t(filter.name)}</InputLabel>
                                    <Select
                                        labelId="demo-simple-select-label"
                                        id="demo-simple-select"
                                        label={t(filter.name)}
                                        key={filter.field}
                                        value={
                                            (() => {
                                                const val = values.find(v => v.field === filter.field)?.value;
                                                if (val instanceof Date) return val.toISOString();
                                                return val ?? "      ";
                                            })()
                                        }
                                        onChange={(e) => handleFilterChange(filter.field, e.target.value)}
                                    >
                                        <MenuItem value="">---</MenuItem>
                                        {filter.options?.map((opt) => (
                                            <MenuItem value={opt.value}>
                                                {t(opt.label)}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Grid>
                        );
                    case "string":
                        return (
                            <div key={filter.field} style={commonStyle}>
                                <TextField id="standard-basic" label={t(filter.name)} variant="standard"
                                    type="text"
                                    value={
                                        (() => {
                                            const val = values.find(v => v.field === filter.field)?.value;
                                            if (val instanceof Date) return val.toISOString();
                                            return val ?? "";
                                        })()
                                    }
                                    onChange={(e) => handleFilterChange(filter.field, e.target.value)}
                                />
                            </div>
                        );
                    case "number":
                        return (
                            <div key={filter.field} style={commonStyle}>
                                <TextField id="standard-basic" label={t(filter.name)} variant="standard"
                                    type="number"
                                    value={
                                        (() => {
                                            const val = values.find(v => v.field === filter.field)?.value;
                                            if (val instanceof Date) return val.toISOString();
                                            return val ?? "";
                                        })()
                                    }
                                    onChange={(e) => handleFilterChange(filter.field, e.target.value)}
                                />
                            </div>
                        );
                    case "date":
                        return (
                            <div key={filter.field} style={{ display: "flex", gap: 5, width: itemWidth, minWidth: 120, flex: `1 1 ${itemWidth}`, flexDirection: 'column' }}>
                                <InputLabel id="demo-simple-select-label">{t(filter.name)}</InputLabel>
                                <TextField id="standard-basic" variant="standard"
                                    type="datetime-local"
                                    value={
                                        (() => {
                                            const val = values.find(v => v.field === filter.field)?.value;
                                            if (val instanceof Date) return val.toISOString();
                                            return val ?? "";
                                        })()
                                    }
                                    onChange={(e) => handleFilterChange(filter.field, e.target.value)}
                                />

                            </div>
                        );
                    default:
                        return null;
                }
            })}
            <Button onClick={resetFilter}>{t('filter_reset')}</Button>
        </Grid>
    );
};