
import {
  createContext,
  useContext,
  useEffect,
  useRef,
  type ReactNode,
} from "react";

type EventCallback<T = unknown> = (data: T) => void;

type EventBus = {
  subscribe: <T>(
    event: string,
    callback: EventCallback<T>
  ) => () => void;

  publish: <T>(event: string, data: T) => void;
};

const EventContext = createContext<EventBus | null>(null);

export const EVENT_MESSAGES = {
   UserLogIn: 'UserLogIn',
} as const;

export function EventProvider({
  children,
}: {
  children: ReactNode;
}) {
  const listeners = useRef(
    new Map<string, Set<EventCallback>>()
  );

  const subscribe: EventBus["subscribe"] = (
    event,
    callback
  ) => {
    let callbacks = listeners.current.get(event);

    if (!callbacks) {
      callbacks = new Set();
      listeners.current.set(event, callbacks);
    }

    callbacks.add(callback as EventCallback);

    // Return unsubscribe function
    return () => {
      callbacks!.delete(callback as EventCallback);

      if (callbacks!.size === 0) {
        listeners.current.delete(event);
      }
    };
  };

  const publish: EventBus["publish"] = (event, data) => {
    listeners.current
      .get(event)
      ?.forEach((callback) => callback(data));
  };

  return (
    <EventContext.Provider value={{ subscribe, publish }}>
      {children}
    </EventContext.Provider>
  );
}

export function useEventBus() {
  const context = useContext(EventContext);

  if (!context) {
    throw new Error(
      "useEventBus must be used inside EventProvider"
    );
  }

  return context;
}

export function useEvent<T>(
  event: string,
  callback: (data: T) => void
) {
  const { subscribe } = useEventBus();
  const callbackRef = useRef(callback);

  useEffect(() => {
    callbackRef.current = callback;
  }, [callback]);

  useEffect(() => {
    return subscribe<T>(event, (data) => {
      callbackRef.current(data);
    });
  }, [event, subscribe]);
}
