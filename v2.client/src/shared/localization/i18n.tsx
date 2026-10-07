import i18n from "i18next";
import type { BackendModule, ReadCallback } from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import { initReactI18next } from "react-i18next";
import { readFirestoreDocument } from '@/shared/firebase/firestoreContent';

const firestoreBackend: BackendModule = {
  type: 'backend',
  init() {},
  read(language: string, namespace: string, callback: ReadCallback) {
    void readFirestoreDocument<Record<string, unknown>>(`translations_${language}_${namespace}`)
      .then(resources => callback(null, resources))
      .catch(async () => {
        try {
          const response = await fetch(`/locales/${language}/${namespace}.json`);
          if (!response.ok) throw new Error(`Local ${language}/${namespace} fallback is unavailable`);
          callback(null, await response.json());
        } catch (error) {
          callback(error as Error, false);
        }
      });
  }
};

const debugBackend: BackendModule = {
  type: 'backend',
  init() {},
  read(language: string, namespace: string, callback: ReadCallback) {
    const response = fetch(`/locales/${language}/${namespace}.json`).then(response =>
      response.json().then(json => callback(null, json)) 
    );      
  }
};

i18n
  .use(LanguageDetector)
  .use(debugBackend)
  .use(initReactI18next)
  ;



export const i18nReady = i18n.init({
    ns: ["translation", "dictionaries"],
    defaultNS: "translation",

    fallbackLng: "en",
    debug: true,

    react: {
      useSuspense: false
    }
  });

export default i18n;
