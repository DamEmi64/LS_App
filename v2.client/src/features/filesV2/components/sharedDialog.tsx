import { useEffect, useState } from "react";
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

enum Privilage {
    OWNER = 1,
    READ = 2,
    WRITE = 3
}

interface ShareDialogProps {
  file: FileV2Dto | null;
  onClose: () => void;
  onFileUpdated: (file: FileV2Dto) => void;
}

export default function ShareDialog({ file, onClose, onFileUpdated }: ShareDialogProps) {
  const [users, setUsers] = useState<FileUserDto[]>([]);
  const [login, setLogin] = useState("");
  const [privilage, setPrivilage] = useState<Privilage>(Privilage.READ);
  const [isPublic, setIsPublic] = useState(false);

  useEffect(() => {
    if (!file) return;
    setIsPublic(file.public);
    call<FileUserDto[]>(api => api.filesV2Api.getByIdUsers,{id: file.id}).then(setUsers);
  }, [file]);

  if (!file) return null;

  const handleTogglePublic = async (checked: boolean) => {
    setIsPublic(checked);
    const updated = await call<FileV2Dto>(api => api.filesV2Api.updateById,{id: file.id,updateFileDto: { public: checked } });
    onFileUpdated(updated);
  };

  return (
    <Dialog open={!!file} onClose={onClose} fullWidth maxWidth="xs">
      <DialogTitle sx={{ display: "flex", alignItems: "center", pr: 1 }}>
        <Box sx={{ flex: 1 }}>Share "{file.title}"</Box>
        <IconButton onClick={onClose} size="small">
          <CloseRounded fontSize="small" />
        </IconButton>
      </DialogTitle>

      <DialogContent>
        <Box sx={{ display: "flex", gap: 1, mb: 2 }}>
          <TextField
            size="small"
            fullWidth
            placeholder="Add person by login"
            value={login}
            onChange={(e) => setLogin(e.target.value)}
          />
          <Select
            size="small"
            value={privilage}
            onChange={(e) => setPrivilage(e.target.value as Privilage)}
          >
            <MenuItem value={Privilage.READ}>Can view</MenuItem>
            <MenuItem value={Privilage.WRITE}>Can edit</MenuItem>
          </Select>
          <Button variant="contained">
            Add
          </Button>
        </Box>

        <List dense disablePadding>
          {users.map((u) => (
            <ListItem
              key={u.userId}
              secondaryAction={
                u.privilage !== Privilage.OWNER && (
                  <IconButton size="small">
                    <DeleteOutlineRounded fontSize="small" />
                  </IconButton>
                )
              }
            >
              <ListItemText
                primary={u.login}
                secondary={u.privilage === Privilage.OWNER ? "Owner" : u.privilage === Privilage.READ ? "Can edit" : "Can view"}
              />
            </ListItem>
          ))}
          {users.length === 0 && (
            <Typography variant="body2" color="text.secondary" sx={{ py: 1 }}>
              Only you have access.
            </Typography>
          )}
        </List>

        <Divider sx={{ my: 2 }} />

        <FormControlLabel
          control={<Switch checked={isPublic} onChange={(e) => handleTogglePublic(e.target.checked)} />}
          label={
            <Box>
              <Typography variant="body2">Anyone with the link can view</Typography>
            </Box>
          }
        />
      </DialogContent>

      <DialogActions>
        <Button onClick={onClose}>Done</Button>
      </DialogActions>
    </Dialog>
  );
}