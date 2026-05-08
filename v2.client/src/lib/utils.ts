import { clsx, type ClassValue } from "clsx";
import { useTranslation } from "react-i18next";
import { twMerge } from "tailwind-merge";
import dictionaries from '@/app/dictionaries.json';
import configuration from '@/app/configuration.json';
import { saveAs } from 'file-saver';
import { raw } from "@/shared";

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs));
}

export function convertToDateStr(date: string): string {
    if (date == null) {
        return '';
    }

    return typeof date === 'string' ? new Date(date).toLocaleString() : date;
};

export const download = (id: string, title: string) => {
    raw(api => api.homeApi.getMedia, { id })
        .then((response) => {
            let filename = title + response.data.extension;

            const mime = getMimeFromExtension(response.data.extension);


            const byteCharacters = atob(response.data.content);

            // convert to byte array
            const byteNumbers = new Array(byteCharacters.length);

            for (let i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }

            const byteArray = new Uint8Array(byteNumbers);

            // create blob
            const blob = new Blob([byteArray], { type: mime });
            saveAs(blob, filename);
        })
        .catch((error) => console.error('Download failed:', error));
}

const getMimeFromExtension = (extension) => {
    switch (extension.toLowerCase()) {
        case "docx":
            return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        case "xlsx":
            return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        case "bmp":
            return "image/bmp";

        case "gif":
            return "image/gif";

        case "jpg":
        case "jpeg":
            return "image/jpeg";

        case "png":
            return "image/png";

        case "pdf":
            return "application/pdf";

        case "html":
            return "text/html";

        case "txt":
            return "text/plain";

        default:
            return "application/octet-stream";
    }
};

export const get = (key: keyof typeof configuration) => {
    return (
        localStorage.getItem(key) ??
        configuration[key] ??
        ''
    );
}

export function useDictionaryTranslation() {
    const { t } = useTranslation('dictionaries');

    return (dictionary: string, value: number | string) => {
        const dictKey = dictionary.replace(/\s+/g, '_');

        return {
            key: value,
            title: t(`${dictKey}.${value}.title`),
            description: t(`${dictKey}.${value}.description`)
        };
    };
}

export function getDictionary(dictionaryName: string): DictionaryItem[] {
    const dictKey = dictionaryName.replace(/\s+/g, '_');

    const dict = dictionaries[dictKey];

    if (!dict) return [];

    return Object.entries(dict).map(([key, value]: [string, any]) => ({
        key,
        title: value.Title,
        description: value.Description
    }));
}

export interface DictionaryItem {
    key: string;
    title: string;
    description?: string;
}