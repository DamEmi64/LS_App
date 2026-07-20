import { useConfiguration } from "@/shared/context/configuration";
import { Box, Grid, TextField, Button, useTheme } from "@mui/material";
import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import ServerInfo, { ServerInfoProps } from "../serverInfo";
import { call } from "@/shared";
import RefreshIcon from '@mui/icons-material/Refresh';

const ServerSettings: React.FC = () => {
    const { useVariable } = useConfiguration();
    const [frontendVersion] = useVariable('version');
    const { t } = useTranslation();
    const [endpoint, setEndpoint] = useVariable('apiEndpoint');
    const theme = useTheme();
    const labelColor = theme.palette.text.primary;
    const [serverData, setServerData] = useState<ServerInfoProps>({ frontendVersion: 'unknown', version: 'unknown', modules: [] });

    // Always use updateData for initial load
    useEffect(() => {
        call<ServerInfoProps>(api => api.homeApi.get, {}).then(data => {
            data.frontendVersion = frontendVersion;
            setServerData(data);
        });
    }, []);

    const onEndpointChange = (data: string) => {
        setEndpoint(data);
        refreshConnection();
    }

    const refreshConnection = () => {
        call<ServerInfoProps>(api => api.homeApi.get, {}).then(data => {
            data.frontendVersion = frontendVersion;
            setServerData(data);
        });
    }

    return (
        <>
            <Box display="flex" flexDirection="column" gap={2}>
                <Grid display="flex" flexDirection="row" gap={1}>
                    <TextField
                        fullWidth
                        label={t('apiEndpoint')}
                        value={endpoint}
                        onChange={e => onEndpointChange(e.target.value)}
                        InputLabelProps={{ sx: { color: labelColor } }}
                    />
                    <Button onClick={() => refreshConnection()}
                        variant="outlined"
                        size="small">
                        <RefreshIcon />
                    </Button>
                </Grid>
                 <ServerInfo frontendVersion={frontendVersion} version={serverData.version} modules={serverData.modules || []} />
            </Box>
        </>
    );
};

export default ServerSettings;
