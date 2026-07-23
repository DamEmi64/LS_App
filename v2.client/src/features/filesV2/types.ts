export interface FileItem {
    id: string,
    name: string;
    type: 'file' | 'folder'
    icon?: React.ReactNode;
    onClick: () => void;
    onDetails?: () => void;
    onEdit?: () => void;
    onDelete?: () => void;
}