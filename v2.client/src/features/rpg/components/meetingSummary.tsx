import { Typography } from "@mui/material";

type MeetingSummaryProps = {
    summary: string;
};

const MeetingSummary = ({ summary }: MeetingSummaryProps) => {

    return (
        <Typography variant="body1" sx={{ whiteSpace: "pre-line", color: "text.primary" }}>
            {summary}
        </Typography>
    );
};

export default MeetingSummary;
