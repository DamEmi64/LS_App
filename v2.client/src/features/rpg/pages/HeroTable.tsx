import { useModal, useApiConnect, Operations } from "@/shared";
import OperationCell from "@/shared/components/operationCell";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { Table, TableBody, TableCell, TableRow } from "@mui/material";
import { t } from "i18next";
import { Chapter, Hero, HeroDto } from "../types";
import { HeroForm } from "../components/heroForm";
import { Image } from "@/features/system";
import SelectChapter from "@/features/rpg/components/selectChapter";


export type HeroTableProps = {
    heroes: Hero[],
    storyChapters: Chapter[],
    refresh: () => void
}

export const HeroTable: React.FC<HeroTableProps> = ({ heroes, storyChapters, refresh }) => {
    const modal = useModal();
    const api = useApiConnect();

    const operations: Operations<Hero>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => edit(o) },
        { name: 'opt.delete', method: (o) => del(o) }
    ]
    const toDto = (data: Hero): HeroDto => {
        return {...data, playerData: data.playerData || '', skills: data.skills || []} as unknown as HeroDto;
    }

    const details = (o: Hero) => { 
        modal.showModal(<HeroForm hero={toDto(o)} onSave={saveHero} onDelete={del} onCopy={copyHero}  />)
    }

    const edit = (o: Hero) => { 
        modal.showModal(<HeroForm hero={toDto(o)} onSave={saveHero} onDelete={del} onCopy={copyHero} isEdit={true} />)
    }

    const saveHero = (data: HeroDto) => {
        api.put<HeroDto>('rpg_hero_edit', data, null, data.id)
            .then(() => {
                modal.hideModal();
                refresh();
            });
    }

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

                    api.post<HeroDto>('rpg_hero_new', newHero, null)
                        .then(() => {
                            modal.hideModal();
                        });
                    });
                }}
            />
        );
    }

    const del = (data: any) => {
        modal.showModal(<YesNoWindow message={t('entity.del_info')} yesMethod={() => delConfirm(data)} open={true} onClose={modal.hideModal} noMethod={modal.hideModal} />);
    }

    const delConfirm = (data: any) => {
        api.del<Hero>('rpg_hero_details', null, data.id)
            .then(() => {
                modal.hideModal();
                refresh();
            });
    }

    return <>
        <Table>
            <TableRow>
                <TableCell
                    key='Hero_FirstName'
                >
                    {t('rpg.hero.firstName')}
                </TableCell>
                <TableCell
                    key='Hero_LastName'
                >
                    {t('rpg.hero.lastName')}
                </TableCell>
                <TableCell
                    key='Hero_Player'
                >
                    {t('rpg.hero.player')}
                </TableCell>
            </TableRow>
            <TableBody>
                {heroes && (heroes.map((hero: Hero) => (
                    <TableRow>
                        <TableCell>
                            {String(hero.firstName)}
                        </TableCell>
                        <TableCell>
                            {String(hero.lastName)}
                        </TableCell>
                        <TableCell>
                            {String(hero.player)}
                        </TableCell>
                        <OperationCell operations={operations} data={hero} />
                    </TableRow>
                )))}
            </TableBody>
        </Table>
    </>;
}