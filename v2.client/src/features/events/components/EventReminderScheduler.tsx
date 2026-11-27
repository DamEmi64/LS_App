import { useEffect } from "react";
import { App as CapacitorApp } from "@capacitor/app";
import { BackgroundTask, BackgroundTaskResult } from "@capgo/capacitor-background-task";
import { LocalNotifications } from "@capacitor/local-notifications";
import { isNativeApp } from "@/shared/platform";
import { appStorage } from "@/shared/storage/appStorage";
import { formatDay, scheduleMyEventReminders } from "../services/eventService";

const eventReminderTask = "event-reminders";

BackgroundTask.defineTask(eventReminderTask, async () => {
  const now = new Date();
  if (now.getHours() >= 3 && now.getHours() < 22) return BackgroundTaskResult.Success;

  const eventDay = new Date(now.getFullYear(), now.getMonth(), now.getDate());
  if (now.getHours() >= 22) eventDay.setDate(eventDay.getDate() + 1);

  const syncKey = "eventReminderSyncDay";
  if (appStorage.get(syncKey) === formatDay(eventDay)) return BackgroundTaskResult.Success;

  try {
    await scheduleMyEventReminders(eventDay);
    appStorage.set(syncKey, formatDay(eventDay));
    await appStorage.flush();
    return BackgroundTaskResult.Success;
  } catch (error) {
    console.error("Background event reminder sync failed.", error);
    return BackgroundTaskResult.Failed;
  }
});

const scheduleTomorrow = () => {
  const today = new Date();
  const tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);
  return scheduleMyEventReminders(today)
    .then(() => scheduleMyEventReminders(tomorrow))
    .catch(() => undefined);
};

export default function EventReminderScheduler() {
  useEffect(() => {
    if (!isNativeApp) return;

    void LocalNotifications.requestPermissions();
    void scheduleTomorrow();
    void BackgroundTask.isTaskRegisteredAsync(eventReminderTask).then(isRegistered => {
      if (!isRegistered) {
        return BackgroundTask.registerTaskAsync(eventReminderTask, {
          minimumInterval: 15,
          requiresNetwork: true,
        });
      }
    });

    const listener = CapacitorApp.addListener("appStateChange", ({ isActive }) => {
      if (isActive) void scheduleTomorrow();
    });

    let storageTimer = window.setTimeout(() => void scheduleTomorrow(), 1000);
    const unsubscribeStorage = appStorage.subscribe(() => {
      window.clearTimeout(storageTimer);
      storageTimer = window.setTimeout(() => void scheduleTomorrow(), 300);
    });

    return () => {
      window.clearTimeout(storageTimer);
      unsubscribeStorage();
      void listener.then(handle => handle.remove());
    };
  }, []);

  return null;
}
