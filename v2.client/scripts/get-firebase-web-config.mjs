import { readFile, writeFile } from 'node:fs/promises';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import { cert } from 'firebase-admin/app';

const repoRoot = resolve(dirname(fileURLToPath(import.meta.url)), '..');
const keyPath = process.env.GOOGLE_APPLICATION_CREDENTIALS || resolve(repoRoot, 'firebase/service-account.json');
const serviceAccount = JSON.parse(await readFile(keyPath, 'utf8'));

if (!serviceAccount.project_id || !serviceAccount.client_email || !serviceAccount.private_key) {
  throw new Error('The service-account JSON is missing project_id, client_email, or private_key.');
}

const appIdIndex = process.argv.indexOf('--app-id');
const requestedAppId = appIdIndex >= 0 ? process.argv[appIdIndex + 1] : undefined;
if (appIdIndex >= 0 && !requestedAppId) throw new Error('Provide an app ID after --app-id.');

const token = await cert(serviceAccount).getAccessToken();
const authHeaders = { Authorization: `Bearer ${token.access_token}` };
const project = encodeURIComponent(serviceAccount.project_id);
const listResponse = await fetch(`https://firebase.googleapis.com/v1beta1/projects/${project}/webApps`, {
  headers: authHeaders,
});
if (!listResponse.ok) {
  throw new Error(`Unable to list Firebase web apps (${listResponse.status}): ${await listResponse.text()}`);
}

const apps = (await listResponse.json()).apps ?? [];
let selectedApp = requestedAppId
  ? apps.find(app => app.appId === requestedAppId)
  : apps.length === 1 ? apps[0] : undefined;

if (apps.length === 0 && !requestedAppId) {
  console.log('No Firebase Web app is registered. Creating “LS App Client”…');
  const createResponse = await fetch(`https://firebase.googleapis.com/v1beta1/projects/${project}/webApps`, {
    method: 'POST',
    headers: { ...authHeaders, 'Content-Type': 'application/json' },
    body: JSON.stringify({ displayName: 'LS App Client' }),
  });
  if (!createResponse.ok) {
    throw new Error(`Unable to register a Firebase Web app (${createResponse.status}): ${await createResponse.text()}`);
  }

  const operation = await createResponse.json();
  for (let attempt = 0; attempt < 60; attempt += 1) {
    if (operation.done) {
      if (operation.error) throw new Error(`Firebase Web app registration failed: ${JSON.stringify(operation.error)}`);
      selectedApp = operation.response;
      break;
    }

    await new Promise(resolveDelay => setTimeout(resolveDelay, 1000));
    const operationResponse = await fetch(`https://firebase.googleapis.com/v1beta1/${operation.name}`, {
      headers: authHeaders,
    });
    if (!operationResponse.ok) {
      throw new Error(`Unable to check Web app registration (${operationResponse.status}): ${await operationResponse.text()}`);
    }
    Object.assign(operation, await operationResponse.json());
  }

  if (!selectedApp) throw new Error('Timed out while registering the Firebase Web app. Run the script again to retrieve its config.');
}

if (!selectedApp) {
  const availableApps = apps.map(app => `${app.displayName || '(unnamed)'}: ${app.appId}`).join('\n');
  throw new Error(`Choose a Web app with --app-id. Available apps:\n${availableApps}`);
}

const configResponse = await fetch(
  `https://firebase.googleapis.com/v1beta1/projects/${project}/webApps/${encodeURIComponent(selectedApp.appId)}/config`,
  { headers: authHeaders },
);
if (!configResponse.ok) {
  throw new Error(`Unable to get Firebase Web app config (${configResponse.status}): ${await configResponse.text()}`);
}

const webConfig = await configResponse.json();
if (!webConfig.apiKey) throw new Error('Firebase returned a Web app config without an API key.');

const envPath = resolve(repoRoot, '.env.local');
let envLines = [];
try {
  envLines = (await readFile(envPath, 'utf8')).replace(/^\uFEFF/, '').split(/\r?\n/);
} catch (error) {
  if (error.code !== 'ENOENT') throw error;
}

const clientConfig = {
  VITE_FIREBASE_API_KEY: webConfig.apiKey,
  VITE_FIREBASE_AUTH_DOMAIN: webConfig.authDomain,
  VITE_FIREBASE_PROJECT_ID: webConfig.projectId,
  VITE_FIREBASE_STORAGE_BUCKET: webConfig.storageBucket,
  VITE_FIREBASE_MESSAGING_SENDER_ID: webConfig.messagingSenderId,
  VITE_FIREBASE_APP_ID: webConfig.appId,
};
const clientConfigKeys = new Set(Object.keys(clientConfig));
envLines = envLines.filter(line => !clientConfigKeys.has(line.split('=', 1)[0].trim()));
for (const [key, value] of Object.entries(clientConfig)) {
  if (value) envLines.push(`${key}=${value}`);
}
await writeFile(envPath, `${envLines.filter(Boolean).join('\n')}\n`);

console.log(`Updated ${envPath} from Firebase Web app ${selectedApp.appId}.`);
console.log('The API key is a public client config value; the service-account key was not written to the app config.');
