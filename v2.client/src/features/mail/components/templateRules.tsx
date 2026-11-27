import { useEffect, useState } from "react";
import { Button } from "@mui/material";
import { useTranslation } from "react-i18next";
import { call } from "@/shared";
import HtmlViewer from "@/shared/components/HtmlViewer";
import { useModal } from "@/shared/context/modal";
import { useDictionaryTranslation } from "@/lib/utils";
import { CommunicationRules, CommunicationRule } from "../types";
import { getFluidFunctionDictionaryId } from "../fluidFunctionDictionary";

const escapeHtml = (value: string) => value.replace(/[&<>"']/g, character => ({
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': "&quot;",
    "'": "&#39;",
}[character] as string));

export const TemplateRules = () => {
    const { t, i18n } = useTranslation();
    const modal = useModal();
    const getDictionaryTranslation = useDictionaryTranslation();
    const [rules, setRules] = useState<CommunicationRules | null>(null);

    useEffect(() => {
        void call<CommunicationRules>(api => api.templatesApi.getRules, {}).then(setRules);
    }, []);

    const renderRules = (items: CommunicationRule[], dictionary: string, isFunction: boolean) => items.map(rule => {
        const dictionaryId = isFunction ? getFluidFunctionDictionaryId(rule) : rule.id;
        const translated = getDictionaryTranslation(dictionary, dictionaryId);
        const name = translated.title || rule.title;
        const syntax = isFunction ? `{{ ${rule.invoker}() }}` : `{{ ${rule.invoker} }}`;
        const description = isFunction ? translated.description : rule.description;
        return `<article><h3>${escapeHtml(name)}</h3><code>${escapeHtml(syntax)}</code>${description ? `<p>${escapeHtml(description)}</p>` : ""}</article>`;
    }).join("");

    const openGuide = async () => {
        const variables = rules?.variables ?? [];
        const functions = rules?.functions ?? [];
        const guideKey = "communication.template.strategies.guide";
        const response = await fetch("/template-guide.html");
        if (!response.ok) return;
        let html = await response.text();
        const replacements: Record<string, string> = {
            lang: i18n.language,
            title: t(`${guideKey}.title`),
            intro: t(`${guideKey}.intro`),
            syntaxTitle: t(`${guideKey}.syntaxTitle`),
            syntax: t(`${guideKey}.syntax`),
            variableSyntax: t(`${guideKey}.variableSyntax`),
            functionSyntax: t(`${guideKey}.functionSyntax`),
            parserHelp: t(`${guideKey}.parserHelp`),
            seqNote: t(`${guideKey}.seqNote`),
            dataTitle: t(`${guideKey}.dataTitle`),
            dataIntro: t(`${guideKey}.dataIntro`),
            dataSender: t(`${guideKey}.dataSender`),
            dataReceiver: t(`${guideKey}.dataReceiver`),
            dataReceivers: t(`${guideKey}.dataReceivers`),
            dataUserData: t(`${guideKey}.dataUserData`),
            dataSeq: t(`${guideKey}.dataSeq`),
            dataFields: t(`${guideKey}.dataFields`),
            propertiesTitle: t(`${guideKey}.propertiesTitle`),
            propertiesIntro: t(`${guideKey}.propertiesIntro`),
            propertyId: t(`${guideKey}.propertyId`),
            propertyUserId: t(`${guideKey}.propertyUserId`),
            propertyLogin: t(`${guideKey}.propertyLogin`),
            propertyEmailPhone: t(`${guideKey}.propertyEmailPhone`),
            propertyRole: t(`${guideKey}.propertyRole`),
            propertyPermissions: t(`${guideKey}.propertyPermissions`),
            variablesTitle: t("fluid.variables"),
            variablesHelp: t(`${guideKey}.variablesHelp`),
            variables: renderRules(variables, "Fluid variables", false) || `<p>${escapeHtml(t(`${guideKey}.noRules`))}</p>`,
            functionsTitle: t("fluid.functions"),
            functionsHelp: t(`${guideKey}.functionsHelp`),
            functions: renderRules(functions, "Fluid functions", true) || `<p>${escapeHtml(t(`${guideKey}.noRules`))}</p>`,
            functionDetailsTitle: t(`${guideKey}.functionDetailsTitle`),
            randomNumberDetail: t(`${guideKey}.randomNumberDetail`),
            randomDetail: t(`${guideKey}.randomDetail`),
            randomNamed: t(`${guideKey}.randomNamed`),
            randomUniqueDetail: t(`${guideKey}.randomUniqueDetail`),
            incrementDetail: t(`${guideKey}.incrementDetail`),
            callNote: t(`${guideKey}.callNote`),
            workflowTitle: t(`${guideKey}.workflowTitle`),
            workflow1: t(`${guideKey}.workflow1`),
            workflow2: t(`${guideKey}.workflow2`),
            workflow3: t(`${guideKey}.workflow3`),
        };
        for (const [key, value] of Object.entries(replacements)) {
            html = html.replaceAll(`__${key.toUpperCase()}__`, key === "variables" || key === "functions" ? value : escapeHtml(value));
        }
        modal.showModal(<HtmlViewer title={t(`${guideKey}.title`)} html={html} />);
    };

    return <Button variant="outlined" onClick={openGuide} disabled={!rules}>
        {t("communication.template.strategies.guide.open")}
    </Button>;
};

export default TemplateRules;
