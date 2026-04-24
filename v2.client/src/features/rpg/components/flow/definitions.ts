import { Edge, Node } from "reactflow";

export interface RPGNodeData {
  title: string;
  description?: string;
  condition?: string;
  status?: NodeStatus;
  kind?: NodeTypeKind;
  editable?: boolean;
  onEdit?: () => void;
  visited?: boolean;
}

export interface RPGFlowProps {
  readonly?: boolean;
  initialNodes?: Node<RPGNodeData>[];
  initialEdges?: Edge[];
  onSave?: (data: { nodes: Node<RPGNodeData>[]; edges: Edge[] }) => void;
}

// ---------------- CONSTANTS ----------------
export const STATUS_COLORS: Record<NodeStatus, string> = {
  default: "#555",
  success: "#2e7d32",
  additional: "#ed6c02",
  fail: "#d32f2f",
};

// ---------------- TYPES ----------------
export type NodeStatus = "default" | "success" | "additional" | "fail";
export type NodeTypeKind = "quest" | "important" | "event";
