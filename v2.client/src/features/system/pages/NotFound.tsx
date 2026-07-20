import { useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import ArrowBackRoundedIcon from "@mui/icons-material/ArrowBackRounded";
import HomeRoundedIcon from "@mui/icons-material/HomeRounded";
import SearchOffRoundedIcon from "@mui/icons-material/SearchOffRounded";
import { Box, Button, Chip, Paper, Stack, Typography } from "@mui/material";

const NotFound = () => {
    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        console.warn(
            "Route not found:",
            location.pathname,
        );
    }, [location.pathname]);

    return (
        <Box
            sx={{
                minHeight: "calc(100vh - 56px)",
                width: "100%",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                px: 2,
                py: 5,
            }}
        >
            <Paper
                elevation={8}
                sx={{
                    width: "min(100%, 560px)",
                    borderRadius: 2,
                    p: { xs: 3, sm: 4 },
                    textAlign: "center",
                    bgcolor: "rgba(18, 18, 18, 0.82)",
                    color: "common.white",
                    border: "1px solid rgba(255, 255, 255, 0.16)",
                    backdropFilter: "blur(12px)",
                }}
            >
                <Stack spacing={3} alignItems="center">
                    <Box
                        sx={{
                            width: 72,
                            height: 72,
                            borderRadius: "50%",
                            display: "grid",
                            placeItems: "center",
                            bgcolor: "rgba(255, 255, 255, 0.12)",
                            border: "1px solid rgba(255, 255, 255, 0.18)",
                        }}
                    >
                        <SearchOffRoundedIcon sx={{ fontSize: 38 }} />
                    </Box>

                    <Stack spacing={1} alignItems="center">
                        <Typography
                            variant="overline"
                            sx={{
                                fontWeight: 700,
                                letterSpacing: 0,
                                color: "rgba(255, 255, 255, 0.7)",
                            }}
                        >
                            404
                        </Typography>
                        <Typography variant="h4" component="h1" sx={{ fontWeight: 800 }}>
                            Page not found
                        </Typography>
                        <Typography
                            sx={{
                                maxWidth: 440,
                                color: "rgba(255, 255, 255, 0.72)",
                            }}
                        >
                            The page you tried to open does not exist or was moved.
                        </Typography>
                    </Stack>

                    <Chip
                        label={location.pathname}
                        variant="outlined"
                        sx={{
                            maxWidth: "100%",
                            color: "rgba(255, 255, 255, 0.82)",
                            borderColor: "rgba(255, 255, 255, 0.25)",
                            "& .MuiChip-label": {
                                display: "block",
                                maxWidth: "100%",
                                overflow: "hidden",
                                textOverflow: "ellipsis",
                            },
                        }}
                    />

                    <Stack
                        direction={{ xs: "column", sm: "row" }}
                        spacing={1.5}
                        sx={{ width: "100%", justifyContent: "center" }}
                    >
                        <Button
                            variant="contained"
                            startIcon={<HomeRoundedIcon />}
                            onClick={() => navigate("/")}
                            sx={{ minWidth: 150 }}
                        >
                            Home
                        </Button>
                        <Button
                            variant="outlined"
                            startIcon={<ArrowBackRoundedIcon />}
                            onClick={() => navigate(-1)}
                            sx={{
                                minWidth: 150,
                                color: "common.white",
                                borderColor: "rgba(255, 255, 255, 0.4)",
                                "&:hover": {
                                    borderColor: "rgba(255, 255, 255, 0.75)",
                                    bgcolor: "rgba(255, 255, 255, 0.08)",
                                },
                            }}
                        >
                            Go back
                        </Button>
                    </Stack>
                </Stack>
            </Paper>
        </Box>
    );
};

export default NotFound;
