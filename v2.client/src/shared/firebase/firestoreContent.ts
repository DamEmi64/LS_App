import { setRemoteConfiguration } from '@/app/configurationData';
import { setRemoteDictionaries } from '@/app/dictionaryData';
import { getApps, initializeApp } from 'firebase/app';
import { doc, getDoc, getFirestore } from 'firebase/firestore/lite';

const firebaseConfig = {
  apiKey: "AIzaSyDYwhcfSKZ4M0-UXzFJZTcBS1fPdS030fo",
  authDomain: "lsfamilia-app.firebaseapp.com",
  projectId: "lsfamilia-app",
  storageBucket: "lsfamilia-app.firebasestorage.app",
  messagingSenderId: "921053108358",
  appId: "1:921053108358:web:a42278062bab73c5f65907"
};

function getClientFirestore() {
  if (!firebaseConfig.apiKey || !firebaseConfig.projectId) {
    throw new Error('Firebase Web app config is missing. Set VITE_FIREBASE_API_KEY and VITE_FIREBASE_PROJECT_ID.');
  }

  const app = getApps()[0] ?? initializeApp(firebaseConfig);
  return getFirestore(app);
}

export async function readFirestoreDocument<T>(documentId: string): Promise<T> {
  const snapshot = await getDoc(doc(getClientFirestore(), 'clientContent', documentId));
  if (!snapshot.exists()) throw new Error(`Firestore document ${documentId} does not exist`);
  return snapshot.data() as T;
}

export async function loadRemoteAppContent() {
  if (!firebaseConfig.apiKey || !firebaseConfig.projectId) return;

  const [configuration, dictionaries] = await Promise.all([
    readFirestoreDocument<Record<string, unknown>>('configuration'),
    readFirestoreDocument<Record<string, unknown>>('dictionaries'),
  ]);

  setRemoteConfiguration(configuration);
  setRemoteDictionaries(dictionaries);
}
