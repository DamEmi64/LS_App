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
    HomeApi,
    EventsApi
} from '@/shared/api/generated';

import { notify } from '../components/NotificationListener';
import { get } from '@/lib/utils';
import { MapRule } from '../types';
import { map } from '../api/extension';

const authTokenKey = 'authToken';

export const getAuthToken = () => localStorage.getItem(authTokenKey);

export const setAuthToken = (token: string | null) => {
    if (token) {
        localStorage.setItem(authTokenKey, token);
    } else {
        localStorage.removeItem(authTokenKey);
    }
};

type ApiError = {
    message?: string;
    title?: string;
};

const axiosInstance = axios.create();

axiosInstance.interceptors.request.use((config) => {
    const baseURL = get('apiEndpoint');

    config.baseURL = baseURL.endsWith('/')
        ? baseURL.slice(0, -1)
        : baseURL;

    if (!config.url || config.url === 'null') {
        config.url = '';
    }

    const token = getAuthToken();
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
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
    storiesApi: bindApi(new StoriesApi(null, '', axiosInstance)),
    placesApi: bindApi(new PlacesApi(null, '', axiosInstance)),
    heroesApi: bindApi(new HeroesApi(null, '', axiosInstance)),
    chaptersApi: bindApi(new ChaptersApi(null, '', axiosInstance)),
    processApi: bindApi(new ProcessApi(null, '', axiosInstance)),
    emailsApi: bindApi(new EmailsApi(null, '', axiosInstance)),
    templatesApi: bindApi(new TemplatesApi(null, '', axiosInstance)),
    authApi: bindApi(new AuthApi(null, '', axiosInstance)),
    filesApi: bindApi(new FilesApi(null, '', axiosInstance)),
    automationApi: bindApi(new AutomationsApi(null, '', axiosInstance)),
    homeApi: bindApi(new HomeApi(null, '', axiosInstance)),
    eventClient: bindApi(new EventsApi(null, '', axiosInstance)),
};




export const call = async <TRes = unknown,TReq = unknown>(
    selector: (api: typeof API) => (req:TReq) => Promise<any>,
    input: any,
    mapResponse?: (data: any) => TRes
): Promise<TRes> => {
    const method = selector(API);
    const res = await method(input);

    return mapResponse ? mapResponse(res.data) : res.data;
};

export const raw = async <TRes, TReq>(
    selector: (api: typeof API) => (req: TReq) => Promise<TRes>,
    input: TReq
) => {
    const method = selector(API);
    return await method(input);
};
