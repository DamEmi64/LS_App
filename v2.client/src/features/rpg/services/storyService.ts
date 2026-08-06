import { call, raw } from "@/shared";
import { saveAs } from "file-saver";

import { SessionDto, Story } from "../index";
import { download } from "@/lib/utils";

export async function createStory(data: Story) {
  return call(api => api.storiesApi.create, { storyDto: { storyDto: data } });
}

export async function importStory(file: File, converterType: number, externalUrl: string) {
  return call(api => api.storiesApi.createImport, {
    file,
    converterType,
    externalUrl,
  });
}

export async function getStoryById(id: string, draft?: boolean) {
  return call<Story>(api => draft ? api.storiesApi.getByIdDraft : api.storiesApi.getById, { id });
}

export async function updateStory(data: Story) {
  return call(api => api.storiesApi.updateById, { id: data.id, storyDto: data });
}

export async function createChapter(data: SessionDto) {
  return call(api => api.chaptersApi.create, { chapterDto: data });
}

export async function deleteStory(id: string) {
  return call(api => api.storiesApi.deleteById, { id });
}

export async function startStory(id: string) {
  return call(api => api.storiesApi.updateByIdStart, { id });
}

export async function endStory(id: string) {
  return call(api => api.storiesApi.updateByIdEnd, { id });
}

export async function generateStorySummary(data: Story, isPdf: boolean) {
  return call(api => api.storiesApi.updateByIdSummary, {
    id: data.id,
    summaryModel: {
      id: data.id,
      title: data.title,
      description: data.description,
      chapters: data.chapters.map((x: any) => x.id),
      isPdf,
    },
  });
}

export async function sendStoryToFirebase(data: Story) {
  return call(api => api.storiesApi.updateByIdFirebase, {
    id: data.id,
    summaryModel: {
      id: data.id,
      title: data.title,
      description: data.description,
      chapters: data.chapters.map((x: any) => x.id),
    },
  });
}

export async function exportStory(data: Story) {
  const response = await raw(api => api.storiesApi.getByIdExport, { id: data.id });
  const contentType = response.headers['content-type'] || 'application/octet-stream';
  const disposition = response.headers['content-disposition'];
  let filename = `${data.title}.json`;

  if (disposition) {
    const match = disposition.match(/filename\*?=(?:UTF-8'')?([^;]+)/);
    if (match?.[1]) filename = decodeURIComponent(match[1].replace(/"/g, ''));
  }

  const blob = new Blob([response.data], { type: contentType.toLocaleString() });
  saveAs(blob, filename);
}

export function downloadStorySummary(data: Story) {
  download(data.summary, data.title);
}
