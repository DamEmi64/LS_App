import React, { useRef } from 'react';
import { CKEditor } from 'ckeditor4-react';
import { useTranslation } from 'react-i18next';
import { CommunicationRules } from '../types';
import {call} from "@/shared";
import { useDictionaryTranslation } from '@/lib/utils';

const Editor = ({ initData, onChange, readonly }) => {
    const { t, i18n } = useTranslation();
    const getDictionaryTranslation = useDictionaryTranslation();

    // 👇 cache per editor instance
    const cacheRef = useRef(null);

    const loadSuggestions = async () => {
        const res = await call<CommunicationRules>(api =>api.templatesApi.getRules,{})
        const data = res;

        // 👇 capture values NOW (not during async execution later)
        return {
            functions: data.functions.map(f => {
                const tr = getDictionaryTranslation('Fluid functions', f.id);
                return {
                    title: tr.title,
                    description: tr.description,
                    invoker: f.invoker,
                    type: "function"
                };
            }),
            variables: data.variables.map(v => {
                return {
                    title: v.title,
                    description: v.description,
                    invoker: v.invoker,
                    type: "variable"
                };
            })
        };
    };


    return (
        <CKEditor
            key={i18n.language} // 👈 reset cache on language change
            initData={initData}
            readOnly={readonly}
            style={{width:'80vw',height:'90vh'}}
            config={{
                extraPlugins: 'fluidSuggestion',
                fluidSuggestion: {
                    loadSuggestions, // 👈 only once
                    labels: {
                        functions: t('fluid.functions'),
                        variables: t('fluid.variables'),
                        empty: t('fluid.empty')
                    }
                }
            }}
            onBeforeLoad={(CKEDITOR) => {
                CKEDITOR.plugins.addExternal(
                    'fluidSuggestion',
                    '/ckeditor/plugins/fluidSuggestion/',
                    'plugin.js'
                );
            }}
            onChange={(evt) => {
                onChange?.(evt.editor.getData());
            }}
        />
    );
};

export default Editor;