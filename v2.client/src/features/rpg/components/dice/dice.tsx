import "./styles.css";
import { useState } from "react";
import { Dice } from "./dicebox";
import { useTranslation } from "react-i18next";
import React from "react";
import { Box, Button, FormLabel, Grid, Paper, TextField } from "@mui/material";

// initialize the Dice Box outside of the component
Dice.init().then(() => {
    // clear dice on click anywhere on the screen
    document.addEventListener("mousedown", () => {
        const diceBoxCanvas = document.getElementById("dice-canvas");
        if (window.getComputedStyle(diceBoxCanvas).display !== "none") {
            Dice.hide().clear();
        }
    });
});

const DiceBox: React.FC<{notation?: string }> = ({notation}) =>  {
    const { t } = useTranslation();
    const [diceVal, setDiceVal] = useState('');
    const [diceNotatation, setDiceNotation] = useState(notation ||'1d20 + 1d6');

    // trigger dice roll
    const rollDice = (notationStr: string) => {
        Dice.clear();
        const notation = notationStr.split('+');

        Dice.show().roll(notation).then(result => {
            let val = '';

            result.forEach(element => {
                val = val + ' ' + element.value;
            });

            setDiceVal(val); 
        }).catch((ex) => alert(ex.message));
    };

    return (
        <Paper>
            <Grid container>
                <Grid size={{ xs: 6 }}>
                    <TextField value={diceVal} type="outlined" />
                </Grid>
                <Grid container size={{ xs: 6 }} direction={'row'}>
                    <TextField value={diceNotatation} onChange={(e) => setDiceNotation(e.target.value)} />
                    <Button
                        onClick={(e) => rollDice(diceNotatation || '1d20')}
                    >
                        {t('rpg.hero.diceRoll')}
                    </Button>
                </Grid>
            </Grid>

        </Paper>
    )
}

export default DiceBox;