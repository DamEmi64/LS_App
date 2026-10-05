import { setRemoteConfiguration } from '@/app/configurationData';
import { setRemoteDictionaries } from '@/app/dictionaryData';

const projectId = import.meta.env.VITE_FIREBASE_PROJECT_ID;
const apiKey = import.meta.env.VITE_FIREBASE_API_KEY;

function decodeValue(value: any): any {
  if ('stringValue' in value) return value.stringValue;
  if ('integerValue' in value) return Number(value.integerValue);
  if ('doubleValue' in value) return value.doubleValue;
  if ('booleanValue' in value) return value.booleanValue;
  if ('nullValue' in value) return null;
  if ('timestampValue' in value) return value.timestampValue;
  if ('arrayValue' in value) return (value.arrayValue.values ?? []).map(decodeValue);
  if ('mapValue' in value) return decodeFields(value.mapValue.fields ?? {});
  return undefined;
}

function decodeFields(fields: Record<string, unknown>) {
  return Object.fromEntries(Object.entries(fields).map(([key, value]) => [key, decodeValue(value)]));
}

export async function readFirestoreDocument<T>(documentId: string): Promise<T> {
  if (!projectId) throw new Error('VITE_FIREBASE_PROJECT_ID is not configured');

  const keyParameter = apiKey ? `?key=${encodeURIComponent(apiKey)}` : '';
  const url = `https://firestore.googleapis.com/v1/projects/${encodeURIComponent(projectId)}/databases/(default)/documents/clientContent/${encodeURIComponent(documentId)}${keyParameter}`;
  const controller = new AbortController();
  const timeout = window.setTimeout(() => controller.abort(), 4000);

  try {
    const response = await fetch(url, { signal: controller.signal, cache: 'no-store' });
    if (!response.ok) throw new Error(`Firestore returned ${response.status} for ${documentId}`);
    const document = await response.json();
    return decodeFields(document.fields ?? {}) as T;
  } finally {
    window.clearTimeout(timeout);
  }
}

export async function loadRemoteAppContent() {
  if (!projectId) return;

  const [configuration, dictionaries] = await Promise.all([
    readFirestoreDocument<Record<string, unknown>>('configuration'),
    readFirestoreDocument<Record<string, unknown>>('dictionaries'),
  ]);

  setRemoteConfiguration(configuration);
  setRemoteDictionaries(dictionaries);
}
