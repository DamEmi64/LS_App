import { useEffect, useState } from "react";
import { useForm, Controller } from "react-hook-form";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Divider,
  FormControlLabel,
  IconButton,
  List,
  ListItem,
  ListItemText,
  MenuItem,
  Select,
  Switch,
  TextField,
  Typography,
} from "@mui/material";
import CloseRounded from "@mui/icons-material/CloseRounded";
import DeleteOutlineRounded from "@mui/icons-material/DeleteOutlineRounded";
import {call} from "@/shared";
import { FileV2Dto, FileUserDto } from "@/shared/api/generated";
import { t } from "i18next";

enum Privilage {
    OWNER = 1,
    READ = 2,
    WRITE = 3
}

interface ShareFormData {
  login: string;
}

interface FileEditFormData {
  title: string;
  description: string;
}

interface FileDialogProps {
  file: FileV2Dto | null;
  onSubmit: (file: FileV2Dto) => void;
  onClose: () => void;
}

export default function FileDialog({ file, onClose, onSubmit }: FileDialogProps) {
  const [users, setUsers] = useState<FileUserDto[]>([]);
  const [privilage, setPrivilage] = useState<Privilage>(Privilage.READ);
  const [isPublic, setIsPublic] = useState(false);
  const [isSaving, setIsSaving] = useState(false);

  // File metadata form
  const {
    control: editControl,
    handleSubmit: handleEditSubmit,
    reset: resetEdit,
    formState: { errors: editErrors },
  } = useForm<FileEditFormData>({
    defaultValues: {
      title: "",
      description: "",
    },
  });

  // Share form
  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ShareFormData>({
    defaultValues: {
      login: "",
    },
  });

  useEffect(() => {
    if (!file) {
      resetEdit();
      reset();
      return;
    }
    resetEdit({
      title: file.title || "",
      description: file.description || "",
    });
    setIsPublic(file.public);
    call<FileUserDto[]>(api => api.filesV2Api.getByIdUsers, { id: file.id }).then(setUsers);
  }, [file, resetEdit, reset]);

  if (!file) return null;

  const handleEditFile = async (formData: FileEditFormData) => {
    if (!file) return;
    
    try {
      setIsSaving(true);
      const updated = await call<FileV2Dto>(
        api => api.filesV2Api.updateById,
        {
          id: file.id,
          updateFileDto: {
            title: formData.title,
            description: formData.description,
          },
        }
      );
      onSubmit(updated);
    } catch (error) {
      console.error("Failed to update file:", error);
    } finally {
      setIsSaving(false);
    }
  };

  const handleAddPrivilage = async (formData: ShareFormData) => {
    if (!file) return;
    
    try {
      setIsSaving(true);
      // Add new user with privilege
      await call(api => api.filesV2Api.createByIdUsers, {
        id: file.id,
        createFileUserDto: {
          login: formData.login,
          privilage: privilage,
        },
      });
      
      // Refresh users list
      const updatedUsers = await call<FileUserDto[]>(
        api => api.filesV2Api.getByIdUsers,
        { id: file.id }
      );
      setUsers(updatedUsers);
      reset(); // Clear form
      setPrivilage(Privilage.READ); // Reset privilege to default
    } catch (error) {
      console.error("Failed to add user privilege:", error);
    } finally {
      setIsSaving(false);
    }
  };

  const handleRemovePrivilage = async (userId: string) => {
    if (!file) return;
    
    try {
      setIsSaving(true);
      await call(api => api.filesV2Api.deleteByIdUsersByUserId, {
        id: file.id,
        userId: userId,
      });
      
      // Refresh users list
      const updatedUsers = await call<FileUserDto[]>(
        api => api.filesV2Api.getByIdUsers,
        { id: file.id }
      );
      setUsers(updatedUsers);
    } catch (error) {
      console.error("Failed to remove user privilege:", error);
    } finally {
      setIsSaving(false);
    }
  };

  const handleTogglePublic = async (checked: boolean) => {
    try {
      setIsSaving(true);
      setIsPublic(checked);
      const updated = await call<FileV2Dto>(
        api => api.filesV2Api.updateById,
        { id: file.id, updateFileDto: { public: checked } }
      );
      onSubmit(updated);
    } catch (error) {
      console.error("Failed to update public status:", error);
      setIsPublic(!checked); // Revert on error
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <Dialog open={!!file} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle sx={{ display: "flex", alignItems: "center", pr: 1 }}>
        <Box sx={{ flex: 1 }}>{t('files.name')}</Box>
        <IconButton onClick={onClose} size="small" disabled={isSaving}>
          <CloseRounded fontSize="small" />
        </IconButton>
      </DialogTitle>

      <DialogContent>
        <form onSubmit={handleEditSubmit(handleEditFile)}>
          <Box sx={{ mb: 3 }}>
            <Typography variant="h6" sx={{ mb: 2 }}>
              {t('window.info')}
            </Typography>

            {/* Title field */}
            <Controller
              name="title"
              control={editControl}
              rules={{ required: t('validation.required') as string }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label={t('files.name')}
                  fullWidth
                  margin="dense"
                  error={!!editErrors.title}
                  helperText={editErrors.title?.message}
                  disabled={isSaving}
                />
              )}
            />

            {/* Description field */}
            <Controller
              name="description"
              control={editControl}
              render={({ field }) => (
                <TextField
                  {...field}
                  label={t('rpg.story.description')}
                  multiline
                  minRows={3}
                  maxRows={8}
                  fullWidth
                  margin="dense"
                  error={!!editErrors.description}
                  helperText={editErrors.description?.message}
                  disabled={isSaving}
                />
              )}
            />

            {/* Save file metadata button */}
            <Box sx={{ display: "flex", gap: 2, mt: 2 }}>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                disabled={isSaving}
              >
                {t('opt.save')}
              </Button>
            </Box>
          </Box>
        </form>

        <Divider sx={{ my: 2 }} />

        {/* Share section */}
        <Typography variant="h6" sx={{ mb: 2 }}>
          {t('files.share.public')}
        </Typography>

        <form onSubmit={handleSubmit(handleAddPrivilage)}>
          <Box sx={{ display: "flex", gap: 1, mb: 2 }}>
            <Controller
              name="login"
              control={control}
              rules={{ required: t('validation.required') as string }}
              render={({ field }) => (
                <TextField
                  {...field}
                  size="small"
                  fullWidth
                  placeholder={t('files.share.user')}
                  error={!!errors.login}
                  helperText={errors.login?.message}
                  disabled={isSaving}
                />
              )}
            />
            <Select
              size="small"
              value={privilage}
              onChange={(e) => setPrivilage(e.target.value as Privilage)}
              disabled={isSaving}
            >
              <MenuItem value={Privilage.READ}>{t('files.share.read')}</MenuItem>
              <MenuItem value={Privilage.WRITE}>{t('files.share.write')}</MenuItem>
            </Select>
            <Button
              variant="contained"
              type="submit"
              disabled={isSaving}
            >
              {t('opt.add')}
            </Button>
          </Box>
        </form>

        <List dense disablePadding>
          {users.map((u) => (
            <ListItem
              key={u.userId}
              secondaryAction={
                u.privilage !== Privilage.OWNER && (
                  <IconButton
                    size="small"
                    onClick={() => handleRemovePrivilage(u.userId)}
                    disabled={isSaving}
                  >
                    <DeleteOutlineRounded fontSize="small" />
                  </IconButton>
                )
              }
            >
              <ListItemText
                primary={u.login}
                secondary={
                  u.privilage === Privilage.OWNER
                    ? t('files.share.owner')
                    : u.privilage === Privilage.READ
                    ? t('files.share.read')
                    : t('files.share.write')
                }
              />
            </ListItem>
          ))}
        </List>

        <Divider sx={{ my: 2 }} />

        <FormControlLabel
          control={
            <Switch
              checked={isPublic}
              onChange={(e) => handleTogglePublic(e.target.checked)}
              disabled={isSaving}
            />
          }
          label={
            <Box>
              <Typography variant="body2">{t('files.share.public')}</Typography>
            </Box>
          }
        />
      </DialogContent>

      <DialogActions>
        <Button onClick={onClose} disabled={isSaving}>
          {t('opt.close')}
        </Button>
      </DialogActions>
    </Dialog>
  );
}