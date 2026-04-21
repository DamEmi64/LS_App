import { Box, TextField, Select, MenuItem, Button, Checkbox, FormControlLabel, IconButton } from "@mui/material";
import { useState } from "react";
import {Node} from "reactflow";
import { NodeStatus, NodeTypeKind, RPGNodeData } from "./definitions";
import {t} from 'i18next';
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";

export const ProgressEdit = ({
  node,
  onChange,
}: {
  node: Node<RPGNodeData>;
  onChange: (data: RPGNodeData) => void;
}) => {
  const [form, setForm] = useState<RPGNodeData>(node.data);

  const update = (patch: Partial<RPGNodeData>) => {
    const updated = { ...form, ...patch };
    setForm(updated);
    onChange(updated);
  };

  return (
    <Box sx={{ p: 2, minWidth: 300 }}>
      <h3>{t('opt.edit')}</h3>
      <Box display="flex" alignItems="center" justifyContent="space-between" mt={1}>
        <IconButton onClick={() => update({ visited: !form.visited })}>
          {form.visited ? <VisibilityIcon /> : <VisibilityOffIcon />}
        </IconButton>
      </Box>
      <TextField label={t('rpg.flow.title')} fullWidth margin="dense" value={form.title} onChange={(e) => update({ title: e.target.value })} />
      <TextField label={t('rpg.flow.description')} fullWidth margin="dense" value={form.description || ""} onChange={(e) => update({ description: e.target.value })} />
      <TextField label={t('rpg.flow.condition')} fullWidth margin="dense" value={form.condition || ""} onChange={(e) => update({ condition: e.target.value })} />

      <Select fullWidth value={form.status || "default"} onChange={(e) => update({ status: e.target.value as NodeStatus })}>
        <MenuItem value="default">{t('rpg.flow.default_node')}</MenuItem>
        <MenuItem value="success">{t('rpg.flow.success_node')}</MenuItem>
        <MenuItem value="additional">{t('rpg.flow.additional_node')}</MenuItem>
        <MenuItem value="fail">{t('rpg.flow.fail_node')}</MenuItem>
      </Select>

      <Select fullWidth sx={{ mt: 1 }} value={form.kind || "quest"} onChange={(e) => update({ kind: e.target.value as NodeTypeKind })}>
        <MenuItem value="quest">{t('rpg.flow.quest')}</MenuItem>
        <MenuItem value="important">{t('rpg.flow.important')}</MenuItem>
        <MenuItem value="event">{t('rpg.flow.event')}</MenuItem>
      </Select>
    </Box>
  );
};