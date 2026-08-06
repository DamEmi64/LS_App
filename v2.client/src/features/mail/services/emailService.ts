import { call } from "@/shared";
import { ResponseList } from "@/shared/api/extension";
import { Email } from "../index";

export async function loadEmails(query: Record<string, string>) {
  const result = await call<ResponseList<Email>>(api => api.emailsApi.get, query);

  return {
    data: result.data,
    total: result.total,
  };
}

export async function getEmailById(id: string) {
  return call<Email>(api => api.emailsApi.getById, { id });
}

export async function createEmail(email: Email) {
  return call(api => api.emailsApi.create, { email });
}

export async function updateEmail(id: string, email: Email) {
  return call(api => api.emailsApi.updateById, { id, email });
}

export async function deleteEmail(id: string) {
  return call(api => api.emailsApi.deleteById, { id });
}

export async function sendEmail(id: string) {
  return call(api => api.emailsApi.updateByIdSend, { id });
}
