import React from "react";
import {
    Card,
    CardContent,
    Typography,
    Grid,
    useTheme,
    Box,
} from "@mui/material";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { useTranslation } from "react-i18next";

export type ServerInfoProps = {
    version: string;
    frontendVersion: string;
    modules: { name: string; version: string }[];
};

const ServerInfo: React.FC<ServerInfoProps> = ({
    version,
    frontendVersion,
    modules,
}) => {
    const { t } = useTranslation();
    const theme = useTheme();
    const textColor =
        theme.palette.mode === "dark"
            ? theme.palette.text.primary
            : theme.palette.text.secondary;

    const moduleRows = modules.map((module, index) => ({
        id: index,
        name: module.name,
        version: module.version // Assuming you want to display a version, replace with actual data if available
    }));

    const columns: GridColDef[] = [
        {
            field: "name",
            headerName: t('server.module_name'),
            flex: 1,
        },
        {
            field: "version",
            headerName: t('server.module_version'),
            flex: 2,
        },
    ];

    return (
        <Card sx={{ maxWidth: 800, margin: "auto", mt: 5, p: 2 }}>
            <CardContent>
                <Typography variant="h5" gutterBottom style={{ color: textColor }}>
                    {t('server.info')}
                </Typography>
                <Grid container spacing={2} sx={{ mb: 2 }}>
                    <Grid>
                        <Typography variant="subtitle2">{t('server.server_version')}</Typography>
                        <Typography>{version}</Typography>
                    </Grid>
                    <Grid>
                        <Typography variant="subtitle2">{t('server.frontend_version')}</Typography>
                        <Typography>{frontendVersion}</Typography>
                    </Grid>
                </Grid>
                <Typography variant="subtitle2" sx={{ mb: 1 }}>
                    {t('server.modules')}
                </Typography>
                <Box style={{ height: 300 }}>
                    <DataGrid
                        rows={moduleRows}
                        columns={columns}
                        disableRowSelectionOnClick
                    />
                </Box>
            </CardContent>
        </Card>
    );
};

export default ServerInfo;