import { ReactNode, createContext, useContext, useMemo } from 'react';
import axios, { AxiosError, AxiosResponse } from 'axios';
import useLocalStorage from 'react-use-localstorage'

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
import { BaseAPI } from '../api/generated/base';

type ApiError = {
    message?: string;
    title?: string;
};

export type ApiConnectContextType = {
    storiesApi: StoriesApi;
    placesApi: PlacesApi;
    heroesApi: HeroesApi;
    chaptersApi: ChaptersApi;
    processApi: ProcessApi;
    emailsApi: EmailsApi;
    templatesApi: TemplatesApi;
    authApi: AuthApi;
    filesApi: FilesApi;
    automationApi: AutomationsApi;
    homeApi: HomeApi;

    call: <TResponse>(
        api: BaseAPI,
        apiCall: (req: any) => Promise<{ data: any }>,
        input: any,
        mapResponse?: (data: any) => TResponse
    ) => Promise<TResponse>;

    raw: (
        api: BaseAPI,
        apiCall: (req: any) => Promise<{ data: any }>,
        input: any
    ) => Promise<AxiosResponse>;
};

const ApiConnectContext = createContext<ApiConnectContextType | null>(null);

export const ApiConnect = ({ children }: { children: ReactNode }) => {
    const [baseURL] = useLocalStorage(
        'apiEndpoint',
        'https://192.168.1.58:5144'
    );

    const axiosInstance = useMemo(() => {
        const instance = axios.create({
            baseURL,
            withCredentials: true
        });

        instance.interceptors.response.use(
            res => res,
            (error: AxiosError<ApiError>) => {
                if (error.message && !error.message.startsWith('Request failed with status')) {
                    notify('error', error.message);
                }else if (error.response.data.message) {
                    notify('error', error.response.data.message);
                }

                return Promise.reject(error);
            }
        );

        return instance;
    }, [baseURL]);


    const normalizeRequest = (input: any) => {
        if (
            input &&
            typeof input === 'object' &&
            !Array.isArray(input)
        ) {
            const keys = Object.keys(input);

            if (keys.includes('body') || (keys.includes('order') && keys.includes('orderBy'))) {
                return input;
            }

            if (keys.length === 1 && keys[0] === 'id') {
                return input;
            }

            return { body: input };
        }

        return input;
    };

    const call = async <
        TRes = any,
        TReq = any
    >(
        api: any,
        method: (req: TReq) => Promise<{ data: TRes }>,
        input: TReq,
        mapResponse?: (data: TRes) => any
    ): Promise<any> => {

        const res = await method.call(api, normalizeRequest(input));

        return mapResponse
            ? mapResponse(res.data)
            : res.data;
    };

    const raw = async <
        TRes = any,
        TReq = any
    >(
        api: any,
        method: (req: TReq) => Promise<{ data: TRes }>,
        input: TReq,
    ): Promise<any> => {

        return await method.call(api, normalizeRequest(input));
    };

    const api = useMemo<ApiConnectContextType>(() => ({
        storiesApi: new StoriesApi(null, null, axiosInstance),
        placesApi: new PlacesApi(null, null, axiosInstance),
        heroesApi: new HeroesApi(null, null, axiosInstance),
        chaptersApi: new ChaptersApi(null, null, axiosInstance),
        processApi: new ProcessApi(null, null, axiosInstance),
        emailsApi: new EmailsApi(null, null, axiosInstance),
        templatesApi: new TemplatesApi(null, null, axiosInstance),
        authApi: new AuthApi(null, null, axiosInstance),
        filesApi: new FilesApi(null, null, axiosInstance),
        automationApi: new AutomationsApi(null, null, axiosInstance),
        homeApi: new HomeApi(null, null, axiosInstance),
        call,
        raw
    }), [axiosInstance, call]);

    return (
        <ApiConnectContext.Provider value={api}>
            {children}
        </ApiConnectContext.Provider>
    );
};

export const useApiConnect = () => {
    const context = useContext(ApiConnectContext);

    if (!context) {
        throw new Error('useApiConnect must be used within ApiConnect');
    }

    return context;
};