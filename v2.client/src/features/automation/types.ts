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
    eventId: number;
    cron?: string;
}

export interface AutomatTask {
    id: string;
    OperationId: number;
    order: number;
    data: any;
}