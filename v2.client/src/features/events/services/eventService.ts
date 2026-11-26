import { API, call } from "@/shared";
import { ResponseList } from "@/shared/api/extension";
import { EventDto } from "@/shared/api/generated";

import { EventBody, EventParticipant } from "../types";
import type { UserData } from "@/features/auth";
import { LocalNotifications } from "@capacitor/local-notifications";
import { isNativeApp } from "@/shared/platform";
import i18n from "@/shared/localization/i18n";

export async function loadEvents(query: Record<string, string>) {
  const result = await call<ResponseList<EventDto>>(api => api.eventApi.get, query);

  return result.data || [];
}

/** Fetch a day's signed-up events and schedule local reminders 15 minutes before each start. */
export async function scheduleMyEventReminders(eventDay = new Date(Date.now() + 24 * 60 * 60 * 1000)) {
  if (!isNativeApp) return;

  const now = new Date();
  const dayStart = new Date(eventDay.getFullYear(), eventDay.getMonth(), eventDay.getDate());
  const dayEnd = new Date(dayStart.getFullYear(), dayStart.getMonth(), dayStart.getDate() + 1);
  dayEnd.setMilliseconds(dayEnd.getMilliseconds() - 1);
  const dayKey = formatDay(dayStart);
  const from = toLocalDateTime(dayStart);
  const to = toLocalDateTime(dayEnd);
  const result = await call<ResponseList<EventDto>>(api => api.eventApi.getMe, {
    page: 0,
    pageSize: 500,
    dateFrom: from,
    dateTo: to,
  });

  const events = (result.data || []).filter(event => event.id && event.eventDate);
  const permission = await LocalNotifications.checkPermissions();
  const granted = permission.display === "granted"
    ? permission
    : await LocalNotifications.requestPermissions();
  if (granted.display !== "granted") return;

  const existing = await LocalNotifications.getPending();
  const eventReminderIds = existing.notifications
    .filter(notification => notification.extra?.kind === "event-reminder" && notification.extra?.eventDay === dayKey)
    .map(notification => ({ id: notification.id }));
  if (eventReminderIds.length) await LocalNotifications.cancel({ notifications: eventReminderIds });

  const notifications = events.flatMap(event => {
    const at = new Date(new Date(event.eventDate!).getTime() - 15 * 60 * 1000);
    if (at <= now) return [];

    return [{
      id: reminderId(event.id!),
      title: i18n.t("events.reminderTitle"),
      body: i18n.t("events.reminderBody", { title: event.title || i18n.t("events.title") }),
      schedule: { at },
      extra: { kind: "event-reminder", eventId: event.id, eventDay: dayKey },
    }];
  });

  if (notifications.length) await LocalNotifications.schedule({ notifications });
}

function toLocalDateTime(date: Date) {
  const pad = (value: number) => String(value).padStart(2, "0");
  const milliseconds = String(date.getMilliseconds()).padStart(3, "0");
  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}.${milliseconds}`;
}

export function formatDay(date: Date) {
  const pad = (value: number) => String(value).padStart(2, "0");
  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;
}

function reminderId(eventId: string) {
  let hash = 0;
  for (const character of eventId) hash = (hash * 31 + character.charCodeAt(0)) >>> 0;
  return 1000000000 + (hash % 1000000000);
}

export function getEvent(id: string) {
  return call<EventDto>(api => api.eventApi.getById, { id });
}

export function createEvent(event: EventBody) {
  return call(api => api.eventApi.create, { eventDto: event });
}

export function updateEvent(event: EventDto, updatedEvent: EventBody) {
  return call(api => api.eventApi.updateById, {
    id: event.id!,
    eventDto: {
      ...event,
      title: updatedEvent.title,
      description: updatedEvent.description,
      eventDate: updatedEvent.eventDate,
      image: updatedEvent.image,
      imageContent: updatedEvent.imageContent,
      category: updatedEvent.category,
    },
  });
}

export function updateEventParticipants(event: EventDto, participants: EventParticipant[]) {
  return call(api => api.eventApi.updateById, {
    id: event.id!,
    eventDto: {
      ...event,
      participates: participants.map(participant => ({
        id: participant.id,
        login: participant.login,
        userId: participant.userId,
        email: participant.email,
        present: participant.present,
      })),
    },
  });
}

export function deleteEvent(id: string) {
  return call(api => api.eventApi.deleteById, { id });
}

export function signInToEvent(id: string) {
  return call(api => api.eventApi.updateByIdSignIn, { id });
}

export async function loadEventUsers() {
  const result = await call<{ data: UserData[] }>(api => api.homeApi.getUsers, {});
  return result.data || [];
}

export function signInUserToEvent(eventId: string, userId: string) {
  return API.eventApi.updateByIdSignIn({ id: eventId }, { params: { userId } });
}

export function signOutOfEvent(id: string) {
  return call(api => api.eventApi.updateByIdSignOut, { id });
}

export function sendEventInvitation(id: string) {
  return call(api => api.eventApi.createByIdInvitation, { id });
}

export function createEventReminder(id: string, reminderDate: Date) {
  return call(api => api.eventApi.createByIdReminder, {
    id,
    reminderDto: { reminderDate: reminderDate.toISOString() },
  });
}

export function deleteEventReminder(id: string) {
  return call(api => api.eventApi.deleteByIdReminder, { id });
}
