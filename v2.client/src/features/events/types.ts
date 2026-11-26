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
  circularPeriod: CircularEnum
}

export enum CircularEnum {
  None = 0,
  Daily = 1,
  Weekly = 2,
  Monthly = 3,
  Yearly = 4
}

export interface EventComponentProps {
  event?: Partial<EventBody>;
  onSave?: (event: EventBody) => void;
  onDelete?: (event: EventBody) => void;
  isEdit?: boolean;
  isNew?: boolean;
  readonly?: boolean;
}
