import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { Avatar, CircularProgress, useTheme, Box } from '@mui/material';
import ReactImagePickerEditor, { ImagePickerConf } from 'react-image-picker-editor';
import noImage from '@/assets/no-image.png';
import { Image } from '@/features/system';
import { call } from './apiClient';

export interface ImageProviderProps {
    readonly?: boolean;
    saveImage?: (data: string) => void;
    imageId: string,
    image?: string
}

export const ImageProvider: React.FC<ImageProviderProps> = ({ readonly = false, saveImage, imageId }) => {
    const theme = useTheme();
    const [image, setImage] = useState<string>();
    const [pickerSource, setPickerSource] = useState<string>();
    const [loading, setLoading] = useState(Boolean(imageId));
    const [loadedImageId, setLoadedImageId] = useState(imageId);
    const isLoading = loading || Boolean(imageId && loadedImageId !== imageId);
    const currentImage = loadedImageId === imageId ? image : undefined;
    const currentPickerSource = loadedImageId === imageId ? pickerSource : undefined;
    const saveImageRef = useRef(saveImage);
    saveImageRef.current = saveImage;

    const config2 = useMemo<ImagePickerConf>(() => ({
        borderRadius: '8px',
        language: 'en',
        width: '100%',
        height: '250px',
        hideDeleteBtn: true,
        hideDownloadBtn: true,
        hideEditBtn: true,
        hideAddBtn: true,
        objectFit: 'fill',
        compressInitial: null,
        darkMode: theme.palette.mode === 'dark',
        rtl: false
    }), [theme.palette.mode]);

    const setData = useCallback((data: string) => {
        setImage(data);
        saveImageRef.current?.(data);
    }, []);

    useEffect(() => {
        let active = true;
        setImage(undefined);
        setPickerSource(undefined);

        if (!imageId) {
            setLoadedImageId(imageId);
            setLoading(false);
            return () => { active = false; };
        }

        setLoading(true);
        call<Image>(api => api.homeApi.getMedia, { id: imageId })
            .then(res => {
                if (!active) return;
                const content = res?.contentStr || undefined;
                setImage(content);
                setPickerSource(content);
                if (content) saveImageRef.current?.(content);
            })
            .catch(() => {
                if (active) {
                    setImage(undefined);
                    setPickerSource(undefined);
                }
            })
            .finally(() => {
                if (active) {
                    setLoadedImageId(imageId);
                    setLoading(false);
                }
            });

        return () => { active = false; };
    }, [imageId]);

    return (
        <Box sx={{ width: '100%', maxWidth: 250, minWidth: 0, overflow: 'hidden', boxSizing: 'border-box' }}>
            {isLoading ? (
                <Box sx={{ width: '100%', height: 250, display: 'grid', placeItems: 'center' }}>
                    <CircularProgress size={readonly ? 24 : undefined} />
                </Box>
            ) : readonly ? (
                <Avatar
                    src={currentImage || noImage}
                    variant="rounded"
                    sx={{ width: '100%', maxWidth: 250, height: 'auto', aspectRatio: '1 / 1' }}
                />
            ) : (
                <ReactImagePickerEditor
                    config={config2}
                    imageSrcProp={currentPickerSource || noImage}
                    imageChanged={setData} />
            )}
        </Box>
    );
};
