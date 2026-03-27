import { useTranslation } from 'react-i18next';
import { format } from 'react-string-format';
import { t } from 'i18next';

function getNotify(messageId, args = []) {
    if (messageId === 1000) {
        return args[0];
    }

    const template = t('notify.messages.' + messageId) || messageId;
    return format(template, ...args);
}

export { getNotify };