# Firestore app content

The client reads documents from the `clientContent` collection in the `(default)` database. Document reads are public and writes are denied by the included rules because these documents contain public app text and defaults only.

Create these documents and put each source JSON object's root properties directly on the document. Run `npm run firebase:sync-content` to create missing documents and update source fields in existing documents. The script uses `GOOGLE_APPLICATION_CREDENTIALS` when set, or defaults to `firebase/service-account.json`. It updates existing fields but does not delete remote fields that are absent from local files. Use `npm run firebase:seed-content` for create-only behavior.

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

Set the Firebase Web app configuration in the build environment using the Firebase project's web app settings. The API key is a public client identifier; do not put service account credentials in the app. Deploy `firestore.rules` before publishing the app.

To fetch the API key from a registered Firebase Web app and update the ignored `.env.local` file, run `npm run firebase:get-web-config`. It uses the local service-account JSON and Firebase Management API. If none is registered, the script creates a Web app named `LS App Client` first. If the project has multiple Web apps, pass the app ID with `npm run firebase:get-web-config -- --app-id YOUR_WEB_APP_ID`.
