import { clsx, type ClassValue } from "clsx";
import { useTranslation } from "react-i18next";
import { twMerge } from "tailwind-merge";
import dictionaries from '@/app/dictionaries.json';
import configuration from '@/app/configuration.json';
import useLocalStorage from "react-use-localstorage";

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs));
}

export function convertToDateStr(date: string): string {
    if (date == null) {
        return '';
    }

    return typeof date === 'string' ? new Date(date).toLocaleString() : date;
};


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

export function useVariable(key: string) {
    return useLocalStorage(key,configuration[key])
}

export interface DictionaryItem {
    key: string;
    title: string;
    description?: string;
}