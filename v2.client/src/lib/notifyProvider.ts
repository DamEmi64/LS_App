import { format } from 'react-string-format';

import i18n from "i18next";

export function getNotify(messageId: number | string, args: any[] = []) {
    const template =
        i18n.t(`dictionaries:Logs.${messageId}.title`, {
            defaultValue: String(messageId)
        });

    return format(template, ...args);
}
