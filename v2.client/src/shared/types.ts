import { ReactNode } from "react";

export interface Row {
    id: string;
    imageData? : string;
    title?: string;
}

export interface FileItem {
    id: string;
    title: string;
    content: string;
}

export type MapRule = {
    method: string
    map: (input:any) => any
}

export interface NavbarItemProps {
    label: string;
    href: string;
    isActive?: boolean;
    permissions?: string[];
    submenu: NavbarItemProps[];
}

export interface LayoutProps {
    image: string, 
    content: React.FC,
    title: string,
    permissions?: string[],
    menu: NavbarItemProps[],
    allowAnonymous?: boolean;
}

export interface Collection<T> {
    data: T[];
    total: number;
}

export interface YesNoWindowProps {
    message: string;
    open: boolean;
    onClose: () => void;
    yesMethod?: () => void;
    noMethod?: () => void;
    cancelMethod?: () => void;
}

export interface TileContainerProps<T> {
    updateData: (paramsObj: onChangeParams) => Promise<void>;
    filters: FilterItem[];
    data: T[];
    operations: Operations<T>[];
    addData?: () => Promise<void>;
    includeImages?: boolean;
};

export enum FilterType {
    String = 'string',
    Number = 'number',
    Date = 'date',
    Enum = 'enum',
    Boolean = 'boolean',
    DateRange = 'dateRange'
}

export interface FilterOption {
    label: string;
    value: string | number;
}

export interface FilterItem {
    field: string;
    name: string;
    type: FilterType;
    options?: FilterOption[]; // For combo type
}

export interface FilterValue {
    field: string,
    value: any;
}

export interface FilterProps {
    filters: FilterItem[];
    onChange: (values: FilterValue[]) => void;
}

export enum ColumnType {
    String = 'string',
    Number = 'number',
    Date = 'date',
    Enum = 'enum',
    Boolean = 'boolean',
    Progress = 'progress',
    SubString = 'substring'
}

export interface Operations<T> {
    name: string,
    method: (data: T) => void;
    hidden?: (data: T) => boolean;
}

export interface onChangeParams {
    page: number;
    pageSize: number;
    orderBy: string | null;
    order: 'asc' | 'desc';
    filters?: FilterValue[];
}

export interface TableProps<T> {
    columns: TableColumn<T>[],
    filters?: FilterItem[],
    data: TableData<T>,
    operations: Operations<T>[],
    setData: (data: TableData<T>) => void,
    onChange: (params: onChangeParams) => Promise<TableData<T>>;
}

export interface TableData<T> {
    data: T[];
    total: number;
}

export interface TableFilterProps {
    columns: TableColumn<any>[];
    filters: FilterItem[];
    onFilterChange: (filters: FilterItem[]) => void;
};

export interface GridTableProps<T> {
    columns: TableColumn<T>[];
    data: TableData<T>,
    pageSizeOptionArray?: number[],
    setData?: (data: TableData<T>) => void,
    canDelete?: boolean;
}

export interface dateRange {
    start: Date,
    end: Date
}

export type TableColumn<T> = {
    field: keyof T | string;
    header: string;
    render?: (row: T) => ReactNode;
    sortable?: boolean;
    type: ColumnType;
    width?: number | string;
    options? : string[];
};

export type ExpandableTableProps<T> = {
    rows: T[];
    columns: TableColumn<T>[];
    getRowId: (row: T) => string;
    operations?: Operations<T>[];
    renderExpanded?: (row: T) => ReactNode;
    loadingRow?: string | null;
    onToggle?: (row: T, open: boolean) => void;
    orderBy?: string | null;
    order?: "asc" | "desc";
    onSort?: (field: string) => void;
    filters?: FilterItem[];
    onFilterChange?: (filters: FilterValue[]) => void;
};