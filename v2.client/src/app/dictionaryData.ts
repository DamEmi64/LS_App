import fallbackDictionaries from './dictionaries.json';

let dictionaries: Record<string, unknown> = fallbackDictionaries;

export function setRemoteDictionaries(value: Record<string, unknown>) {
  dictionaries = { ...fallbackDictionaries, ...value };
}

export function getDictionaries() {
  return dictionaries;
}

