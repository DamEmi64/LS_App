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
    EventsApi,
    CommunicationHistoryApi
} from '@/shared/api/generated';

import { notify } from '../components/NotificationListener';
import { get } from '@/lib/utils';
import { MapRule } from '../types';
import { map } from '../api/extension';

const authTokenKey = 'authToken';
const refreshTokenKey = 'refreshToken';
const authUserIdKey = 'authUserId';

export type AuthToken = {
    accessToken?: string;
    refreshToken?: string;
    userId?: string;
    expiresAt?: string;
    refreshTokenExpiresAt?: string;
};

export const getAuthToken = () => localStorage.getItem(authTokenKey);
export const getRefreshToken = () => localStorage.getItem(refreshTokenKey);
export const getAuthUserId = () => localStorage.getItem(authUserIdKey);

export const setAuthToken = (token: string | null) => {
    if (token) {
        localStorage.setItem(authTokenKey, token);
    } else {
        localStorage.removeItem(authTokenKey);
        localStorage.removeItem(refreshTokenKey);
        localStorage.removeItem(authUserIdKey);
    }
};

export const setAuthTokens = (token: AuthToken | null) => {
    if (!token?.accessToken || !token.refreshToken || !token.userId) {
        setAuthToken(null);
        return;
    }

    localStorage.setItem(authTokenKey, token.accessToken);
    localStorage.setItem(refreshTokenKey, token.refreshToken);
    localStorage.setItem(authUserIdKey, token.userId);
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
    async (error: AxiosError<ApiError>) => {
        const originalRequest = error.config as any;
        const refreshToken = getRefreshToken();
        const userId = getAuthUserId();

        if (
            error.response?.status === 401 &&
            originalRequest &&
            !originalRequest._retry &&
            refreshToken &&
            userId &&
            !originalRequest.url?.includes('/api/Auth/refresh')
        ) {
            originalRequest._retry = true;

            try {
                const baseURL = get('apiEndpoint');
                const normalizedBaseUrl = baseURL.endsWith('/')
                    ? baseURL.slice(0, -1)
                    : baseURL;
                const refreshResponse = await axios.post<AuthToken>(`${normalizedBaseUrl}/api/Auth/refresh`, {
                    userId,
                    refreshToken
                });

                setAuthTokens(refreshResponse.data);
                originalRequest.headers.Authorization = `Bearer ${refreshResponse.data.accessToken}`;

                return axiosInstance(originalRequest);
            } catch {
                setAuthToken(null);
            }
        }

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
    communicationHistoryClient: bindApi(new CommunicationHistoryApi(null, '', axiosInstance))
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
