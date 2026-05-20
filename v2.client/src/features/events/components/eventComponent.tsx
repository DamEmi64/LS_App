import React, { useEffect, useState } from "react";
import { Box, Button, Grid, TextField, Typography, useTheme } from "@mui/material";
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { ColumnType, TableColumn } from "@/shared";
import { GridTable } from "@/shared/components/datatables/gridTable";
import { ImageProvider } from "@/shared/components/imageProvider";
import { EventBody, EventComponentProps, EventParticipant } from "../types";

const emptyEvent: EventBody = {
  title: "",
  description: "",
  image: "",
  participants: [],
};

export const EventComponent: React.FC<EventComponentProps> = ({
  event,
  onSave,
  onDelete,
  isEdit = false,
  isNew = false,
  readonly = false,
}) => {
  const { t } = useTranslation();
  const theme = useTheme();
  const textColor =
    theme.palette.mode === "dark"
      ? theme.palette.text.primary
      : theme.palette.text.secondary;

  const initialData = { ...emptyEvent, ...event };
  const [isEditing, setIsEditing] = useState(isNew || isEdit);
  const [previewImageId, setPreviewImageId] = useState(initialData.image);

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<EventBody>({
    defaultValues: initialData,
  });

  const participantColumns: TableColumn<EventParticipant>[] = [
    { field: "id", header: "events.participants.id", type: ColumnType.String },
    {
      field: "login",
      header: "events.participants.login",
      type: ColumnType.String,
    },
  ];

  useEffect(() => {
    reset(initialData);
    setPreviewImageId(initialData.image);
  }, [event]);

  const onSubmit = (data: EventBody) => {
    onSave?.({
      ...data,
      participants: initialData.participants,
    });
    if (!isNew) setIsEditing(false);
  };

  const cancelEdit = () => {
    reset(initialData);
    setPreviewImageId(initialData.image);
    setIsEditing(false);
  };

  const canEdit = !readonly;

  return (
    <Grid container spacing={3} alignItems="flex-start">
      <Grid
        size={{ xs: 12, md: 4 }}
        sx={{ display: "flex", justifyContent: "flex-start" }}
      >
        <ImageProvider imageId={previewImageId || ""} readonly />
      </Grid>

      <Grid size={{ xs: 12, md: 8 }}>
        {isEditing && canEdit ? (
          <form onSubmit={handleSubmit(onSubmit)}>
            <Controller
              name="title"
              control={control}
              rules={{ required: t("validation.required") as string }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label={t("events.title")}
                  fullWidth
                  margin="dense"
                  variant="outlined"
                  error={!!errors.title}
                  helperText={errors.title?.message}
                  InputProps={{ style: { color: textColor } }}
                />
              )}
            />

            <Controller
              name="image"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label={t("events.image")}
                  fullWidth
                  margin="dense"
                  variant="outlined"
                  helperText={t("events.imageExternalKey")}
                  InputProps={{ style: { color: textColor } }}
                  onChange={(changeEvent) => {
                    field.onChange(changeEvent.target.value);
                    setPreviewImageId(changeEvent.target.value);
                  }}
                />
              )}
            />

            <Controller
              name="description"
              control={control}
              rules={{ required: t("validation.required") as string }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label={t("events.description")}
                  multiline
                  minRows={4}
                  maxRows={16}
                  fullWidth
                  margin="dense"
                  variant="outlined"
                  error={!!errors.description}
                  helperText={errors.description?.message}
                  InputProps={{ style: { color: textColor } }}
                />
              )}
            />

            <Box sx={{ display: "flex", gap: 2, mt: 3 }}>
              <Button type="submit" variant="contained" color="success">
                {t("opt.save")}
              </Button>
              {!isNew && (
                <Button
                  type="button"
                  variant="outlined"
                  color="error"
                  onClick={cancelEdit}
                >
                  {t("opt.cancel")}
                </Button>
              )}
            </Box>
          </form>
        ) : (
          <>
            <Typography variant="h4" component="h2" sx={{ color: textColor, mb: 2 }}>
              {initialData.title}
            </Typography>

            <Typography sx={{ color: textColor, whiteSpace: "pre-line" }}>
              {initialData.description}
            </Typography>

            {canEdit && (
              <Box sx={{ display: "flex", gap: 2, mt: 3 }}>
                <Button
                  type="button"
                  onClick={() => setIsEditing(true)}
                  variant="contained"
                >
                  {t("opt.edit")}
                </Button>

                {onDelete && (
                  <Button
                    type="button"
                    onClick={() => onDelete(initialData)}
                    variant="contained"
                    color="error"
                  >
                    {t("opt.delete")}
                  </Button>
                )}
              </Box>
            )}
          </>
        )}
      </Grid>

      <Grid size={{ xs: 12 }}>
        <Typography variant="h6" sx={{ color: textColor, mb: 2 }}>
          {t("events.participants.title")}
        </Typography>
        <GridTable
          columns={participantColumns}
          data={{
            data: initialData.participants || [],
            total: initialData.participants?.length || 0,
          }}
          canDelete={false}
          readonly
        />
      </Grid>
    </Grid>
  );
};

export default EventComponent;
