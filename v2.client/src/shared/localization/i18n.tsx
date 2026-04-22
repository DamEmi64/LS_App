import i18n from "i18next";
import Backend from "i18next-http-backend";
import LanguageDetector from "i18next-browser-languagedetector";
import { initReactI18next } from "react-i18next";

i18n
  .use(LanguageDetector)
  .use(Backend)
  .use(initReactI18next)
  .init({
    ns: ["translation", "dictionaries"],
    defaultNS: "translation",

    fallbackLng: "en",
    debug: true,

    backend: {
      loadPath: "/locales/{{lng}}/{{ns}}.json"
    },

    react: {
      useSuspense: false
    }
  });

export default i18n;