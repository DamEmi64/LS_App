import React from 'react';
import { CKEditor } from 'ckeditor4-react';

interface EditorProps {
    initData?: string;
    readonly?: boolean;
    onChange?: (data: string) => void;
}

export const Editor: React.FC<EditorProps> = ({ initData, onChange, readonly }) => {
    return <CKEditor initData={initData}
        readOnly={readonly}
        onChange={(evt) => {
            const data = evt.editor.getData();
            if (onChange) onChange(data);
        }} />;
}

export default Editor;