import { ResponseList } from "@/shared/api/extension";
import { axiosInstance } from "@/shared/components/apiClient";
import { SystemAutomat } from "../types";


export async function loadAutomations(query: Record<string, string>) {
  const response = await axiosInstance.get<ResponseList<SystemAutomat>>("/api/SystemAutomate", { params: query });
  const result = response.data;

  return {
    data: result.data,
    total: result.total,
  };
}

export async function activateAutomat(id: string) {
  return axiosInstance.put(`/api/SystemAutomate/${id}/activate`);
}

export async function deactivateAutomat(id: string) {
  return axiosInstance.put(`/api/SystemAutomate/${id}/Deactivate`);
}

export async function updateConfiguration(id: string, configuration: string) {
  return axiosInstance.put(`/api/SystemAutomate/${id}/configuration`, JSON.stringify(configuration), {
    headers: { "Content-Type": "application/json" },
  });
}
