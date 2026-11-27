import { useSyncExternalStore } from "react";
import { Backdrop, CircularProgress } from "@mui/material";

let activeTasks = 0;
const listeners = new Set<() => void>();

function subscribe(listener: () => void) {
    listeners.add(listener);
    return () => listeners.delete(listener);
}

function getSnapshot() {
    return activeTasks > 0;
}

function notify() {
    listeners.forEach(listener => listener());
}

export function beginTaskLoading() {
    activeTasks++;
    notify();
}

export function endTaskLoading() {
    activeTasks = Math.max(0, activeTasks - 1);
    notify();
}

export async function ExecuteWithLoading<T>(action: () => Promise<T>): Promise<T> {
    beginTaskLoading();

    // Let React paint the backdrop before starting work that may block the UI thread.
    await new Promise<void>(resolve => requestAnimationFrame(() => resolve()));

    try {
        return await action();
    } finally {
        endTaskLoading();
    }
}

export default function TaskExecuter() {
    const loading = useSyncExternalStore(subscribe, getSnapshot, () => false);

    return (
        <Backdrop
            open={loading}
            sx={{ color: "#fff", zIndex: theme => theme.zIndex.modal + 1 }}
            aria-busy={loading}
            aria-label="Task in progress"
        >
            <CircularProgress color="inherit" />
        </Backdrop>
    );
}
