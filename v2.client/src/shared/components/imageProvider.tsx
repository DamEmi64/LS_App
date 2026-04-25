import React, { useEffect, useState } from 'react';
import { Avatar, IconButton, CircularProgress, useTheme, Box } from '@mui/material';
import ReactImagePickerEditor, { ImagePickerConf } from 'react-image-picker-editor';
import { useApiConnect } from '@/shared/context/apiConnect';
import noImage from '@/assets/no-image.png';
import { Image } from '@/features/system';

export interface ImageProviderProps {
    readonly?: boolean;
    saveImage?: (data: string) => void;
    imageId: string,
    image?: string
}

export const ImageProvider: React.FC<ImageProviderProps> = ({ readonly = false, saveImage, imageId }) => {
    const {homeApi, call} = useApiConnect();
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
        call<Image>(homeApi,homeApi.getHomeImage,{id:imageId})
            .then((res) => {
                if (res != null && res.contentStr !== '') {
                    setData(res.contentStr);
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