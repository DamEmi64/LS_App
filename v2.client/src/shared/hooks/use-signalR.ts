import { useEffect, useRef, useState, useCallback } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel
} from "@microsoft/signalr";
import { useApiConnect } from "@/shared/context/apiConnect";

type Handler = (...args: any[]) => void;

export const useSignalR = (hubName: string, onConnected?: () => void) => {
  const connectionRef = useRef<HubConnection | null>(null);
  const handlersRef = useRef<Map<string, Handler>>(new Map());

  const [connected, setConnected] = useState(false);

  const api = useApiConnect();

  const hubUrl = api.getUrl(hubName);

  // 🚀 Start connection
  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl(hubUrl, {
        withCredentials: true
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    connectionRef.current = connection;

    connection
      .start()
      .then(() => {
        console.log(`SignalR connected: ${hubName}`);
        setConnected(true);

        // 🔁 re-register handlers after reconnect
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
  }, [hubName]);

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

  const send = useCallback(async (method: string, payload?: any) => {
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
      await connection.invoke(method, payload);
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