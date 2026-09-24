import { call } from "@/shared";
import { ResponseList } from "@/shared/api/extension";
import { EventDto } from "@/shared/api/generated";

import { EventBody, EventParticipant } from "../types";

export async function loadEvents(query: Record<string, string>) {
  const result = await call<ResponseList<EventDto>>(api => api.eventClient.get, query);

  return result.data || [];
}

export function getEvent(id: string) {
  return call<EventDto>(api => api.eventClient.getById, { id });
}

export function createEvent(event: EventBody) {
  return call(api => api.eventClient.create, { eventDto: event });
}

export function updateEvent(event: EventDto, updatedEvent: EventBody) {
  return call(api => api.eventClient.updateById, {
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
  return call(api => api.eventClient.updateById, {
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
  return call(api => api.eventClient.deleteById, { id });
}

export function signInToEvent(id: string) {
  return call(api => api.eventClient.updateByIdSignIn, { id });
}

export function signOutOfEvent(id: string) {
  return call(api => api.eventClient.updateByIdSignOut, { id });
}

export function sendEventInvitation(id: string) {
  return call(api => api.eventClient.createByIdInvitation, { id });
}

export function createEventReminder(id: string, reminderDate: Date) {
  return call(api => api.eventClient.createByIdReminder, {
    id,
    reminderDto: { reminderDate: reminderDate.toISOString() },
  });
}

export function deleteEventReminder(id: string) {
  return call(api => api.eventClient.deleteByIdReminder, { id });
}
