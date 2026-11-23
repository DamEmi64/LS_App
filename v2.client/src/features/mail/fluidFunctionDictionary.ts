import type { CommunicationRule } from "./types";

const dictionaryIdByInvoker: Record<string, number> = {
    randomNumber: 80301,
    random: 80302,
    randomUnique: 80303,
    increment: 80304,
};

export function getFluidFunctionDictionaryId(rule: CommunicationRule) {
    return dictionaryIdByInvoker[rule.invoker] ?? rule.id;
}
