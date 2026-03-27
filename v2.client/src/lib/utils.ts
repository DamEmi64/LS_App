import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs));
}

export function convertToDateStr(date: string): string {
    if (date == null) {
        return '';
    }

    return typeof date === 'string' ? new Date(date).toLocaleString() : date;
};

export function getDictionaryKey(value: number, dictionary: any) {
    return Object.entries(dictionary).find(([key, val]) => val === value)?.[0];
}