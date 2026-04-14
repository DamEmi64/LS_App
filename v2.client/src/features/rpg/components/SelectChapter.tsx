import {
    Box,
    Button,
    CircularProgress,
    List,
    ListItemButton,
    ListItemText,
    Typography,
} from '@mui/material';
import { useState } from 'react';
import { Chapter } from '../types';
import { useTranslation } from 'react-i18next';

type SelectChapterProps = {
    onSelect: (chapterId: string) => void;
    onClose?: () => void;
    chapters: Chapter[];
};

const SelectChapter: React.FC<SelectChapterProps> = ({ onSelect, onClose, chapters }) => {
    const { t } = useTranslation();
    const [selected, setSelected] = useState<string | null>(null);

    const handleConfirm = () => {
        if (selected) {
            onSelect(selected);
        }
    };

    return (
        <Box sx={{ p: 3, minWidth: 400 }}>
            <Typography variant="h6" sx={{ mb: 2 }}>
                {t('rpg.chapter.select')}
            </Typography>
            <List>
                {chapters.map((chapter) => (
                    <ListItemButton
                        key={chapter.id}
                        selected={selected === chapter.id}
                        onClick={() => setSelected(chapter.id)}
                    >
                        <ListItemText
                            primary={chapter.title}
                            secondary={chapter.description}
                        />
                    </ListItemButton>
                ))}
            </List>

            <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2, mt: 3 }}>
                <Button variant="outlined" onClick={onClose}>
                    {t('opt.cancel')}
                </Button>

                <Button
                    variant="contained"
                    color="primary"
                    disabled={!selected}
                    onClick={handleConfirm}
                >
                    {t('opt.select')}
                </Button>
            </Box>
        </Box>
    );
};

export default SelectChapter;