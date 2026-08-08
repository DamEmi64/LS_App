import { saveAs } from "file-saver";

import { getMimeFromExtension } from "@/lib/utils";
import { call, raw } from "@/shared/components/apiClient";
import { UserData } from "@/features/auth";

import { FileEditFormData, FileUser, FileV2, Privilage, PrivilageToSend } from "../types";

export async function loadFiles(directoryId: string | null) {
  return call<FileV2[]>(api => api.filesV2Api.get, {
    directoryId: directoryId ?? undefined,
  });
}

export async function createFileEntry(file: FileEditFormData, directoryId: string | null) {
  return call(api => api.filesV2Api.create, {
    file: file.File,
    description: file.description,
    directoryId: directoryId ?? undefined,
    title: file.title,
  });
}

export async function deleteFileEntry(id: string) {
  return call(api => api.filesV2Api.deleteById, { id });
}

export async function updateFileEntry(id: string, form: FileEditFormData) {
  return call(api => api.filesV2Api.updateById, {
    id,
    title: form.title,
    description: form.description,
    file: form.File,
  });
}

export function loadFileUsers(id: string) {
  return call<FileUser[]>(api => api.filesV2Api.getByIdUsers, { id });
}

export function loadShareableUsers() {
  return call<{ data: UserData[] }>(api => api.homeApi.getUsers, {});
}

export function grantFileAccess(id: string, login: string, userId: string, privilage: PrivilageToSend) {
  return call(api => api.filesV2Api.createByIdUsers, {
    id,
    grantAccessDto: { login, userId, privilage },
  });
}

export function revokeFileAccess(id: string, userId: string) {
  return call(api => api.filesV2Api.deleteByIdUsersByUserId, { id, userId });
}

export function setFilePublicStatus(id: string, isPublic: boolean) {
  return call<FileV2>(api => api.filesV2Api.updateById, { id, _public: isPublic });
}

export function getFilePrivilage(file: FileV2, user?: UserData | null): Privilage {
  if (!user) {
    return Privilage.NONE;
  }

  if (user.userId === file.owner) {
    return Privilage.OWNER;
  }

  if (!file.fileUsers) {
    return Privilage.NONE;
  }

  const fileUser = file.fileUsers.find(x => x.userId === user.userId);

  return (fileUser?.privilage as Privilage) ?? Privilage.NONE;
}

export async function downloadFile(file: FileV2) {
  const response = await raw(api => api.filesV2Api.getByIdDownload, { id: file.id });

  const filename = `${file.title}${response.data.extension}`;
  const mime = getMimeFromExtension(response.data.extension);
  const byteCharacters = atob(response.data.content);
  const byteNumbers = new Array(byteCharacters.length);

  for (let i = 0; i < byteCharacters.length; i += 1) {
    byteNumbers[i] = byteCharacters.charCodeAt(i);
  }

  const byteArray = new Uint8Array(byteNumbers);
  const blob = new Blob([byteArray], { type: mime });

  saveAs(blob, filename);
}
