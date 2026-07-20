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
import { Automat, Trigger } from "@/features/automation";
import { useTheme } from "@mui/material";
import SummaryLastRPGTaskForm from "./Tasks/SummaryLastRPGTaskJob";
import DownloadLastFileForm from "./Tasks/DownloadLastFile";
import SendReminderTaskForm from "./Tasks/SendReminderTaskForm";
import { DictionaryItem, getDictionary, useDictionaryTranslation } from "@/lib/utils";
import GenericTaskForm from "./Tasks/GenericTaskFrom";

const taskTypes = [
    { id: 41, label: "automations.actions.archive", fallback: "Archive files" },
    { id: 31, label: "automations.actions.GenerateRPGSummary", fallback: "Generate RPG summary" },
    { id: 51, label: "automations.actions.sendReminder", fallback: "Send reminder" },
    { id: 52, label: "automations.actions.sendInvitation", fallback: "Send invitation" }
];

const getDefaultTaskData = (operationId: number) => operationId === 51 ? "Min15" : {};

export const AutomatForm = ({ initialData, onSubmit }) => {

    const triggerTypes = getDictionary('Automation events');

    const translateDictionary = useDictionaryTranslation();
    const { t } = useTranslation();
    const theme = useTheme();
    const textColor = theme.palette.text.primary;

    const { control, handleSubmit, formState: { errors } } = useForm<Automat>({ defaultValues: initialData });
    const [tasks, setTasks] = useState(initialData.tasks || []);
    const [triggers, setTriggers] = useState(initialData.triggers || []);

    const addTrigger = () => {
        const newTrigger = {
            id: crypto.randomUUID(),
            cron: "",
            eventId: 110
        };
        setTriggers([...triggers, newTrigger]);
    };

    const updateTrigger = (index, updated: Trigger) => {
        const arr = [...triggers];
        arr[index] = updated;
        setTriggers(arr);
    };

    const deleteTrigger = (index) => {
        setTriggers(triggers.filter((_, i) => i !== index));
    };

    const onSubmitInternal = (data) => {
        onSubmit({ ...data, tasks: tasks, triggers: triggers });
    }

    const addTask = () => {
        const newTask = {
            id: crypto.randomUUID(),
            name: "",
            operationId: 41,
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

        const convertedTasks = [];

        for (let i = 0; i < tasks.length; i++) {
            const task = tasks[i];
            if (task.operationId === 1) {
                task.data = {
                    SourceDir: task.data.sourceDir,
                    DestDir: task.data.destDir
                }

                convertedTasks.push(task);
            }
            else if (task.operationId === 2) {

                const generateStoryFromSummaryTask = {
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

                const getLastEditedRPGTask = {
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
                            <FormControl fullWidth sx={{ mt: 2 }}>
                                <InputLabel>{t("automations.triggers.type")}</InputLabel>
                                <Select
                                    value={tr.eventId}
                                    label={t("automations.triggers.type")}
                                    onChange={(e) => updateTrigger(index, { ...tr, eventId: e.target.value })}
                                >
                                    {triggerTypes.map((tItem) => {
                                        if (tItem.key === '1') {
                                            return (<MenuItem key={tItem.key} value={tItem.key}>{t(tItem.title)}</MenuItem>);
                                        }
                                        else {
                                            return (<MenuItem key={tItem.key} value={tItem.key}>{translateDictionary('Automation events', tItem.key).title}</MenuItem>);
                                        }
                                        
            })}
                                </Select>
                            </FormControl>

                            {(tr.eventId === 110 || tr.eventId === '110') && (
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
                                <InputLabel>{t("automations.triggers.type")}</InputLabel>
                                <Select
                                    value={task.operationId}
                                    label={t("automations.triggers.type")}
                                    onChange={(e) => {
                                        const operationId = Number(e.target.value);
                                        updateTask(index, {
                                            ...task,
                                            operationId,
                                            data: getDefaultTaskData(operationId)
                                        });
                                    }}
                                >
                                    {taskTypes.map((tItem) => (
                                        <MenuItem key={tItem.id} value={tItem.id}>{t(tItem.label, tItem.fallback)}</MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            {task.operationId === 41 && <ArchiveTaskForm task={task} onChange={(t) => updateTask(index, t)} />}
                            {task.operationId === 31 && <SummaryLastRPGTaskForm task={task} onChange={(t) => updateTask(index, t)} />}
                            {task.operationId === 51 && <SendReminderTaskForm task={task} onChange={(t) => updateTask(index, t)} />}
                            {task.operationId === 52 && <GenericTaskForm task={task} onChange={(t) => updateTask(index, t)} />}
                        </Paper>
                    ))}


                    <Button variant="contained" onClick={addTask}><GridAddIcon /></Button>
                    <Button variant="contained" color="success" type="submit">{t("opt.save")}</Button>
                </Box>
            </form>
        </Paper>
    );
};
