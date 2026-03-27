import React, { useEffect, useState } from 'react';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import {
    Box,
    TextField,
    Button,
    Snackbar,
    Alert
} from '@mui/material';

const LinkGen: React.FC<{ queryParams, endpoint: string }> = ({ queryParams = {}, endpoint }) => {
    const [link, setLink] = useState('');
    const [copied, setCopied] = useState(false);

    useEffect(() => {
        const baseUrl = endpoint;

        // Create a new URLSearchParams instance
        const generatedSearchParams = new URLSearchParams(queryParams);
        const queryString = generatedSearchParams.toString();

        const fullLink = queryString ? `${baseUrl}?${queryString}` : baseUrl;
        setLink(fullLink);
    }, [queryParams]);

    const handleCopy = async () => {
        try {
            await navigator.clipboard.writeText(link);
            setCopied(true);
        } catch (err) {
            console.error('Copy failed:', err);
        }
    };

    const handleCloseSnackbar = () => setCopied(false);

    return (
        <Box
            sx={{
                maxWidth: 600,
                margin: '2rem auto',
                padding: 3,
                border: '1px solid #ccc',
                borderRadius: 2,
                boxShadow: 2,
            }}
            flexDirection={'column'}
        >
            <TextField
                fullWidth
                label="Link"
                value={link}
                InputProps={{ readOnly: true }}
                sx={{ mb: 2 }}
            />

            <Button variant="contained" onClick={handleCopy} disabled={!link}>
                <ContentCopyIcon />
            </Button>

            <Snackbar
                open={copied}
                autoHideDuration={3000}
                onClose={handleCloseSnackbar}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
            >
                <Alert onClose={handleCloseSnackbar} severity="success" sx={{ width: '100%' }}>
                    Link copied to clipboard!
                </Alert>
            </Snackbar>
        </Box>
    );
};

export default LinkGen;