import React, { useEffect, useState } from "react";
import { Box, Button, Grid, MenuItem, TextField, Typography, useTheme } from "@mui/material";
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { ColumnType, TableColumn } from "@/shared";
import { GridTable } from "@/shared/components/datatables/gridTable";
import { ImageProvider } from "@/shared/components/imageProvider";
import { EventBody, EventComponentProps, EventParticipant } from "../types";
import { getDictionary, useDictionaryTranslation } from "@/lib/utils";
import { DateTimePicker  } from "@mui/x-date-pickers";
import dayjs from "dayjs";

const emptyEvent: EventBody = {
  title: "",
  description: "",
  image: "",
  imageContent: "",
  participants: [],
  eventDate: new Date().toISOString(),
  category: 0
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
  const [image, setImage] = useState(initialData.imageContent);

  const categories = getDictionary("Event categories");
  const translateDictonary = useDictionaryTranslation();

  const {
    control,
    handleSubmit,
    reset,
    setValue,
    formState: { errors },
  } = useForm<EventBody>({
    defaultValues: initialData,
  });

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
    }
  ];

  useEffect(() => {
    reset(initialData);
    setImage(initialData.imageContent);
  }, [event]);

  const onSubmit = (data: EventBody) => {
    onSave?.({
      ...data,
      imageContent: image,
      participants: initialData.participants,
    });
    if (!isNew) setIsEditing(false);
  };

  const cancelEdit = () => {
    reset(initialData);
    setImage(initialData.imageContent);
    setIsEditing(false);
  };

  const updateImage = (data: string) => {
    setImage(data);
    setValue("imageContent", data);
  };

  const canEdit = !readonly;

  return (
    <Grid container spacing={3} alignItems="flex-start">
      <Grid
        size={{ xs: 12, md: 4 }}
        sx={{ display: "flex", justifyContent: "flex-start" }}
      >
        <ImageProvider
          imageId={initialData.image || ""}
          readonly={!isEditing || readonly}
          saveImage={updateImage}
        />
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
              name="category"
              control={control}
              rules={{ required: t("validation.required") as string }}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  label={t("events.category")}
                  fullWidth
                  margin="dense"
                  variant="outlined"
                  error={!!errors.category}
                  helperText={errors.category?.message}
                  SelectProps={{
                    native: false,
                  }}
                >
                  {categories.map((category) => (
                    <MenuItem key={category.key} value={category.key}>
                      {translateDictonary("Event categories", category.key).title}
                    </MenuItem>
                  ))}
                </TextField>
              )}
            />
            <Controller
              name="eventDate"
              control={control}
              rules={{ required: t("validation.required") as string }}
              render={({ field }) => (
                <DateTimePicker
                  label={t("events.eventDate")}
                  value={field.value ? dayjs(field.value) : null}
                  onChange={(date) => field.onChange(date)}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      margin: "dense",
                      variant: "outlined",
                      error: !!errors.eventDate,
                      helperText: errors.eventDate?.message,
                      InputProps: {
                        style: { color: textColor },
                      },
                    },
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
              {t('events.category')}: {translateDictonary("Event categories", initialData.category)?.title}
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
      {!isEditing && (
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
      </Grid>)}
    </Grid>
  );
};

export default EventComponent;
