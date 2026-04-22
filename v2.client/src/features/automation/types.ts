export interface Automat {
    id: string;
    insDate: Date;
    title: string;
    description?: string;
    triggers: Trigger[];
    tasks: AutomatTask[];
    active: boolean;
}

export interface Trigger {
    id: string;
    type: number;
    cron?: string;
    eventId: number[];
}

export interface AutomatTask {
    id: string;
    OperationId: number;
    order: number;
    data: any;
}