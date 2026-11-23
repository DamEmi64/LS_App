import { useEffect, useState } from "react";
import { Button } from "@mui/material";
import { useTranslation } from "react-i18next";
import { call } from "@/shared";
import HtmlViewer from "@/shared/components/HtmlViewer";
import { useModal } from "@/shared/context/modal";
import { useDictionaryTranslation } from "@/lib/utils";
import { CommunicationRules, CommunicationRule } from "../types";

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
        const translated = getDictionaryTranslation(dictionary, rule.id);
        const name = translated.title || rule.title;
        const syntax = isFunction ? `{{ ${rule.invoker}() }}` : `{{ ${rule.invoker} }}`;
        return `<article><h3>${escapeHtml(name)}</h3><code>${escapeHtml(syntax)}</code>${rule.description ? `<p>${escapeHtml(rule.description)}</p>` : ""}</article>`;
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
            variablesTitle: t("fluid.variables"),
            variablesHelp: t(`${guideKey}.variablesHelp`),
            variables: renderRules(variables, "Fluid variables", false) || `<p>${escapeHtml(t(`${guideKey}.noRules`))}</p>`,
            functionsTitle: t("fluid.functions"),
            functionsHelp: t(`${guideKey}.functionsHelp`),
            functions: renderRules(functions, "Fluid functions", true) || `<p>${escapeHtml(t(`${guideKey}.noRules`))}</p>`,
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
