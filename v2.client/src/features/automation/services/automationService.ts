import { call } from "@/shared";
import { ResponseList } from "@/shared/api/extension";
import { Automat } from "../types";

export async function loadAutomations(query: Record<string, string>) {
  const result = await call<ResponseList<Automat>>(api => api.automationApi.get, query);

  return {
    data: result.data,
    total: result.total,
  };
}

export async function createAutomation(automat: Automat) {
  return call(api => api.automationApi.create, { automationDto: automat });
}

export async function updateAutomation(id: string, automat: Automat) {
  return call(api => api.automationApi.updateById, { id, automationDto: automat });
}

export async function deleteAutomation(id: string) {
  return call(api => api.automationApi.deleteById, { id });
}

export async function turnOnAutomation(id: string) {
  return call(api => api.automationApi.updateByIdTurnon, { id });
}

export async function turnOffAutomation(id: string) {
  return call(api => api.automationApi.updateByIdTurnoff, { id });
}

export async function getAutomationById(id: string) {
  return call<Automat>(api => api.automationApi.getById, { id });
}
