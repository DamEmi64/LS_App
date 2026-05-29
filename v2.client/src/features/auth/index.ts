import { AuthContextType } from "./context/authProvider";

export interface UserData {
    id: number;
    userId: string;
    login: string;
    email: string;
    role: string;
    permissions: string[];
}

export interface RegisterData {
    login: string;
    email: string;
    password: string;
    firstName?: string;
    lastName?: string;
}

export interface LoginData {
    login: string;
    password: string;
    rememberMe: boolean;
}

export interface PasswordChangeData {
    oldPassword: string;
    newPassword: string;
}

export interface ResetPasswordData {
    login: string;
    code: string;
    password: string;
}

export enum ProgressStatus {
    New = 'New',
    Executing = 'Executing',
    Success = 'Success',
    Failed = 'Failed',
    Paused = 'Paused'
}

export interface Process {
    id: string;
    title: string;
    jobs: Job[];
    errors: ProcessError[];
    endDate?: Date;
    startDate?: Date;
    percentage: number;
    status: ProgressStatus;
    user?: UserData;
}

export interface ProcessError {
    id: string;
    message: string;
    jobId: string;
}

export interface Job {
    id: string;
    name: string;
    jobId?: string;
    status: ProgressStatus;
    requestDate: Date;
    startDate?: Date;
    endDate?: Date;
    process: Process;
    parent?: Job;
    children: Job[];
    jobData?: string;
    operation: number;
}

export interface User {
    id : string,
    userName: string;
    email: string;
    phoneNumber: string;
    firstName: string;
    lastName: string;
    insDate: Date;
}

export interface Image {
    contentStr: string;
    id: string;
}

export interface LoginFormProps {
  auth: AuthContextType;
  onClose: () => void;
}
