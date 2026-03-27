import { Accordion, AccordionDetails, AccordionSummary, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material"
import { useState } from "react";
import { useTranslation } from "react-i18next";

export const TemplateRules = () => {
    const { t } = useTranslation();

    const strategies = [
        {
            title: t('communication.template.strategies.increment.title'),
            description: t('communication.template.strategies.increment.desc'),
            example: t('communication.template.strategies.increment.example')
        },
        {
            title: t('communication.template.strategies.random.title'),
            description: t('communication.template.strategies.random.desc'),
            example: t('communication.template.strategies.random.example')
        },
        {
            title: t('communication.template.strategies.randomUnique.title'),
            description: t('communication.template.strategies.randomUnique.desc'),
            example: t('communication.template.strategies.randomUnique.example')
        },
        {
            title: t('communication.template.strategies.randomNumber.title'),
            description: t('communication.template.strategies.randomNumber.desc'),
            example: t('communication.template.strategies.randomNumber.example')
        },
    ]

    return <Accordion>
        <AccordionSummary>
            <Typography variant="h6">{t('communication.template.strategies.title')}</Typography>
        </AccordionSummary>
        <AccordionDetails>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>{t('communication.template.strategies.title')}</TableCell>
                        <TableCell>{t('communication.template.strategies.description')}</TableCell>
                        <TableCell>{t('communication.template.strategies.example')}</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {strategies.map((strategy, index) => (
                        <TableRow key={index}>
                            <TableCell>
                                <Typography variant="subtitle1">{strategy.title}</Typography>
                            </TableCell>
                            <TableCell>{strategy.description}</TableCell>
                            <TableCell>{strategy.example}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </AccordionDetails>
    </Accordion>;
}

export default TemplateRules;