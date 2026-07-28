import { UserData } from "../system";

export interface FileItem {
    id: string,
    name: string;
    type: 'file' | 'folder'
    icon?: React.ReactNode;
    onClick: () => void;
    onDetails?: () => void;
    onEdit?: () => void;
    onDelete?: () => void;
    privilage: Privilage
}

export enum Privilage {
    OWNER = 0,
    READ = 1,
    WRITE = 2,
    NONE = 3
}

export interface ShareFormData {
  user: UserData
}

export interface FileEditFormData {
  title: string;
  description: string;
  File: File;
}

export interface Directory {
    'id'?: string;
    'title'?: string | null;
    'parentId'?: string | null;
    'childDirectoryCount'?: number;
    'fileCount'?: number;
}

export interface FileV2 {
      'id'?: string;
      'title'?: string | null;
      'description'?: string | null;
      'owner'?: string | null;
      'public'?: boolean;
      'directoryId'?: string | null;
      'path'?: string | null;
      'fileUsers'?: Array<FileUser> | null;
}

export interface FileUser {
      'userId': string | null;
      'login'?: string | null;
      'privilage'?: Privilage;
}

export interface GrantAccess {
    'login': string | null;
    'privilage'?: Privilage;
}