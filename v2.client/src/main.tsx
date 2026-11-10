import { createRoot } from "react-dom/client";
import App from "./app/App.tsx";
import "./index.css";
import { appStorage } from './shared/storage/appStorage.ts';
import { i18nReady } from './shared/localization/i18n';

async function bootstrap() {
  try {
    await appStorage.initialize();
  } catch (error) {
    // A device can still run the app if its database cannot be opened.
    console.error('Unable to initialize app storage; using browser storage.', error);
  }

  try {
    await i18nReady;
  } catch (error) {
    console.warn('Unable to initialize translations.', error);
  }

  createRoot(document.getElementById("root")!).render(<App />);
}

void bootstrap();
