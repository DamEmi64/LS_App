import { useEffect, useState } from "react";
import { useForm, Controller } from "react-hook-form";
import {
  Autocomplete,
  Box,
  Button,
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
  useTheme,
} from "@mui/material";
import DeleteOutlineRounded from "@mui/icons-material/DeleteOutlineRounded";
import { t } from "i18next";
import { ShareFormData, Privilage, FileUser, Directory, PrivilageToSend } from "../types";
import { UserData } from "@/features/system";
import { grantDirAccess, loadDirUsers, loadShareableUsers, revokeDirAccess, setDirPublicStatus } from "../services/directoryService";

interface ShareFolderWrapperProps {
  directory: Directory | null;
  onSubmit: (directory: Directory) => void;
}

export default function ShareFolderWrapper({ directory, onSubmit }: ShareFolderWrapperProps) {
  const [users, setUsers] = useState<FileUser[]>([]);
  const [privilage, setPrivilage] = useState<Privilage>(Privilage.READ);
  const [isPublic, setIsPublic] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [serverUsers, setServerUsers] = useState<UserData[]>([]);

  // Share form
  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ShareFormData>({
    defaultValues: {
      user: null,
    },
  });

  const theme = useTheme();
  useEffect(() => {
    if (!directory) {
      reset();
      return;
    }

    setIsPublic(directory.public);
    loadDirUsers(directory.id).then(setUsers);
    loadShareableUsers().then(response => setServerUsers(response.data));
  }, [directory, reset]);

  if (!directory) return null;

  const handleAddPrivilage = async (formData: ShareFormData) => {
    if (!directory) return;

    try {
      setIsSaving(true);
      // Add new user with privilege
      await grantDirAccess(directory.id, formData.user.login, formData.user.userId, mapToPrivilageToSend(privilage));

      // Refresh users list
      const updatedUsers = await loadDirUsers(directory.id);
      setUsers(updatedUsers);
      reset(); // Clear form
      setPrivilage(Privilage.NONE); // Reset privilege to default
    } catch (error) {
      console.error("Failed to add user privilege:", error);
    } finally {
      setIsSaving(false);
    }
  };

  const mapToPrivilageToSend = (privilage: Privilage) => {
    if (privilage == Privilage.OWNER)
      return PrivilageToSend.OWNER;
    if (privilage == Privilage.READ)
      return PrivilageToSend.READ;
    if (privilage == Privilage.WRITE)
      return PrivilageToSend.WRITE;

    return PrivilageToSend.NONE;
  }

  const handleTogglePublic = async (checked: boolean) => {
    try {
      setIsSaving(true);
      setIsPublic(checked);
      const updated = await setDirPublicStatus(directory.id, checked);
      onSubmit(updated);
    } catch (error) {
      console.error("Failed to update public status:", error);
      setIsPublic(!checked); // Revert on error
    } finally {
      setIsSaving(false);
    }
  };

  const handleRemovePrivilage = async (userId: string) => {
    if (!directory) return;

    try {
      setIsSaving(true);
      await revokeDirAccess(directory.id, userId);

      // Refresh users list
      const updatedUsers = await loadDirUsers(directory.id);
      setUsers(updatedUsers);
    } catch (error) {
      console.error("Failed to remove user privilege:", error);
    } finally {
      setIsSaving(false);
    }
  };


  return (
    <>
      <form onSubmit={handleSubmit(handleAddPrivilage)}>
        <Box sx={{ display: "flex", gap: 1, mb: 2 }}>
          <Controller
            name="user"
            control={control}
            rules={{ required: t("validation.required") as string }}
            render={({ field }) => (
              <Autocomplete
                sx={{ width: '200px' }}
                options={serverUsers}
                value={serverUsers.find(x => x.userId == field?.value?.userId) || null}
                onChange={(_, value) => field.onChange(value)}
                getOptionLabel={(option) => option.login}
                isOptionEqualToValue={(option, value) => option.userId === value.userId}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    size="small"
                    fullWidth
                    label="Login"
                    error={!!errors.user}
                    helperText={errors.user?.message}
                  />
                )}
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
              style={{color: theme.palette.text.primary}}
              primary={u.login}
              secondary={
                u.privilage == Privilage.OWNER
                  ? t('files.share.owner')
                  : u.privilage == Privilage.READ
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
    </>
  );
}
