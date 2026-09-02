import { Capacitor } from '@capacitor/core';
import { CapacitorSQLite, SQLiteConnection, type SQLiteDBConnection } from '@capacitor-community/sqlite';

type StorageListener = () => void;

const databaseName = 'ls_app_data';
const values = new Map<string, string>();
const listeners = new Set<StorageListener>();

let database: SQLiteDBConnection | undefined;
let writeQueue = Promise.resolve();

const notify = () => listeners.forEach(listener => listener());

const readBrowserStorage = () => {
    for (let index = 0; index < localStorage.length; index += 1) {
        const key = localStorage.key(index);
        if (key) {
            const value = localStorage.getItem(key);
            if (value !== null) values.set(key, value);
        }
    }
};

/**
 * A synchronous cache backed by SQLite on iOS and Android. The cache is loaded
 * before React renders, so callers that previously needed localStorage remain
 * safe in request interceptors and during initial component rendering.
 */
export const appStorage = {
    async initialize() {
        readBrowserStorage();

        if (!Capacitor.isNativePlatform()) return;

        const sqlite = new SQLiteConnection(CapacitorSQLite);
        const existingConnection = await sqlite.isConnection(databaseName, false);
        database = existingConnection.result
            ? await sqlite.retrieveConnection(databaseName, false)
            : await sqlite.createConnection(databaseName, false, 'no-encryption', 1, false);

        await database.open();
        await database.execute(`
            CREATE TABLE IF NOT EXISTS app_storage (
                key TEXT PRIMARY KEY NOT NULL,
                value TEXT NOT NULL
            );
        `);

        const result = await database.query('SELECT key, value FROM app_storage');

        if (result.values?.length) {
            values.clear();
            result.values.forEach(row => values.set(String(row.key), String(row.value)));
        } else {
            // First native launch: retain any values available in the WebView.
            await Promise.all([...values.entries()].map(([key, value]) =>
                database!.run(
                    'INSERT OR REPLACE INTO app_storage (key, value) VALUES (?, ?)',
                    [key, value]
                )
            ));
        }
    },

    get(key: string) {
        return values.get(key) ?? null;
    },

    set(key: string, value: string) {
        values.set(key, value);
        notify();

        if (database) {
            writeQueue = writeQueue
                .then(() => database!.run(
                    'INSERT OR REPLACE INTO app_storage (key, value) VALUES (?, ?)',
                    [key, value]
                ))
                .catch(error => console.error('Unable to persist app data to SQLite.', error));
        } else {
            localStorage.setItem(key, value);
        }
    },

    remove(key: string) {
        values.delete(key);
        notify();

        if (database) {
            writeQueue = writeQueue
                .then(() => database!.run('DELETE FROM app_storage WHERE key = ?', [key]))
                .catch(error => console.error('Unable to remove app data from SQLite.', error));
        } else {
            localStorage.removeItem(key);
        }
    },

    async flush() {
        await writeQueue;
    },

    subscribe(listener: StorageListener) {
        listeners.add(listener);
        return () => listeners.delete(listener);
    }
};
