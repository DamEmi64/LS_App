import { ResponseList } from "@/shared/api/extension";
import { call } from "@/shared/components/apiClient";
import { SystemAutomat } from "../types";


export async function loadAutomations(query: Record<string, string>) {
  const result = await call<ResponseList<SystemAutomat>>(api => api.systemAutomationApi.get, query);

  return {
    data: result.data,
    total: result.total,
  };
}

export async function activateAutomat(id: string) {
  return call(api => api.systemAutomationApi.updateByIdActivate, { id });
}

export async function deactivateAutomat(id: string) {
  return call(api => api.systemAutomationApi.updateByIdDeactivate, { id });
}

export async function updateConfiguration(id: string, configuration: string) {
  return call(api => api.systemAutomationApi.updateByIdConfiguration, { id, body: configuration });
}
