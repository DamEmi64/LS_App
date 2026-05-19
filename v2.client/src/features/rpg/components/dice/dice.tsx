import "./styles.css";
import React, { useEffect, useRef, useState } from "react";
import DiceBoxLib from "@3d-dice/dice-box";
import { useTranslation } from "react-i18next";
import {
    Box,
    Button,
    Grid,
    Paper,
    TextField
} from "@mui/material";

/* ---------------- DICE INSTANCE ---------------- */

const Dice = new DiceBoxLib("#dice-box", {
    id: "dice-canvas",
    assetPath: "/assets/",
    startingHeight: 8,
    throwForce: 6,
    spinForce: 5,
    lightIntensity: 0.9
});

/* ---------------- COMPONENT ---------------- */

const DiceBox: React.FC<{ notation?: string }> = ({ notation }) => {
    const { t } = useTranslation();

    const [diceVal, setDiceVal] = useState("");
    const [diceNotation, setDiceNotation] = useState(
        notation || "1d20 + 1d6"
    );

    const rollingRef = useRef(false);
    const initializedRef = useRef(false);

    /* ---------------- INIT ---------------- */

    useEffect(() => {
        if (initializedRef.current) return;

        initializedRef.current = true;

        let mounted = true;

        const initDice = async () => {
            await Dice.init();

            if (!mounted) return;

            const handler = () => {
                // do not clear while rolling
                if (rollingRef.current) return;

                const diceCanvas =
                    document.getElementById("dice-canvas");

                if (
                    diceCanvas &&
                    window.getComputedStyle(diceCanvas).display !== "none"
                ) {
                    Dice.hide().clear();
                }
            };

            document.addEventListener("mousedown", handler);

            return () => {
                document.removeEventListener(
                    "mousedown",
                    handler
                );
            };
        };

        initDice();

        return () => {
            mounted = false;
        };
    }, []);

    /* ---------------- ROLL ---------------- */

    const rollDice = async (notationStr: string) => {
        // prevent overlapping rolls
        if (rollingRef.current) return;

        rollingRef.current = true;

        try {
            Dice.clear();

            const notation = notationStr
                .split("+")
                .map((n) => n.trim())
                .filter(Boolean);

            const result = await Dice.show().roll(notation);

            // validate values
            const invalid = result.some(
                (d: any) =>
                    d.value == null ||
                    d.value <= 0
            );

            if (invalid) {
                console.warn(
                    "Invalid dice result received:",
                    result
                );

                setDiceVal("Roll failed");
                return;
            }

            const val = result
                .map((d: any) => d.value)
                .join(" ");

            setDiceVal(val);
        } catch (ex) {
            console.error(ex);
            setDiceVal("Error");
        } finally {
            rollingRef.current = false;
        }
    };

    /* ---------------- UI ---------------- */

    return (
        <Paper sx={{ p: 2 }}>
            <Grid container spacing={2}>
                <Grid size={{ xs: 12, md: 6 }}>
                    <TextField
                        fullWidth
                        value={diceVal}
                        variant="outlined"
                        label="Result"
                        InputProps={{
                            readOnly: true
                        }}
                    />
                </Grid>

                <Grid
                    container
                    size={{ xs: 12, md: 6 }}
                    spacing={1}
                    alignItems="center"
                >
                    <Grid size={{ xs: 8 }}>
                        <TextField
                            fullWidth
                            value={diceNotation}
                            variant="outlined"
                            label="Dice notation"
                            onChange={(e) =>
                                setDiceNotation(
                                    e.target.value
                                )
                            }
                        />
                    </Grid>

                    <Grid size={{ xs: 4 }}>
                        <Button
                            fullWidth
                            variant="contained"
                            onClick={() =>
                                rollDice(
                                    diceNotation || "1d20"
                                )
                            }
                        >
                            {t("rpg.hero.diceRoll")}
                        </Button>
                    </Grid>
                </Grid>
            </Grid>
        </Paper>
    );
};

export default DiceBox;