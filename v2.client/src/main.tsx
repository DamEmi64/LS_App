import { createRoot } from "react-dom/client";
import App from "./app/App.tsx";
import "./index.css";
import { appStorage } from './shared/storage/appStorage.ts';

async function bootstrap() {
  try {
    await appStorage.initialize();
  } catch (error) {
    // A device can still run the app if its database cannot be opened.
    console.error('Unable to initialize app storage; using browser storage.', error);
  }

  createRoot(document.getElementById("root")!).render(<App />);
}

void bootstrap();
