import { useState } from "react";
import { Box, Button, TextField, InputLabel, MenuItem } from "@mui/material";
import { useTranslation } from 'react-i18next';

type Props = {
    onSubmit: (data: {
        file?: File;
        converterType: number;
        externalUrl?: string;
    }) => void;
};

// enum-like mapping (clear + maintainable)
const CONVERTER_TYPES = [
    { value: 502, label: "Old JSON" },
    { value: 501, label: "JSON" },
    { value: 503, label: "Firebase" }
];

const ImportStory: React.FC<Props> = ({ onSubmit }) => {
    const [file, setFile] = useState<File | null>(null);
    const [converterType, setConverterType] = useState(0);
    const [externalUrl, setExternalUrl] = useState("");

    const {t} = useTranslation();

    const handleSubmit = () => {
        onSubmit({
            file: converterType !== 503 ? file ?? undefined : undefined,
            converterType,
            externalUrl: converterType === 503 ? externalUrl : undefined
        });
    };

    return (
        <Box display="flex" flexDirection="column" gap={2}>
            
            {/* Converter type */}
            <TextField
                select
                label={t("rpg.import.converterType")}
                value={converterType}
                onChange={(e) => {
                    const val = Number(e.target.value);
                    setConverterType(val);

                    // reset fields on change
                    setFile(null);
                    setExternalUrl("");
                }}
                fullWidth
            >
                {CONVERTER_TYPES.map((opt) => (
                    <MenuItem key={opt.value} value={opt.value}>
                        {opt.label}
                    </MenuItem>
                ))}
            </TextField>

            {/* FILE INPUT (non-firebase only) */}
            {!(converterType === 503) && (
                <>
                    <InputLabel htmlFor="file-upload" required>
                        Select file
                    </InputLabel>

                    <Button variant="outlined" component="label">
                        {t("rpg.import.selectFile")}
                        <input
                            id="file-upload"
                            type="file"
                            hidden
                            accept=".json"
                            onChange={(e) => {
                                const selected = e.target.files?.[0] || null;
                                setFile(selected);
                            }}
                        />
                    </Button>

                    {file && <span>{file.name}</span>}
                </>
            )}

            {converterType === 503 && (
                <TextField
                    label={t("rpg.import.externalUrl")}
                    value={externalUrl}
                    onChange={(e) => setExternalUrl(e.target.value)}
                    required
                    fullWidth
                />
            )}

            <Button
                variant="contained"
                onClick={handleSubmit}
            >
                {t("opt.import")}
            </Button>
        </Box>
    );
};

export default ImportStory;