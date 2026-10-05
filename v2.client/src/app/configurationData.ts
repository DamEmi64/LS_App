import fallbackConfiguration from './configuration.json';
import type { NavbarItemProps } from '@/shared/types';

type AppConfiguration = typeof fallbackConfiguration;
export type ConfigKey = {
  [Key in keyof AppConfiguration]: AppConfiguration[Key] extends string ? Key : never
}[keyof AppConfiguration];

export type AppMenuItem = NavbarItemProps;

let configuration: AppConfiguration = fallbackConfiguration;

export function setRemoteConfiguration(value: Record<string, unknown>) {
  configuration = { ...fallbackConfiguration, ...value } as AppConfiguration;
}

export function getConfiguration() {
  return configuration;
}
