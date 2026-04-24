import { format } from 'react-string-format';

import i18n from "i18next";

export function getNotify(messageId: number | string, args: any[] = []) {
    if (messageId === 1000) {
        return args[0];
    }

    const template =
        i18n.t(`dictionaries:Notify_types.${messageId}.title`, {
            defaultValue: String(messageId)
        });

    return format(template, ...args);
}