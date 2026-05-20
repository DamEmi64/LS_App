export interface EventParticipant {
  id: string;
  login: string;
}

export interface EventBody {
  title: string;
  description: string;
  image: string;
  participants?: EventParticipant[];
}

export interface EventComponentProps {
  event?: Partial<EventBody>;
  onSave?: (event: EventBody) => void;
  onDelete?: (event: EventBody) => void;
  isEdit?: boolean;
  isNew?: boolean;
  readonly?: boolean;
}
