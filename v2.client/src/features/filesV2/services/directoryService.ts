import { call } from "@/shared/components/apiClient";

import { Directory, FileUser, Privilage, PrivilageToSend } from "../types";
import { UserData } from "@/features/system";

export interface BreadcrumbItem {
  id: string | null;
  title: string;
}

export async function loadDirectories(directoryId: string | null) {
  return call<Directory[]>(api => api.directoriesApi.get, {
    parentId: directoryId ?? undefined,
  });
}

export function getDirPrivilage(directory: Directory, user?: UserData | null): Privilage {
  if (!user) {
    return Privilage.NONE;
  }

  if (user.userId === directory.owner) {
    return Privilage.OWNER;
  }

  if (!directory.fileUsers) {
    return Privilage.NONE;
  }

  const fileUser = directory.fileUsers.find(x => x.userId === user.userId);

  return (fileUser?.privilage as Privilage) ?? Privilage.NONE;
}

export function loadDirUsers(id: string) {
  return call<FileUser[]>(api => api.directoriesApi.getByIdUsers, { id });
}

export function loadShareableUsers() {
  return call<{ data: UserData[] }>(api => api.homeApi.getUsers, {});
}

export function grantDirAccess(id: string, login: string, userId: string, privilage: PrivilageToSend) {
  return call(api => api.directoriesApi.createByIdUsers, {
    id,
    grantAccessDto: { login, userId, privilage },
  });
}

export function setDirPublicStatus(directoryId: string, isPublic: boolean) {
  return call(api => api.directoriesApi.updateById, {
    id: directoryId,
    updateDirectoryDto: { public: isPublic },
  });
}

export function revokeDirAccess(id: string, userId: string) {
  return call(api => api.directoriesApi.deleteByIdUsersByUserId, { id, userId });
}

export async function createDirectoryEntry(title: string, directoryId: string | null) {
  return call(api => api.directoriesApi.create, {
    createDirectoryDto: {
      title,
      parentId: directoryId ?? undefined,
    },
  });
}

export async function deleteDirectoryEntry(id: string) {
  return call(api => api.directoriesApi.deleteById, { id });
}

export async function getDirectoryPath(id: string | null) {
  if (id === null) {
    return [{ id: null, title: "All files" }];
  }

  const trail: BreadcrumbItem[] = [];
  let current = await call<Directory>(api => api.directoriesApi.getById, { id });

  while (current) {
    trail.unshift({
      id: current.id,
      title: current.title ?? "",
    });

    current = current.parentId
      ? await call<Directory>(api => api.directoriesApi.getById, {
          id: current.parentId,
        })
      : null;
  }

  return [{ id: null, title: "All files" }, ...trail];
}
