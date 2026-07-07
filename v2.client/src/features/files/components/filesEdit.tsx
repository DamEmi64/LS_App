import React, { useState } from 'react';
import {
  Box,
  TextField,
  Typography,
  useTheme,
  Grid,
  Button,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  FormHelperText,
  Checkbox,
  FormControlLabel,
  ToggleButton,
  ToggleButtonGroup
} from '@mui/material';
import { EditFile } from '@/features/files';
import { t } from 'i18next';
import noImage from '@/assets/no-image.png';
import { useForm, Controller } from 'react-hook-form';
import { ImageProvider } from '@/shared/components/imageProvider';
import { FilesEditProps } from '@/features/files';  
import { getDictionary, useDictionaryTranslation } from '@/lib/utils';

export const FilesEdit: React.FC<FilesEditProps> = ({ file, toSave }) => {
  const theme = useTheme();
  const textColor = theme.palette.text.primary;

  const getDictionaryTranslation = useDictionaryTranslation();

  const [image, setImage] = useState<string>(file.image || noImage);

  const {
    control,
    handleSubmit,
    watch,
    setValue,
    formState: { errors }
  } = useForm<EditFile>({
    defaultValues: {
      ...file,
      image: file.image || noImage,
      links: file.links || ''
    }
  });

  const fileType = watch('fileType');
  const useFileUpload = watch('useFileUpload');

  const fileTypes = getDictionary('File types');
  const gameGenres = getDictionary('Game genres');
  const sourceTypes = getDictionary('Source types');

  const showStudyFields = fileType === 102;
  const showGameGenre = fileType === 100;

  const onSubmit = (data: EditFile) => {
    data.imageData = image;
    toSave(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Grid container spacing={2} alignItems="flex-start">
        <Grid size={{ xs: 12, md: 8 }}>
          <Typography variant="h6" sx={{ color: textColor, mb: 2 }}>
            {t('window.info')}
          </Typography>

          {/* Title */}
          <Controller
            name="title"
            control={control}
            rules={{ required: t('validation.required') as string }}
            render={({ field }) => (
              <TextField
                {...field}
                label={t('files.name')}
                fullWidth
                margin="dense"
                variant="outlined"
                error={!!errors.title}
                helperText={errors.title?.message}
                InputProps={{ style: { color: textColor } }}
              />
            )}
          />

          {/* Upload toggle */}
          <Controller
            name="useFileUpload"
            control={control}
            render={({ field }) => (
              <FormControlLabel
                control={
                  <Checkbox
                    {...field}
                    checked={field.value}
                    color="primary"
                    onChange={(e) => {
                      const checked = e.target.checked;
                      field.onChange(checked);
                      if (checked) setValue('locaction', '');
                      else setValue('content', '');
                    }}
                  />
                }
                label={t('files.useFileUpload')}
              />
            )}
          />

          {useFileUpload ? (
            <Controller
              name="content"
              control={control}
              rules={{
                required: useFileUpload ? (t('validation.required') as string) : false
              }}
              render={({ field }) => (
                <div>
                  <input
                    type="file"
                    aria-label="content"
                    onChange={(e) => {
                      const file = e.target.files?.[0];
                      if (!file) {
                        setValue('content', '');
                        field.onChange('');
                        return;
                      }
                      const reader = new FileReader();
                      reader.onload = () => {
                        const base64 = reader.result as string;
                        setValue('content', base64);
                        field.onChange(base64);
                      };
                      reader.readAsDataURL(file);
                    }}
                  />
                  {errors.content && (
                    <FormHelperText error>{errors.content.message}</FormHelperText>
                  )}
                </div>
              )}
            />
          ) : (
            <Controller
              name="locaction"
              control={control}
              rules={{
                required: !useFileUpload ? (t('validation.required') as string) : false
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label={t('files.location')}
                  fullWidth
                  margin="dense"
                  variant="outlined"
                  error={!!errors.locaction}
                  helperText={errors.locaction?.message}
                  InputProps={{ style: { color: textColor } }}
                />
              )}
            />
          )}

          {/* FileType */}
          <Controller
            name="fileType"
            control={control}
            rules={{ required: t('validation.required') as string }}
            render={({ field }) => (
              <ToggleButtonGroup
                {...field}
              >
                {fileTypes.map((type) => (
                  <ToggleButton key={type.key} value={type.key} aria-label={type.title}>
                    {getDictionaryTranslation('File types', type.key).title}
                  </ToggleButton>
                ))}
              </ToggleButtonGroup>
            )}
          />

          {/* Additional Data */}
          <Typography variant="subtitle1" sx={{ color: textColor, mt: 2 }}>
            {t('files.additionalData')}
          </Typography>

          <Grid container spacing={2}>
            { showGameGenre && (
              <Grid size={{ xs: 6}}>
                <Controller
                  name="gameGenre"
                  control={control}
                  render={({ field }) => (
                    <FormControl fullWidth margin="dense" error={!!errors.gameGenre}>
                      <InputLabel>{t('files.gameGenre')}</InputLabel>
                      <Select {...field}>
                        {gameGenres.map((genre) => (
                          <MenuItem key={genre.key} value={genre.key}>
                            {getDictionaryTranslation('Game genres', genre.key).title}
                          </MenuItem>
                        ))}
                      </Select>
                      {errors.gameGenre && <FormHelperText>{errors.gameGenre.message}</FormHelperText>}
                    </FormControl>
                  )}
                />
              </Grid>
            )}

            {showStudyFields && (
              <>
                <Grid size={{ xs: 6}}>
                  <Controller
                    name="subject"
                    control={control}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        label={t('files.subject')}
                        fullWidth
                        margin="dense"
                        variant="outlined"
                        error={!!errors.subject}
                        helperText={errors.subject?.message}
                        InputProps={{ style: { color: textColor } }}
                      />
                    )}
                  />
                </Grid>
                <Grid size={{ xs: 6}}>
                  <Controller
                    name="year"
                    control={control}
                    rules={{ min: { value: 1, message: t('validation.min') } }}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        type="number"
                        label={t('files.year')}
                        fullWidth
                        margin="dense"
                        variant="outlined"
                        error={!!errors.year}
                        helperText={errors.year?.message}
                        InputProps={{ style: { color: textColor } }}
                      />
                    )}
                  />
                </Grid>
                <Grid size={{ xs: 12, md: 8 }}>
                  <Controller
                    name="semester"
                    control={control}
                    rules={{ min: { value: 1, message: t('validation.min') } }}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        type="number"
                        label={t('files.semester')}
                        fullWidth
                        margin="dense"
                        variant="outlined"
                        error={!!errors.semester}
                        helperText={errors.semester?.message}
                        InputProps={{ style: { color: textColor } }}
                      />
                    )}
                  />
                </Grid>
              </>
            )}
          </Grid>

          {/* Sources */}
          <Typography variant="subtitle1" sx={{ color: textColor, mt: 2 }}>
            {t('files.sources')}
          </Typography>

          <Controller
            name="sourceType"
            control={control}
            render={({ field }) => (
              <FormControl fullWidth margin="dense" error={!!errors.sourceType}>
                <InputLabel>{t('files.sourceType')}</InputLabel>
                <Select {...field}>
                  {sourceTypes.map((type) => (
                  <MenuItem key={type.key} value={type.key} aria-label={type.title}>
                    {getDictionaryTranslation('File types', type.key).title}
                  </MenuItem>
                ))}
                </Select>
                {errors.sourceType && <FormHelperText>{errors.sourceType.message}</FormHelperText>}
              </FormControl>
            )}
          />

          <Controller
            name="links"
            control={control}
            render={({ field }) => (
              <TextField
                {...field}
                label={t('files.sources')}
                multiline
                fullWidth
                rows={4}
                margin="dense"
                variant="outlined"
              />
            )}
          />

          {/* Save Button */}
          <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
            <Button type="submit" variant="contained" color="primary">
              {t('opt.save')}
            </Button>
          </Box>
        </Grid>

        {/* Image Picker */}
        <Grid size={{ xs: 12, md: 4 }} sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'flex-start' }}>
          <ImageProvider imageId={file.image} readonly={false} saveImage={setImage} />
        </Grid>
      </Grid>
    </form>
  );
};

export default FilesEdit;

