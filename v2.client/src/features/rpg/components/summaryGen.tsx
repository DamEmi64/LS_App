import React, { useState } from 'react';
import {
    Box,
    Typography,
    Checkbox,
    FormControlLabel,
    Button,
    Paper,
    Grid,
    RadioGroup,
    Radio,
    useTheme,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import { Story } from '../types';


type SummaryGenProps = {
    story: Story,
    forFirebase?: boolean,
    onProcess: (data: Story, isPdf: boolean) => void
}

const SummaryGen: React.FC<SummaryGenProps> = ({ story, onProcess, forFirebase }: SummaryGenProps) => {
    const theme = useTheme();
    const textColor = theme.palette.mode === 'dark'
        ? theme.palette.text.primary
        : theme.palette.text.secondary;
    const chapters = story.chapters || [];
    const { t } = useTranslation();
    const [selected, setSelected] = useState<string[]>([]);
    const [isPdf, setIsPdf] = useState(true);

    const handleToggle = (id: string) => {
        if (selected.includes(id)) {
            setSelected(selected.filter((s) => s !== id));
        } else {
            setSelected([...selected, id]);
        }
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        const processed = chapters.filter((ch) => selected.includes(ch.id));
        onProcess({
            ...story,
            chapters: processed
        }, isPdf);
    };

    return (
        <Grid style={{ color: textColor }}>
            <Typography variant="h6" gutterBottom>
                {t('rpg.story.gen_summary_desc')}
            </Typography>
            <Box component="form" onSubmit={handleSubmit} display={'flex'} flexDirection="column" gap={2}>
                {chapters.map((chapter) => (
                    <FormControlLabel
                        key={chapter.id}

                        control={
                            <Checkbox
                                checked={selected.includes(chapter.id)}
                                onChange={() => handleToggle(chapter.id)}
                            />
                        }
                        label={chapter.title}
                    />
                ))}
                {!forFirebase && (
                    <RadioGroup row defaultValue={'pdf'} onChange={o => setIsPdf(o.target.value === 'pdf')}>
                        <FormControlLabel value="pdf" control={<Radio />} label='pdf' />
                        <FormControlLabel value="html" control={<Radio />} label='html' />
                    </RadioGroup>
                )}
                <Box mt={2}>
                    <Button type="submit" variant="contained" color="primary">
                        {t('opt.save')}
                    </Button>
                </Box>
            </Box>
        </Grid>
    );
};

export default SummaryGen;