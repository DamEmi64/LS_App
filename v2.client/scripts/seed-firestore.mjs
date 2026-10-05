import { readFile } from 'node:fs/promises';
import { createSign } from 'node:crypto';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const repoRoot = resolve(dirname(fileURLToPath(import.meta.url)), '..');
const keyPath = './firebase/service-account.json';

if (!keyPath) {
  throw new Error('Set GOOGLE_APPLICATION_CREDENTIALS to the local service-account JSON file path.');
}

const serviceAccount = JSON.parse(await readFile(keyPath, 'utf8'));
if (!serviceAccount.project_id || !serviceAccount.client_email || !serviceAccount.private_key) {
  throw new Error('The service-account JSON is missing project_id, client_email, or private_key.');
}

const documents = [
  ['configuration', 'src/app/configuration.json'],
  ['dictionaries', 'src/app/dictionaries.json'],
  ...['en', 'pl', 'fr', 'de'].flatMap(language => [
    [`translations_${language}_translation`, `public/locales/${language}/translation.json`],
    [`translations_${language}_dictionaries`, `public/locales/${language}/dictionaries.json`],
  ]),
];
const updateMenuOnly = process.argv.includes('--update-menu');

function encodeBase64Url(value) {
  return Buffer.from(value).toString('base64url');
}

async function getAccessToken() {
  const issuedAt = Math.floor(Date.now() / 1000);
  const header = encodeBase64Url(JSON.stringify({ alg: 'RS256', typ: 'JWT' }));
  const claim = encodeBase64Url(JSON.stringify({
    iss: serviceAccount.client_email,
    scope: 'https://www.googleapis.com/auth/datastore',
    aud: 'https://oauth2.googleapis.com/token',
    iat: issuedAt,
    exp: issuedAt + 3600,
  }));
  const unsignedToken = `${header}.${claim}`;
  const signer = createSign('RSA-SHA256');
  signer.update(unsignedToken);
  const assertion = `${unsignedToken}.${signer.sign(serviceAccount.private_key, 'base64url')}`;
  const response = await fetch('https://oauth2.googleapis.com/token', {
    method: 'POST',
    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    body: new URLSearchParams({
      grant_type: 'urn:ietf:params:oauth:grant-type:jwt-bearer',
      assertion,
    }),
  });

  if (!response.ok) throw new Error(`Google OAuth failed (${response.status}): ${await response.text()}`);
  return (await response.json()).access_token;
}

function encodeFirestoreValue(value) {
  if (value === null) return { nullValue: 'NULL_VALUE' };
  if (typeof value === 'string') return { stringValue: value };
  if (typeof value === 'boolean') return { booleanValue: value };
  if (typeof value === 'number') {
    return Number.isInteger(value) ? { integerValue: String(value) } : { doubleValue: value };
  }
  if (Array.isArray(value)) {
    return { arrayValue: { values: value.map(encodeFirestoreValue) } };
  }
  if (typeof value === 'object') {
    return { mapValue: { fields: encodeFirestoreFields(value) } };
  }
  throw new Error(`Unsupported Firestore value type: ${typeof value}`);
}

function encodeFirestoreFields(value) {
  return Object.fromEntries(Object.entries(value).map(([key, item]) => [key, encodeFirestoreValue(item)]));
}

const accessToken = await getAccessToken();
const databasePath = `projects/${encodeURIComponent(serviceAccount.project_id)}/databases/(default)/documents`;

for (const [documentId, relativePath] of documents) {
  const contents = JSON.parse(await readFile(resolve(repoRoot, relativePath), 'utf8'));
  const url = `https://firestore.googleapis.com/v1/${databasePath}/clientContent?documentId=${encodeURIComponent(documentId)}`;
  const response = await fetch(url, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${accessToken}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ fields: encodeFirestoreFields(contents) }),
  });

  if (response.status === 409) {
    if (documentId === 'configuration' && updateMenuOnly) {
      const updateUrl = `https://firestore.googleapis.com/v1/${databasePath}/clientContent/configuration?updateMask.fieldPaths=menu`;
      const updateResponse = await fetch(updateUrl, {
        method: 'PATCH',
        headers: {
          Authorization: `Bearer ${accessToken}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ fields: { menu: encodeFirestoreValue(contents.menu) } }),
      });
      if (!updateResponse.ok) {
        throw new Error(`Unable to update the configuration menu (${updateResponse.status}): ${await updateResponse.text()}`);
      }
      console.log('Updated clientContent/configuration menu');
      continue;
    }
    console.log(`Skipped existing document clientContent/${documentId}`);
    continue;
  }
  if (!response.ok) throw new Error(`Unable to seed ${documentId} (${response.status}): ${await response.text()}`);
  console.log(`Created clientContent/${documentId}`);
}
