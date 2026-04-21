import React, { useRef } from 'react';
import { CKEditor } from 'ckeditor4-react';
import { useTranslation } from 'react-i18next';
import { useApiConnect } from '@/shared';
import { CommunicationRules } from '../types';

const Editor = ({ initData, onChange, readonly }) => {
    const { t, i18n } = useTranslation();
    const api = useApiConnect();

    // 👇 cache per editor instance
    const cacheRef = useRef(null);

    const loadSuggestions = async () => {
        if (cacheRef.current) return cacheRef.current;

        const res = await api.get<CommunicationRules>('communication_rules');
        const data = res.data;
        
        // translate once
        const transformed = {
            functions: data.functions.map(f => ({
                title: t(f.title),
                description: t(f.description),
                invoker: f.invoker
            })),
            variables: data.variables.map(v => ({
                title: t(v.title),
                description: t(v.description),
                invoker: v.invoker
            }))
        };

        cacheRef.current = transformed;
        return transformed;
    };

    return (
        <CKEditor
            key={i18n.language} // 👈 reset cache on language change
            initData={initData}
            readOnly={readonly}
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