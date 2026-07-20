export interface EventParticipant {
  id: string;
  login: string;
  email: string;
  userId: string;
  present: boolean;
}

export interface EventBody {
  title: string;
  description: string;
  image: string;
  imageContent: string;
  eventDate: string;
  participants?: EventParticipant[];
  category: number;
}

export interface EventComponentProps {
  event?: Partial<EventBody>;
  onSave?: (event: EventBody) => void;
  onDelete?: (event: EventBody) => void;
  isEdit?: boolean;
  isNew?: boolean;
  readonly?: boolean;
}
