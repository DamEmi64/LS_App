import React, { useState } from "react";
import { Autocomplete, TextField, Chip, Avatar } from "@mui/material";

export default function RecipientInput({
    suggestions = [],
    value = [],
    onChange = (rs) => { },
    label = "Recipients",
    placeholder = "Add recipients (press Enter or comma)",
    maxRecipients = 50,
}) {
    const [emails, setEmails] = useState(value);

    const addEmails = (input) => {
        const newEmails = input
            .split(/[,;\n\r]+/)
            .map((e) => e.trim())
            .filter(Boolean);

        const combined = [...emails, ...newEmails];
        const unique = Array.from(new Set(combined.map((e) => e.toLowerCase()))).map(
            (e) => combined.find((orig) => orig.toLowerCase() === e)
        );

        const limited = unique.slice(0, maxRecipients);
        setEmails(limited);
        onChange(limited);
    };

    const handleKeyDown = (event) => {
        if ((event.key === "Enter" || event.key === ",") && event.target.value) {
            event.preventDefault();
            addEmails(event.target.value);
            event.target.value = "";
        }
    };

    const handlePaste = (event) => {
        const pasted = event.clipboardData.getData("text");
        if (!pasted) return;
        event.preventDefault();
        addEmails(pasted);
    };

    const handleChange = (event, newValue) => {
        const limited = newValue.slice(0, maxRecipients);
        setEmails(limited);
        onChange(limited);
    };

    return (
        <Autocomplete
            multiple
            freeSolo
            options={suggestions.map((s) =>
                s.name ? `${s.name} <${s.email}>` : s.email
            )}
            value={emails}
            onChange={handleChange}
            renderTags={(value, getTagProps) =>
                value.map((email, index) => (
                    <Chip
                        key={email}
                        label={email}
                        avatar={
                            <Avatar sx={{ width: 24, height: 24 }}>
                                {email[0]?.toUpperCase()}
                            </Avatar>
                        }
                        {...getTagProps({ index })}
                    />
                ))
            }
            renderInput={(params) => (
                <TextField
                    {...params}
                    label={label}
                    placeholder={placeholder}
                    onKeyDown={handleKeyDown}
                    onPaste={handlePaste}
                    helperText={`${emails.length}/${maxRecipients} max`}
                />
            )}
        />
    );
}
