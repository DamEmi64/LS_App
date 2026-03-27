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