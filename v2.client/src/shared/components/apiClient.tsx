import axios, { AxiosError } from 'axios';
import {
    StoriesApi,
    PlacesApi,
    HeroesApi,
    ChaptersApi,
    ProcessApi,
    EmailsApi,
    TemplatesApi,
    AuthApi,
    FilesApi,
    AutomationsApi,
    HomeApi
} from '@/shared/api/generated';

import { notify } from '../components/NotificationListener';

type ApiError = {
    message?: string;
    title?: string;
};


const axiosInstance = axios.create({
    baseURL: 'https://lsfamilia.runasp.net',
    withCredentials: true
});

axiosInstance.interceptors.response.use(
    res => res,
    (error: AxiosError<ApiError>) => {
        if (
            error.message &&
            !error.message.startsWith('Request failed with status')
        ) {
            notify('error', error.message);
        } else if (error.response?.data?.message) {
            notify('error', error.response.data.message);
        }

        return Promise.reject(error);
    }
);

function bindApi<T extends object>(api: T): T {
    const proto = Object.getPrototypeOf(api);

    Object.getOwnPropertyNames(proto).forEach(key => {
        if (key === 'constructor') return;

        const value = (api as any)[key];

        if (typeof value === 'function') {
            (api as any)[key] = value.bind(api);
        }
    });

    return api;
}

export const API = {
    storiesApi: bindApi(new StoriesApi(null, null, axiosInstance)),
    placesApi: bindApi(new PlacesApi(null, null, axiosInstance)),
    heroesApi: bindApi(new HeroesApi(null, null, axiosInstance)),
    chaptersApi: bindApi(new ChaptersApi(null, null, axiosInstance)),
    processApi: bindApi(new ProcessApi(null, null, axiosInstance)),
    emailsApi: bindApi(new EmailsApi(null, null, axiosInstance)),
    templatesApi: bindApi(new TemplatesApi(null, null, axiosInstance)),
    authApi: bindApi(new AuthApi(null, null, axiosInstance)),
    filesApi: bindApi(new FilesApi(null, null, axiosInstance)),
    automationApi: bindApi(new AutomationsApi(null, null, axiosInstance)),
    homeApi: bindApi(new HomeApi(null, null, axiosInstance)),
};

/* ---------------------------------- */
/* Normalize request */
/* ---------------------------------- */

const normalizeRequest = (input: any) => {
    if (
        input &&
        typeof input === 'object' &&
        !Array.isArray(input)
    ) {
        const keys = Object.keys(input);

        if (
            keys.includes('body') ||
            (keys.includes('page') && keys.includes('pageSize'))
        ) {
            return input;
        }

        if (keys.length === 1 && keys[0] === 'id') {
            return input;
        }

        return { body: input };
    }

    return input;
};

export const call = async <TRes = unknown>(
    selector: (api: typeof API) => (req) => Promise<any>,
    input: any,
    mapResponse?: (data: any) => TRes
): Promise<TRes> => {
    const method = selector(API);

    const res = await method(normalizeRequest(input));

    return mapResponse ? mapResponse(res.data) : res.data;
};

export const raw = async <TRes, TReq>(
    selector: (api: typeof API) => (req: TReq) => Promise<TRes>,
    input: TReq
) => {
    const method = selector(API);
    return await method(normalizeRequest(input));
};