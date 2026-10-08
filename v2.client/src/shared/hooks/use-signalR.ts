import { useEffect, useRef, useState, useCallback } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel
} from "@microsoft/signalr";
import { notify } from "../components/NotificationListener";
import { useConfiguration } from "../context/configuration";
import { getAuthToken } from "../components/apiClient";

const hubs = [
  { name: "notify", url: "notify" },
  { name: "rpg", url: "rpghub" }
]

type Handler = (...args: any[]) => void;

export const useSignalR = (hubName: string, onConnected?: () => void, query?: Record<string, string>) => {
  const connectionRef = useRef<HubConnection | null>(null);
  const handlersRef = useRef<Map<string, Handler>>(new Map());
  const {useVariable} = useConfiguration();

  const [ednpoint] = useVariable('apiEndpoint');
  const [connected, setConnected] = useState(false);

  const hub = hubs.find(x => x.name == hubName);

  if (!hub) {
    notify('error', 'Hub ' + hubName + ' not found');
  }

  const baseHubUrl = `${ednpoint.replace(/\/+$/, '')}/${hub.url.replace(/^\/+/, '')}`;
  const queryString = new URLSearchParams(query).toString();
  const hubUrl = queryString ? `${baseHubUrl}?${queryString}` : baseHubUrl;

  // 🚀 Start connection
  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => getAuthToken() ?? ''
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    connectionRef.current = connection;

    connection
      .start()
      .then(() => {
        console.log(`SignalR connected: ${hub.name}`);
        setConnected(true);

        handlersRef.current.forEach((handler, event) => {
          connection.on(event, handler);
        });
      })
      .catch((err) => console.error("SignalR error:", err));

    connection.onclose(() => setConnected(false));
    connection.onreconnected(() => setConnected(true));

    return () => {
      connection.stop();
    };
  }, [hubName, hubUrl]);

  // 📡 Subscribe
  const on = useCallback((event: string, handler: Handler) => {
    handlersRef.current.set(event, handler);

    if (connectionRef.current) {
      connectionRef.current.on(event, handler);
    }
  }, []);


  const off = useCallback((event: string) => {
    if (connectionRef.current) {
      connectionRef.current.off(event);
    }
    handlersRef.current.delete(event);
  }, []);

  const send = useCallback(async (method: string, ...args: any[]) => {
    const connection = connectionRef.current;

    if (!connection) return;

    if (connection.state !== "Connected") {
      try {
        await connection.start();
        setConnected(true);
      } catch (err) {
        console.error("SignalR reconnect failed:", err);
        return;
      }
    }

    try {
      await connection.invoke(method, ...args);
    } catch (err) {
      console.error("SignalR invoke error:", err);
    }
  }, []);

  return {
    send,
    on,
    off,
    connected
  };
};
