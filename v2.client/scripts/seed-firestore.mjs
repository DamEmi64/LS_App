import { mkdir, readFile, writeFile } from 'node:fs/promises';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import { cert, initializeApp } from 'firebase-admin/app';
import { getFirestore } from 'firebase-admin/firestore';

const repoRoot = resolve(dirname(fileURLToPath(import.meta.url)), '..');
const keyPath = process.env.GOOGLE_APPLICATION_CREDENTIALS || resolve(repoRoot, 'firebase/service-account.json');
const dictionariesArgumentIndex = process.argv.indexOf('--dictionaries');
const generatedDictionariesPath = dictionariesArgumentIndex >= 0 ? process.argv[dictionariesArgumentIndex + 1] : undefined;
const downloadContent = process.argv.includes('--download-content');
const outputArgumentIndex = process.argv.indexOf('--output');
const downloadOutputPath = outputArgumentIndex >= 0 ? process.argv[outputArgumentIndex + 1] : undefined;

if (dictionariesArgumentIndex >= 0 && !generatedDictionariesPath) {
  throw new Error('Provide a directory after --dictionaries.');
}
if (downloadContent && !downloadOutputPath) {
  throw new Error('Provide a directory after --output.');
}

const serviceAccount = JSON.parse(await readFile(keyPath, 'utf8'));
if (!serviceAccount.project_id || !serviceAccount.client_email || !serviceAccount.private_key) {
  throw new Error('The service-account JSON is missing project_id, client_email, or private_key.');
}

const firebaseApp = initializeApp({
  credential: cert(serviceAccount),
  projectId: serviceAccount.project_id,
});
const firestore = getFirestore(firebaseApp);

const contentDocuments = [
  ['configuration', 'src/app/configuration.json'],
  ['dictionaries', 'src/app/dictionaries.json'],
  ...['en', 'pl', 'fr', 'de'].flatMap(language => [
    [`translations_${language}_translation`, `public/locales/${language}/translation.json`],
    [`translations_${language}_dictionaries`, `public/locales/${language}/dictionaries.json`],
  ]),
];
const dictionaryLanguages = ['en', 'pl', 'fr', 'de'];
const downloadedContentDocuments = [
  ['dictionaries', 'dictionaries.json'],
  ...dictionaryLanguages.flatMap(language => [
    [`translations_${language}_translation`, `${language}/translation.json`],
    [`translations_${language}_dictionaries`, `${language}/dictionaries.json`],
  ]),
];
const documents = downloadContent
  ? []
  : generatedDictionariesPath
  ? [
      ['dictionaries', resolve(generatedDictionariesPath, 'dictionaries.json')],
      ...dictionaryLanguages.flatMap(language => [
        [`translations_${language}_translation`, resolve(generatedDictionariesPath, language, 'translation.json')],
        [`translations_${language}_dictionaries`, resolve(generatedDictionariesPath, language, 'dictionaries.json')],
      ]),
    ]
  : contentDocuments.map(([documentId, relativePath]) => [documentId, resolve(repoRoot, relativePath)]);
const updateExisting = process.argv.includes('--update');

for (const [documentId, filePath] of documents) {
  const contents = JSON.parse(await readFile(filePath, 'utf8'));
  const document = firestore.collection('clientContent').doc(documentId);

  if (updateExisting) {
    await document.set(contents, { merge: true });
    console.log(`Updated clientContent/${documentId}`);
    continue;
  }

  try {
    await document.create(contents);
    console.log(`Created clientContent/${documentId}`);
  } catch (error) {
    if (error.code === 6 || error.code === 'already-exists') {
      console.log(`Skipped existing document clientContent/${documentId}`);
      continue;
    }
    throw error;
  }
}

if (downloadContent) {
  for (const [documentId, relativePath] of downloadedContentDocuments) {
    const snapshot = await firestore.collection('clientContent').doc(documentId).get();
    if (!snapshot.exists) throw new Error(`Firestore document clientContent/${documentId} does not exist.`);

    const outputFile = resolve(downloadOutputPath, relativePath);
    await mkdir(dirname(outputFile), { recursive: true });
    await writeFile(outputFile, `${JSON.stringify(snapshot.data(), null, 2)}\n`);
    console.log(`Downloaded clientContent/${documentId} to ${outputFile}`);
  }
}
