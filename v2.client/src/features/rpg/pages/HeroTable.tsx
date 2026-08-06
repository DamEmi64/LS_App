import { useModal, call, Operations, ExpandableTable, ColumnType } from "@/shared";
import YesNoWindow from "@/shared/components/YesNoWindow";

import {
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
    const { checkPermission } = useAuth();

    // 📱 RESPONSIVE
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const operations: Operations<Hero>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => edit(o), hidden: (o) => !checkPermission(['rpg-write']) },
        { name: 'opt.delete', method: (o) => del(toDto(o)), hidden: (o) => !checkPermission(['rpg-write']) }
    ];

    const toDto = (data: Hero): HeroDto => {
        return {
            ...data,
            playerData: data.playerData || '',
            skills: data.skills || []
        } as unknown as HeroDto;
    };

    const details = (o: Hero) => {

        if (checkPermission(['rpg-write'])) {
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
       call(api => api.heroesApi.updateById,{id:data.id,heroDto:data})
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

                    call<Image>(api => api.homeApi.getMedia,{id:hero.imageId})
                        .then((res) => {
                            const imageData = res?.contentStr || '';

                            const newHero = { 
                                ...hero,
                                id: undefined,
                                chapter: chapterId,
                                image: imageData
                            };
                            call(api => api.heroesApi.create,{heroDto:newHero})
                                .then(() => modal.hideModal());
                        });
                }}
                onClose={() => modal.hideModal()}
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
        call(api => api.heroesApi.deleteById,{id:data.id})
            .then(() => {
                modal.hideModal();
                refresh();
            });
    };

    return (
        <ExpandableTable
            rows={heroes}
            getRowId={x => x.id}
            operations={operations}
            columns={[
                {
                    field: "firstName",
                    header: t('rpg.hero.firstName'),
                    type:ColumnType.String
                },
                {
                    field: "lastName",
                    header: t('rpg.hero.lastName'),
                    type:ColumnType.String
                },
                {
                    field: "player",
                    header: t('rpg.hero.player'),
                    type:ColumnType.String
                }
            ]}
        />
    );
};