import { useSyncExternalStore } from 'react';
import { appStorage } from './appStorage';

export function useAppStorage(key: string, defaultValue: string) {
    const value = useSyncExternalStore(
        appStorage.subscribe,
        () => appStorage.get(key) ?? defaultValue,
        () => defaultValue
    );

    return [value, (nextValue: string) => appStorage.set(key, nextValue)] as const;
}
