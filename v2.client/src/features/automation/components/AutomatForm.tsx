import React, { useState } from "react";
import {
    Box,
    Button,
    TextField,
    Switch,
    FormControlLabel,
    MenuItem,
    Select,
    InputLabel,
    FormControl,
    Typography,
    Paper,
    IconButton,
    Checkbox,
    ListItemText,
    Grid
} from "@mui/material";
import DeleteIcon from '@mui/icons-material/Delete';
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { GridAddIcon } from "@mui/x-data-grid";
import ArchiveTaskForm from "./Tasks/ArchiveTaskForm";
import SummaryTaskForm from "./Tasks/SummaryRPGTaskJob";
import { Automat, Trigger } from "../../../models/Automations";
import { useTheme } from "@mui/material";
import SummaryLastRPGTaskForm from "./Tasks/SummaryLastRPGTaskJob";

const taskTypes = [
    { id: 1, label: "automations.actions.archive" },
    { id: 2, label: "automations.actions.GenerateRPGSummary" },
    { id: 3, label: "automations.actions.GenerateRPGSummary" },
];

const triggerTypes = [
    { id: 'Cron', label: "automations.triggers.cron" },
    { id: 'Notifier', label: "automations.triggers.onRPGModify" }
];

export const AutomatForm = ({ initialData, onSubmit }) => {
    const { t } = useTranslation();
    const theme = useTheme();
    const textColor =
        theme.palette.mode === "dark"
            ? theme.palette.text.primary
            : theme.palette.text.secondary;

    const { control, handleSubmit, formState: { errors } } = useForm<Automat>({ defaultValues: initialData });
    const [tasks, setTasks] = useState(initialData.tasks || []);
    const [triggers, setTriggers] = useState(initialData.triggers || []);

    const addTrigger = () => {
        const newTrigger = {
            id: crypto.randomUUID(),
            name: "",
            description: "",
            type: 1,
            cron: "",
            eventId: []
        };
        setTriggers([...triggers, newTrigger]);
    };

    const updateTrigger = (index, updated: Trigger) => {
        //if type is rpg changed
        if (updated.type == 2) {
            updated.eventId = [];
            for (let i = 1012; i <= 1027; i++) {
                updated.eventId.push(i);
            }
        }

        const arr = [...triggers];
        arr[index] = updated;
        setTriggers(arr);
    };

    const deleteTrigger = (index) => {
        setTriggers(triggers.filter((_, i) => i !== index));
    };

    const onSubmitInternal = (data) => {
        onSubmit({ ...data, tasks: convertTasks(tasks), triggers: triggers });
    }

    const addTask = () => {
        const newTask = {
            id: crypto.randomUUID(),
            name: "",
            operationId: 1,
            order: tasks.length + 1,
            data: {}
        };
        setTasks([...tasks, newTask]);
    };

    const updateTask = (index, updatedTask) => {
        const newTasks = [...tasks];
        newTasks[index] = updatedTask;
        setTasks(newTasks);
    };

    const deleteTask = (index) => {
        setTasks(tasks.filter((_, i) => i !== index));
    };

    const convertTasks = (tasks) => {

        let convertedTasks = [];

        for (let i = 0; i < tasks.length; i++) {
            var task = tasks[i];
            if (task.operationId === 1) {
                task.data = {
                    SourceDir: task.data.sourceDir,
                    DestDir: task.data.destDir
                }

                convertedTasks.push(task);
            }
            else if (task.operationId === 2) {

                var generateStoryFromSummaryTask = {
                    id: crypto.randomUUID(),
                    name: "",
                    operationId: 34,
                    order: tasks.length + 1,
                    data: {
                        Summary: {
                            Id: task.data.summaryData.id,
                            Title: task.data.summaryData.title,
                            All: true
                        }
                    }
                };

                convertedTasks.push(generateStoryFromSummaryTask);

                var generateSummaryJob = {
                    id: crypto.randomUUID(),
                    name: "",
                    operationId: 31,
                    order: tasks.length + 1,
                    data: {}
                };

                convertedTasks.push(generateSummaryJob);
            }
            else if (task.operationId === 3) {

                var getLastEditedRPGTask = {
                    id: crypto.randomUUID(),
                    name: "",
                    operationId: 33,
                    order: tasks.length + 1,
                    data: {}
                };

                convertedTasks.push(getLastEditedRPGTask);

                var generateSummaryJob = {
                    id: crypto.randomUUID(),
                    name: "",
                    operationId: 31,
                    order: tasks.length + 1,
                    data: {}
                };
                
                convertedTasks.push(generateSummaryJob);
            }
        }

        return convertedTasks;
    };

    return (
        <Paper sx={{ p: 3 }}>
            <form onSubmit={handleSubmit(onSubmitInternal)}>
                <Typography variant="h5" mb={2}>{t("automations.site_title")}</Typography>

                <Box display="flex" flexDirection="column" gap={2}>
                    <Grid container spacing={2} alignItems="flex-start">
                        <Grid size={{ xs: 12 }}>
                            <Controller
                                name="title"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label={t("automations.title")}
                                        fullWidth
                                        margin="dense"
                                        variant="outlined"
                                        InputProps={{
                                            style: { color: textColor }
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2} alignItems="flex-start">
                        <Grid size={{ xs: 12 }}>
                            <Controller
                                name="description"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label={t("automations.description")}
                                        fullWidth
                                        margin="dense"
                                        variant="outlined"
                                        InputProps={{
                                            style: { color: textColor }
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2} alignItems="flex-start">
                        <Grid size={{ xs: 12 }}>
                            <Controller
                                name="active"
                                control={control}
                                render={({ field: { onChange, value } }) => (
                                    <FormControlLabel
                                        control={<Switch checked={value} onChange={onChange} />}
                                        label={t("automations.active")}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>

                    {/* TRIGGERS SECTION */}
                    <Typography variant="h6">{t("automations.triggers.title")}</Typography>
                    {triggers.map((tr, index) => (
                        <Paper key={tr.id} sx={{ p: 2, mb: 2 }}>
                            <Box display="flex" justifyContent="space-between" alignItems="center">
                                <Typography>Trigger #{index + 1}</Typography>
                                <IconButton onClick={() => deleteTrigger(index)}><DeleteIcon /></IconButton>
                            </Box>

                            <TextField
                                sx={{ mt: 2 }}
                                fullWidth
                                label={t("automations.title")}
                                value={tr.name}
                                onChange={(e) => updateTrigger(index, { ...tr, name: e.target.value })}
                            />

                            <TextField
                                sx={{ mt: 2 }}
                                fullWidth
                                label={t("automations.description")}
                                value={tr.description}
                                multiline
                                onChange={(e) => updateTrigger(index, { ...tr, description: e.target.value })}
                            />

                            <FormControl fullWidth sx={{ mt: 2 }}>
                                <InputLabel>{t("automations.triggers.type")}</InputLabel>
                                <Select
                                    value={tr.type}
                                    label={t("automations.triggers.type")}
                                    onChange={(e) => updateTrigger(index, { ...tr, type: e.target.value })}
                                >
                                    {triggerTypes.map((tItem) => (
                                        <MenuItem key={tItem.id} value={tItem.id}>{t(tItem.label)}</MenuItem>
                                    ))}
                                </Select>
                            </FormControl>

                            {tr.type === 1 && (
                                <TextField
                                    label={t("automations.triggers.cron")}
                                    sx={{ mt: 2 }}
                                    value={tr.cron}
                                    onChange={(e) => updateTrigger(index, { ...tr, cron: e.target.value })}
                                />
                            )}
                        </Paper>
                    ))}

                    <Button variant="outlined" onClick={addTrigger}><GridAddIcon /></Button>

                    {/* TASKS SECTION */}
                    <Typography variant="h6" mt={3}>{t("automations.tasks")}</Typography>
                    {tasks.map((task, index) => (
                        <Paper key={task.id} sx={{ p: 2, mb: 2 }}>
                            <Box display="flex" justifyContent="space-between" alignItems="center">
                                <Typography>Task #{index + 1}</Typography>
                                <IconButton onClick={() => deleteTask(index)}><DeleteIcon /></IconButton>
                            </Box>


                            <FormControl fullWidth sx={{ mt: 2 }}>
                                <InputLabel>{t("automat.trigger.type")}</InputLabel>
                                <Select value={task.operationId} label={t("automat.trigger.type")} onChange={(e) => updateTask(index, { ...task, operationId: e.target.value })}>
                                    {taskTypes.map((tItem) => (
                                        <MenuItem key={tItem.id} value={tItem.id}>{t(tItem.label)}</MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            {task.operationId === 1 && <ArchiveTaskForm task={task} onChange={(t) => updateTask(index, t)} />}
                            {task.operationId === 2 && <SummaryTaskForm task={task} onChange={(t) => updateTask(index, t)} />}
                            {task.operationId === 3 && <SummaryLastRPGTaskForm task={task} onChange={(t) => updateTask(index, t)} />}
                        </Paper>
                    ))}


                    <Button variant="contained" onClick={addTask}><GridAddIcon /></Button>
                    <Button variant="contained" color="success" type="submit">{t("opt.save")}</Button>
                </Box>
            </form>
        </Paper>
    );
};