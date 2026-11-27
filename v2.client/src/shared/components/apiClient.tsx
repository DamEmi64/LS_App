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
    CommunicationHistoryApi,
    DiscordApi,
    FilesV2Api,
    DirectoriesApi,
    InvoicesApi,
    EventInvoicesApi,
    SystemAutomateApi
} from '@/shared/api/generated';

import { notify } from '../components/NotificationListener';
import { get } from '@/lib/utils';
import { getNotify } from '@/lib/notifyProvider';
import { MapRule } from '../types';
import { map } from '../api/extension';
import { appStorage } from '@/shared/storage/appStorage';
import { beginTaskLoading, endTaskLoading } from '@/shared/components/taskExecuter';

const authTokenKey = 'authToken';
const refreshTokenKey = 'refreshToken';
const authUserIdKey = 'authUserId';
const rememberedUsernameKey = 'rememberedUsername';
const rememberedPasswordKey = 'rememberedPassword';

export type AuthToken = {
    accessToken?: string;
    refreshToken?: string;
    userId?: string;
    expiresAt?: string;
    refreshTokenExpiresAt?: string;
};

export const getAuthToken = () => appStorage.get(authTokenKey);
export const getRefreshToken = () => appStorage.get(refreshTokenKey);
export const getAuthUserId = () => appStorage.get(authUserIdKey);

export const setAuthToken = (token: string | null) => {
    if (token) {
        appStorage.set(authTokenKey, token);
    } else {
        appStorage.remove(authTokenKey);
        appStorage.remove(refreshTokenKey);
        appStorage.remove(authUserIdKey);
    }
};

export const setAuthTokens = (token: AuthToken | null) => {
    if (!token?.accessToken || !token.refreshToken || !token.userId) {
        setAuthToken(null);
        return;
    }

    appStorage.set(authTokenKey, token.accessToken);
    appStorage.set(refreshTokenKey, token.refreshToken);
    appStorage.set(authUserIdKey, token.userId);
};

type ApiError = {
    message?: string;
    title?: string;
};

export const axiosInstance = axios.create();

const loginWithRememberedCredentials = async () => {
    const login = appStorage.get(rememberedUsernameKey);
    const password = appStorage.get(rememberedPasswordKey);
    if (!login || !password) return null;

    const response = await raw(api => api.authApi.createLogin, {
        loginModel: { login, password, rememberMe: true },
    });
    const token = response.data as unknown as AuthToken;
    if (!token?.accessToken || !token.refreshToken || !token.userId) return null;

    setAuthTokens(token);
    await appStorage.flush();
    return token.accessToken;
};

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

    // Mutating API requests come from user actions such as create, edit, delete,
    // publish, and sign-in. Track them centrally so forms do not each need to
    // remember to add a loading wrapper.
    const taskConfig = config as typeof config & { _taskLoading?: boolean };
    if (config.method && config.method.toLowerCase() !== 'get' && !taskConfig._taskLoading) {
        taskConfig._taskLoading = true;
        beginTaskLoading();
    }

    return config;
});

axiosInstance.interceptors.response.use(
    res => {
        const taskConfig = res.config as typeof res.config & { _taskLoading?: boolean };
        if (taskConfig._taskLoading) {
            taskConfig._taskLoading = false;
            endTaskLoading();
        }
        return res;
    },
    async (error: AxiosError<ApiError>) => {
        const originalRequest = error.config as any;
        const refreshToken = getRefreshToken();
        const userId = getAuthUserId();

        if (
            (error.response?.status === 401 || error.response?.status === 403) &&
            originalRequest &&
            !originalRequest._autoLoginAttempted &&
            !originalRequest.url?.includes('/api/Auth/refresh') &&
            !originalRequest.url?.includes('/api/Auth/login')
        ) {
            if (!originalRequest._retry && refreshToken && userId) {
                originalRequest._retry = true;
                try {
                    const refreshResponse = await raw(api => api.authApi.createRefresh, {
                        refreshTokenModel: { userId, refreshToken }
                    });
                    const tokens = refreshResponse.data as unknown as AuthToken;
                    setAuthTokens(tokens);
                    await appStorage.flush();
                    originalRequest.headers.Authorization = `Bearer ${tokens.accessToken}`;
                    return axiosInstance(originalRequest);
                } catch {
                    // Try the saved sign-in below when refresh credentials are rejected.
                }
            }

            originalRequest._autoLoginAttempted = true;
            try {
                const accessToken = await loginWithRememberedCredentials();
                if (accessToken) {
                    originalRequest.headers.Authorization = `Bearer ${accessToken}`;
                    return axiosInstance(originalRequest);
                }
            } catch {
                // Fall through to the original request error when saved credentials are stale.
            }

            setAuthToken(null);
        }

        const responseData = error.response?.data as any;

        if (
            error.message &&
            !error.message.startsWith('Request failed with status')
        ) {
            notify('error', error.message);
        } else if (Array.isArray(error.response?.data)) {
            error.response.data.forEach((message: any) => {
                if (message?.messageId !== undefined) notify('error', getNotify(message.messageId, message.args ?? []));
            });
        } else if (responseData.messageId !== undefined) {
            notify('error', getNotify(responseData.messageId, responseData.args ?? []));
        }

        const taskConfig = originalRequest as { _taskLoading?: boolean } | undefined;
        if (taskConfig?._taskLoading) {
            taskConfig._taskLoading = false;
            endTaskLoading();
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
    eventApi: bindApi(new EventsApi(null, '', axiosInstance)),
    communicationHistoryClient: bindApi(new CommunicationHistoryApi(null, '', axiosInstance)),
    discordClient: bindApi(new DiscordApi(null, '', axiosInstance)),
    filesV2Api: bindApi(new FilesV2Api(null, '', axiosInstance)),
    directoriesApi: bindApi(new DirectoriesApi(null, '', axiosInstance)),
    invoiceApi: bindApi(new InvoicesApi(null,'',axiosInstance)),
    eventInvoiceApi: bindApi(new EventInvoicesApi(null, '', axiosInstance)),
    systemAutomationApi: bindApi(new SystemAutomateApi(null, '', axiosInstance))
};

export const call = async <TRes = unknown, TReq = unknown>(
    selector: (api: typeof API) => (req: TReq) => Promise<any>,
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
