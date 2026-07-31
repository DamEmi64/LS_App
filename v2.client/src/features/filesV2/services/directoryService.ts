import { call } from "@/shared/components/apiClient";

import { Directory } from "../types";

export interface BreadcrumbItem {
  id: string | null;
  title: string;
}

export async function loadDirectories(directoryId: string | null) {
  return call<Directory[]>(api => api.directoriesApi.get, {
    parentId: directoryId ?? undefined,
  });
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
