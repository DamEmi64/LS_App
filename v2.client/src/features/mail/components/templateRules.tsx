import { getDictionary, useDictionaryTranslation } from "@/lib/utils";
import { Accordion, AccordionDetails, AccordionSummary, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material"
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { CommunicationRules } from "../types";
import { useApiConnect } from "@/shared";

export const TemplateRules = () => {
    const { t } = useTranslation();
    const getDictionaryTranslation = useDictionaryTranslation();
    const [rules, setRules] = useState<CommunicationRules | null>(null);
    const {templatesApi,call} = useApiConnect();

    call<CommunicationRules>(templatesApi,templatesApi.getTemplateRule,{}).then(res => {
        const data = res;
        setRules(data);   
        });

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
                                <Typography variant="subtitle1">{getDictionaryTranslation('Fluid functions',strategy.title).title}</Typography>
                            </TableCell>
                            <TableCell>{getDictionaryTranslation('Fluid functions',strategy.title).description}</TableCell>
                        </TableRow>
                    ))}
                    {rules?.variables.map((strategy, index) => (
                        <TableRow key={index}>
                            <TableCell>
                                <Typography variant="subtitle1">{getDictionaryTranslation('Fluid functions',strategy.title).title}</Typography>
                            </TableCell>
                            <TableCell>{getDictionaryTranslation('Fluid functions',strategy.title).description}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </AccordionDetails>
    </Accordion>;
}

export default TemplateRules;