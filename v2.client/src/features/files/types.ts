export interface FilesInfoProps {
    file: File;
    edit?: (data: File) => void;
    del?: (data: File) => void;
}

export interface FilesEditProps {
    file: EditFile;
    toSave?: (data: EditFile) => void;
}

export interface AdditionalData {
    gameGenre?: number;
    subject?: string;
    year?: number;
    semester?: number;
    id: string;
    insDate: Date;
    upDate: Date;
}

export interface SourceLink {
    sourceType: number;
    link: string;
    imported: boolean;

    id: string;
    insDate: Date;
    upDate: Date;
}

export interface File {
    title: string;
    image?: string;
    imageData?: string;
    content?: string;
    locaction?: string;
    fileType: number;
    additionalData: AdditionalData;
    sources: SourceLink[];
    id: string;
    insDate: Date;
    upDate: Date;
    links?: string;
}

export interface EditFile {
    title: string;
    image?: string;
    imageData?: string;
    useFileUpload?: boolean;
    content?: string;
    locaction?: string;
    fileType: number;
    gameGenre?: number;
    subject?: string;
    year?: number;
    semester?: number;
    sourceType?: number;
    links: string;
}