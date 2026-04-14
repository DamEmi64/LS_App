export class UserData {
    id: number;
    userId: string;
    login: string;
    email: string;
    role: string;
    permissions: string[];
}

export class RegisterData {
    login: string;
    email: string;
    password: string;
    firstName?: string;
    lastName?: string;
}

export class LoginData {
    login: string;
    password: string;
    rememberMe: boolean;
}

export class PasswordChangeData {
    oldPassword: string;
    newPassword: string;
}

export enum ProgressStatus {
    New = 'New',
    Executing = 'Executing',
    Success = 'Success',
    Failed = 'Failed',
    Paused = 'Paused'
}

export class Process {
    id: string;
    title: string;
    jobs: Job[] = [];
    errors: ProcessError[] = [];
    endDate?: Date;
    startDate?: Date;
    percentage: number;
    status: ProgressStatus;
    user?: UserData;
}

export class ProcessError {
    id: string;
    message: string;
    jobId: string;
}

export class Job {
    id: string;
    name: string;
    jobId?: string;
    status: ProgressStatus;
    requestDate: Date;
    startDate?: Date;
    endDate?: Date;
    process: Process;
    parent?: Job;
    children: Job[] = [];
    jobData?: string;
    operation: number;
}

export class User {
    userName: string;
    email: string;
    phoneNumber: string;
    firstName: string;
    lastName: string;
    insDate: Date;
}

export class Image {
    contentStr: string;
    content: string;
    id: string;
}