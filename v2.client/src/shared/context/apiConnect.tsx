import React, { createContext, useContext, ReactNode } from 'react';
import endpoints from '@/app/endpoints.json';
import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import useLocalStorage from 'react-use-localstorage';
import { notify } from '@/shared/components/NotificationListener';
import { useTranslation } from 'react-i18next';

export type Response<T = any> = {
    data: T;
    total?: number;
}

export type ApiConnectContextType = {
    getUrl: (key: string) => string;
    get: <T>(urlKey: string, config?: AxiosRequestConfig, id?: string) => Promise<Response<T>>;
    post: <T, D = any>(urlKey: string, data?: D, config?: AxiosRequestConfig, id?: string) => Promise<Response<T>>;
    put: <T, D = any>(urlKey: string, data?: D, config?: AxiosRequestConfig, id?: string) => Promise<Response<T>>;
    del: <T>(urlKey: string, config?: AxiosRequestConfig, id?: string) => Promise<Response<T>>;
    download: (urlKey: string, id?: string) => Promise<AxiosResponse<any>>;
};

const ApiConnectContext = createContext<ApiConnectContextType | undefined>(undefined);

export const ApiConnect = ({ children }: { children: ReactNode }) => {
    axios.defaults.withCredentials = true;
    const [t] = useTranslation();
    const [endpoint] = useLocalStorage('apiEndpoint', 'http://192.168.1.58:5144');

    const getUrl = (key: string) => {
        const value = endpoints[key];
        if (!value) {
            throw new Error(t('EndpointNotFound'));
        }
        return endpoint + '/' + value;
    }

    const handleError = (error: AxiosError) => {
        notify('error', error.response?.data ?? error.message);
        throw error; // ensures the Promise rejects
    }

    const replaceId = (url: string, id?: string) => id ? url.replace(':id', id) : url;

    async function get<T>(urlKey: string, config?: AxiosRequestConfig, id?: string): Promise<Response<T>> {
        const url = replaceId(getUrl(urlKey), id);
        try {
            const rawResponse = await axios.get<T>(url, config);
            const response = rawResponse.data as unknown as Response<T>;
            return response.data == null && response.total == null ? { data: response as T , total: 1 } : response;
        } catch (err) {
            return handleError(err as AxiosError);
        }
    }

    async function post<T, D = any>(urlKey: string, data?: D, config?: AxiosRequestConfig, id?: string): Promise<Response<T>> {
        const url = replaceId(getUrl(urlKey), id);
        try {
            const rawResponse = await axios.post<T>(url, data, config);
            const response = rawResponse.data as unknown as Response<T>;
            return response.data == null && response.total == null ? { data: response as T , total: 1 } : response;
        } catch (err) {
            return handleError(err as AxiosError);
        }
    }

    async function put<T, D = any>(urlKey: string, data?: D, config?: AxiosRequestConfig, id?: string): Promise<Response<T>> {
        const url = replaceId(getUrl(urlKey), id);
        try {
            const rawResponse = await axios.put<T>(url, data, config);
            const response = rawResponse.data as unknown as Response<T>;
            return response.data == null && response.total == null ? { data: response as T , total: 1 } : response;
        } catch (err) {
            return handleError(err as AxiosError);
        }
    }

    async function del<T>(urlKey: string, config?: AxiosRequestConfig, id?: string): Promise<Response<T>> {
        const url = replaceId(getUrl(urlKey), id);
        try {
            const rawResponse = await axios.delete<T>(url, config);
            const response = rawResponse.data as unknown as Response<T>;
            return response.data == null && response.total == null ? { data: response as T , total: 1 } : response;
        } catch (err) {
            return handleError(err as AxiosError);
        }
    }

    async function download(urlKey: string, id?: string) {
        const url = replaceId(getUrl(urlKey), id);
        try {
            const response = await axios.get(url, { responseType: 'blob' });
            return response;
        } catch (err) {
            return handleError(err as AxiosError);
        }
    }

    return (
        <ApiConnectContext.Provider value={{ getUrl, get, post, put, del, download }}>
            {children}
        </ApiConnectContext.Provider>
    );
};

export const useApiConnect = () => {
    const context = useContext(ApiConnectContext);
    if (!context) {
        throw new Error('useApiConnect must be used within an ApiConnectProvider');
    }
    return context;
};