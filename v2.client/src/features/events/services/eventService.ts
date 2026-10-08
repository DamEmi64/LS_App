import { API, call } from "@/shared";
import { ResponseList } from "@/shared/api/extension";
import { EventDto } from "@/shared/api/generated";

import { EventBody, EventParticipant } from "../types";
import type { UserData } from "@/features/auth";

export async function loadEvents(query: Record<string, string>) {
  const result = await call<ResponseList<EventDto>>(api => api.eventApi.get, query);

  return result.data || [];
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
