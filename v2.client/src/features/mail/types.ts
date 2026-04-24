export interface Email {
    id: string;
    insDate: Date;
    subject: string;
    body: string;
    sender: string;
    recipient: string;
    sentDate?: Date;
}

export interface Template {
    id: string;
    insDate: Date;
    subject: string;
    body: string;
}

export interface CommunicationRules {
    variables: CommunicationRule[];
    functions: CommunicationRule[];
}

export interface CommunicationRule {
    id: number;
    title: string;
    description?: string;
    invoker: string;
}