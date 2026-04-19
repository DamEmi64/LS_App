import { Box, Button } from "@mui/material";
import { Handle, Position } from "reactflow";
import { NodeTypeKind, RPGNodeData, STATUS_COLORS } from "./definitions";
import FlagIcon from "@mui/icons-material/Flag";
import GradeIcon from '@mui/icons-material/Grade';
import EventIcon from "@mui/icons-material/Event";

const KIND_ICON: Record<NodeTypeKind, React.ReactNode> = {
  quest: <FlagIcon fontSize="small" />,
  important: <GradeIcon fontSize="small" />,
  event: <EventIcon fontSize="small" />,
};

export const ProgressNodeView = ({ data }: { data: RPGNodeData }) => (
  <Box
    sx={{
      padding: 1.5,
      borderRadius: 2,
      background: "#1e1e1e",
      color: "white",
      border: `2px solid ${STATUS_COLORS[data.status || "default"]}`,
      minWidth: 160,
      position: "relative",
    }}
  >
    <Handle type="target" position={Position.Top} style={{ width: 16, height: 16 }} />
    <Handle type="source" position={Position.Bottom} style={{ width: 16, height: 16 }} />

    <Box display="flex" alignItems="center" gap={1}>
    {data.kind && KIND_ICON[data.kind]}
      <strong>{data.title || "Untitled"}</strong>
    </Box>

    {data.editable && (
      <Button size="small" variant="contained" onClick={data.onEdit} sx={{ mt: 1 }}>
        Edit
      </Button>
    )}
  </Box>
);
