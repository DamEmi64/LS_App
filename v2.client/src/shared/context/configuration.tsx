import {
  createContext,
  ReactNode,
  useContext,
} from 'react';

import configuration from '@/app/configuration.json';
import { useAppStorage } from '@/shared/storage/useAppStorage';

type ConfigKey = keyof typeof configuration;

type ConfigContextType = {
  get: (key: ConfigKey) => string;
  useVariable: (key: ConfigKey) => any;
};

const ConfigurationContext = createContext<ConfigContextType | null>(null);

export function ConfigurationProvider({ children }: { children: ReactNode }) {

  const get = (key: ConfigKey) => {
    return configuration[key] ?? '';
  };

  const useVariable = (key: ConfigKey) => {
    const [value, setValue] = useAppStorage(
      key,
      configuration[key] ?? ''
    );

    return [value, setValue] as const;
  };

  return (
    <ConfigurationContext.Provider value={{ get, useVariable }}>
      {children}
    </ConfigurationContext.Provider>
  );
}

export function useConfiguration() {
  const ctx = useContext(ConfigurationContext);

  if (!ctx) {
    throw new Error('useConfiguration must be used within ConfigurationProvider');
  }

  return ctx;
}
