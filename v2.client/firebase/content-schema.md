# Firestore app content

The client reads documents from the `clientContent` collection in the `(default)` database. Document reads are public and writes are denied by the included rules because these documents contain public app text and defaults only.

Create these documents and put each source JSON object's root properties directly on the document. You can seed missing documents from a local service-account key with `npm run firebase:seed-content` after setting `GOOGLE_APPLICATION_CREDENTIALS` to its path. The script creates missing documents and skips existing ones so it does not overwrite content already in Firestore. After changing the local configuration menu, run `npm run firebase:seed-content -- --update-menu` to update only the existing configuration document's `menu` field.

| Document ID | Source file |
| --- | --- |
| `configuration` | `src/app/configuration.json` |
| `dictionaries` | `src/app/dictionaries.json` |
| `translations_en_translation` | `public/locales/en/translation.json` |
| `translations_en_dictionaries` | `public/locales/en/dictionaries.json` |
| `translations_pl_translation` | `public/locales/pl/translation.json` |
| `translations_pl_dictionaries` | `public/locales/pl/dictionaries.json` |
| `translations_fr_translation` | `public/locales/fr/translation.json` |
| `translations_fr_dictionaries` | `public/locales/fr/dictionaries.json` |
| `translations_de_translation` | `public/locales/de/translation.json` |
| `translations_de_dictionaries` | `public/locales/de/dictionaries.json` |

Keep Firestore field names and values shaped like the JSON: nested objects become maps, arrays become arrays, and primitive values retain their JSON types. The client overlays remote values on bundled defaults. Locale documents are read as their language and namespace are requested, so locale changes can be published without rebuilding the app. When Firestore cannot be reached or a document is missing, the corresponding bundled JSON is used.

Set `VITE_FIREBASE_PROJECT_ID` and `VITE_FIREBASE_API_KEY` in the build environment using the Firebase project's web app settings. The API key is a public client identifier; do not put service account credentials in the app. Deploy `firestore.rules` before publishing the app.
