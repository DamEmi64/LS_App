import { useModal, useApiConnect, Operations } from "@/shared";
import OperationCell from "@/shared/components/operationCell";
import YesNoWindow from "@/shared/components/YesNoWindow";

import {
    Table,
    TableBody,
    TableCell,
    TableRow,
    Box,
    useMediaQuery,
    useTheme
} from "@mui/material";

import { t } from "i18next";
import { Chapter, Hero, HeroDto } from "../types";
import { HeroForm } from "../components/heroForm";
import { Image } from "@/features/system";
import SelectChapter from "@/features/rpg/components/SelectChapter";
import { useAuth } from "@/features/auth/context/authProvider";

export type HeroTableProps = {
    heroes: Hero[],
    storyChapters: Chapter[],
    refresh: () => void
};

export const HeroTable: React.FC<HeroTableProps> = ({
    heroes,
    storyChapters,
    refresh
}) => {
    const modal = useModal();
    const api = useApiConnect();
    const { checkPermission } = useAuth();

    // 📱 RESPONSIVE
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const operations: Operations<Hero>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => edit(o), hidden: (o) => !checkPermission(['rpg_write']) },
        { name: 'opt.delete', method: (o) => del(toDto(o)), hidden: (o) => !checkPermission(['rpg_write']) }
    ];

    const toDto = (data: Hero): HeroDto => {
        return {
            ...data,
            playerData: data.playerData || '',
            skills: data.skills || []
        } as unknown as HeroDto;
    };

    const details = (o: Hero) => {

        if (checkPermission(['rpg_write'])) {
            modal.showModal(
                <HeroForm
                    hero={toDto(o)}
                    onSave={saveHero}
                    onDelete={del}
                    onCopy={copyHero}
                />
            );
         }
        else {
            modal.showModal(
                <HeroForm
                    hero={toDto(o)}
                />
            );
        }
    };

    const edit = (o: Hero) => {
        modal.showModal(
            <HeroForm
                hero={toDto(o)}
                onSave={saveHero}
                onDelete={del}
                onCopy={copyHero}
                isEdit
            />
        );
    };

    const saveHero = (data: HeroDto) => {
        api.put('rpg_hero_edit', data, null, data.id)
            .then(() => {
                modal.hideModal();
                refresh();
            });
    };

    const copyHero = (hero: HeroDto) => {
        modal.showModal(
            <SelectChapter
                chapters={storyChapters}
                onSelect={(chapterId) => {

                    const params = new URLSearchParams({
                        id: hero.imageId || ''
                    });

                    api.get<Image>('image', { params })
                        .then((res) => {
                            const imageData = res.data?.contentStr || '';

                            const newHero = {
                                ...hero,
                                id: undefined,
                                chapter: chapterId,
                                image: imageData
                            };

                            api.post('rpg_hero_new', newHero, null)
                                .then(() => modal.hideModal());
                        });
                }}
            />
        );
    };

    const del = (data: HeroDto) => {
        modal.showModal(
            <YesNoWindow
                message={t('entity.del_info')}
                yesMethod={() => delConfirm(data)}
                open
                onClose={modal.hideModal}
                noMethod={modal.hideModal}
            />
        );
    };

    const delConfirm = (data: HeroDto) => {
        api.del('rpg_hero_details', null, data.id)
            .then(() => {
                modal.hideModal();
                refresh();
            });
    };

    return (
        <Box sx={{ width: "100%", overflowX: "auto" }}>
            <Table size={isMobile ? "small" : "medium"}>

                <TableBody>
                    {/* HEADER ROW */}
                    <TableRow sx={{ fontWeight: "bold" }}>
                        <TableCell>{t('rpg.hero.firstName')}</TableCell>
                        <TableCell>{t('rpg.hero.lastName')}</TableCell>
                        <TableCell>{t('rpg.hero.player')}</TableCell>
                        <TableCell />
                    </TableRow>

                    {/* DATA */}
                    {heroes?.map((hero: Hero) => (
                        <TableRow key={hero.id ?? `${hero.firstName}-${hero.lastName}`}>
                            <TableCell sx={{ py: isMobile ? 1 : 1.5 }}>
                                {hero.firstName}
                            </TableCell>

                            <TableCell sx={{ py: isMobile ? 1 : 1.5 }}>
                                {hero.lastName}
                            </TableCell>

                            <TableCell sx={{ py: isMobile ? 1 : 1.5 }}>
                                {hero.player}
                            </TableCell>

                            <TableCell sx={{ whiteSpace: "nowrap" }}>
                                <OperationCell operations={operations} data={hero} />
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>

            </Table>
        </Box>
    );
};