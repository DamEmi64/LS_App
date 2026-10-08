import { createRoot } from "react-dom/client";
import App from "./app/App.tsx";
import "./index.css";
import { appStorage } from './shared/storage/appStorage.ts';
import { loadRemoteAppContent } from './shared/firebase/firestoreContent';
import { i18nReady } from './shared/localization/i18n';

async function bootstrap() {
  try {
    await appStorage.initialize();
  } catch (error) {
    // A device can still run the app if its database cannot be opened.
    console.error('Unable to initialize app storage; using browser storage.', error);
  }

  try {
  //  await Promise.allSettled([loadRemoteAppContent(), i18nReady]);
  } catch (error) {
    // Bundled JSON remains available when Firestore is unavailable.
    console.warn('Unable to load remote app content; using bundled JSON.', error);
  }

  createRoot(document.getElementById("root")!).render(<App />);
}

void bootstrap();
