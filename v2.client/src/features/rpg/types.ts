import { FileItem } from "@/shared";

export interface ChapterTableProps {
    chapter: Chapter
}

export interface Story {
    id: string;
    insDate: Date;
    upDate: Date;
    title: string;
    description: string;
    startDate?: Date;
    endDate?: Date;
    chapters: Chapter[];
    files: FileItem[];
    summary?: string;
}
export interface Place {
    id: string;
    insDate: Date;
    upDate: Date;
    title: string;
    description: string;
    image?: string;
    imageId?: string;
    chapter?: string;
}
export interface Hero {
    id: string;
    insDate: Date;
    upDate: Date;
    firstName: string;
    lastName: string;
    description: string;
    player?: string;
    image?: string;
    imageId?: string;
    chapter?: string;
    playerData?: string;
    skills: [];
}
export interface Chapter {
    id: string;
    insDate: Date;
    upDate: Date;
    title: string;
    order: number;
    description: string;
    startDate: Date;
    endDate: Date;
    summary?: Uint8Array;
    heroes: Hero[];
    places: Place[];
    links?: Link[];
    sessions?: Session[];
    draft: boolean;
    flow: {nodes: any, edges: any}
}

export interface HeroDto {
    id: string;
    firstName: string;
    lastName: string;
    description: string;
    player?: string;
    image?: string;
    imageId?: string;
    chapter: string;
    playerData?: string;
    skills: Skill[];
}

export interface SessionDto {
    id: string;
    title: string;
    description: string;
    player?: string;
    image?: string;
    imageId?: string;
    story: string;
    chapter?: string;
    order?: number;
    links?: Link[];
    sessions?: Session[],
    draft?: boolean;
    summary?: string;
}

export interface PlayerData {
    id: string;
    content: string;
    skills?: Skill[];
}

export interface Skill {
    id: any;
    title: string;
    skillId: any;
    value: number;
}

export interface Link {
    title: string;
    url: string;
}

export interface Session {
    id: string;
    start: Date;
    end: Date;
    summary?: string;
}

export interface battleNpc {
    id: number;
    title: string;
    health: string;
    row: number;
    column: number;
    color?: 'black' | 'red' | 'blue' | 'green' | 'yellow';
}

export interface ProgressNode {
    id: string;
    title:string;
    description: string;
    condititon: string;
}
