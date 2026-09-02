import { Accordion, AccordionDetails, AccordionSummary, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material"
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { CommunicationRules } from "../types";
import {call} from "@/shared";

export const TemplateRules = () => {
    const { t } = useTranslation();
    const [rules, setRules] = useState<CommunicationRules | null>(null);

    useEffect(() => {
        call<CommunicationRules>(api => api.templatesApi.getRules, {}).then(setRules);
    }, []);

    const getRuleTranslation = (key: string, field: 'title' | 'description') => {
        const normalizedKey = key.replace('communication.templates.', 'communication.template.');
        const fieldKey = normalizedKey.replace(/\.title$/, `.${field}`);

        return t(fieldKey);
    };

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
                    {rules?.functions.map((strategy, index) => (
                        <TableRow key={index}>
                            <TableCell>
                                <Typography variant="subtitle1">{getRuleTranslation(strategy.title, 'title')}</Typography>
                            </TableCell>
                            <TableCell>{getRuleTranslation(strategy.title, 'description')}</TableCell>
                        </TableRow>
                    ))}
                    {rules?.variables.map((strategy, index) => (
                        <TableRow key={index}>
                            <TableCell>
                                <Typography variant="subtitle1">{getRuleTranslation(strategy.title, 'title')}</Typography>
                            </TableCell>
                            <TableCell>{getRuleTranslation(strategy.title, 'description')}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </AccordionDetails>
    </Accordion>;
}

export default TemplateRules;
