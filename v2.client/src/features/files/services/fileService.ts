import { saveAs } from "file-saver";

import { call, raw } from "@/shared";
import { ResponseList } from "@/shared/api/extension";

import { EditFile, File } from "../types";

export async function loadFiles(query: Record<string, string>) {
  const result = await call<ResponseList<File>>(api => api.filesApi.get, query);
  return result.data;
}

export function getFile(id: string) {
  return call<File>(api => api.filesApi.getById, { id });
}

export function createFile(file: EditFile) {
  return call(api => api.filesApi.create, { fileDto: file });
}

export function updateFile(id: string, file: EditFile) {
  return call(api => api.filesApi.updateById, { id, fileDto: file });
}

export function deleteFile(id: string) {
  return call(api => api.filesApi.deleteById, { id });
}

export async function exportFile(file: File) {
  const response = await raw(api => api.filesApi.getByIdExport, { id: file.id });
  const contentType = response.headers["content-type"] || "application/octet-stream";
  const blob = new Blob([response.data], { type: contentType.toLocaleString() });

  saveAs(blob, file.title);
}
