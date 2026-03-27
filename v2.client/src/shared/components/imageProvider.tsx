import React, { useEffect, useState } from 'react';
import { Avatar, IconButton, CircularProgress, useTheme, Box } from '@mui/material';
import ReactImagePickerEditor, { ImagePickerConf } from 'react-image-picker-editor';
import { useApiConnect } from '@/shared/context/apiConnect';
import noImage from '@/assets/no-image.png';
import { Image } from '@/models/System';

export interface ImageProviderProps {
    readonly?: boolean;
    saveImage?: (data: string) => void;
    imageId: string,
    image?: string
}

export const ImageProvider: React.FC<ImageProviderProps> = ({ readonly = false, saveImage, imageId }) => {
    const api = useApiConnect();
    const theme = useTheme();
    const [image, setImage] = useState<string>();

    const config2: ImagePickerConf = {
        borderRadius: '8px',
        language: 'en',
        width: '250px',
        height: '250px',
        hideDeleteBtn: true,
        hideDownloadBtn: true,
        hideEditBtn: true,
        hideAddBtn: true,
        objectFit: 'fill',
        compressInitial: null,
        darkMode: theme.palette.mode !== 'dark',
        rtl: false
    };

    useEffect(() => {
        const params = new URLSearchParams({
            id: imageId
        });

        api.get<Image>('image', { params })
            .then((res) => {
                if (res.data != null && res.data.contentStr !== '') {
                    setData(res.data.contentStr);
                }
            });
    }, [imageId]);

    const setData = (data: string) => {
        setImage(data);
        if (saveImage) saveImage(data);
    }
    if (readonly) {
        return (
            <Avatar
                src={image || noImage}
                variant="rounded"
                sx={{ width: 'fit-content', height: 'fit-content' }}
            />

        );
    }
    else {
        return (
            <ReactImagePickerEditor
                config={config2}
                imageSrcProp={image || noImage}
                imageChanged={(newDataUri: any) => { setData(newDataUri); }} />
        )
    }
};