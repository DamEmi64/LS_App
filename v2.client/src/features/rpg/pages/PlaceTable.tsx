import { useModal, useApiConnect, Operations } from "@/shared";
import OperationCell from "@/shared/components/operationCell";
import YesNoWindow from "@/shared/components/YesNoWindow";
import { format } from 'react-string-format';
import { useTranslation } from "react-i18next";
import PlaceForm from "../components/PlaceForm";
import { Chapter, HeroDto, Place, SessionDto } from "../types";
import { Table, TableBody, TableCell, TableRow } from "@mui/material";
import SelectChapter from "../components/selectChapter";
import {Image} from "@/features/system";

export type PlaceTableProps = {
    places: Place[],
    chapters: Chapter[],
    refresh: () => void
}

export const PlaceTable: React.FC<PlaceTableProps> = ({ places, chapters, refresh }) => {
    const { t } = useTranslation();
    const modal = useModal();
    const api = useApiConnect();

    const operations: Operations<Place>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => editPlace(o) },
        { name: 'opt.delete', method: (o) => del(o) }
    ]

    const details = (o: Place) => {
        var data = o as unknown as SessionDto;
        modal.showModal(<PlaceForm data={data} onSave={(s) => editPlace(o)} onDelete={(s) => del(o)} onCopy={(s) => copyPlace(s)}/>)
    }

        const copyPlace = (place: SessionDto) => {
        modal.showModal(
            <SelectChapter
                chapters={chapters}
                onSelect={(chapterId) => {

                const params = new URLSearchParams({
                            id: place.imageId || ''
                        });

                    api.get<Image>('image', { params })
                    .then((res) => {
                        const imageData = res.data?.contentStr || '';

                    const newPlace = {
                        ...place,
                        id: undefined,
                        chapter: chapterId,
                        image: imageData
                    };

                    api.post<HeroDto>('rpg_place_new', newPlace, null)
                        .then(() => {
                            modal.hideModal();
                        });
                    });
                }}
            />
        );
    }

    const editPlace = (o: Place) => {
        var data = o as unknown as SessionDto;
        data.image = o.image;
        modal.showModal(<PlaceForm data={data} onSave={(s) => savePlace(s, o)} onDelete={(s) => del(o)} isEdit={true} />)
    }
    
    const savePlace = (data: SessionDto, place: Place) => {
        api.put<SessionDto>('rpg_place_edit', data, null, place.id)
            .then(() => {
                refresh();
            });
    }

    const del = (data: Place) => {
        modal.showModal(<YesNoWindow message={format(t('rpg.deleteConfirm.place'), data.title)} yesMethod={() => delConfirm(data)} open={true} onClose={modal.hideModal} noMethod={modal.hideModal} />);
    }

    const delConfirm = (data: any) => {
        api.del<Place>('rpg_place_del', null, data.id)
            .then(() => {
                refresh();
            });
    }

    return <>
        <Table>
            <TableRow>
                <TableCell
                >
                    {t('rpg.other.title')}
                </TableCell>
                <TableCell
                >
                    {t('rpg.other.description')}
                </TableCell>
            </TableRow>
            <TableBody>
                {places && (places.map((place: Place) => (
                    <TableRow>
                        <TableCell>
                            {String(place.title)}
                        </TableCell>
                        <TableCell>
                            {String(place.description.length > 20 ? place.description.slice(0, 20) + '...' : place.description)}
                        </TableCell>
                        <OperationCell operations={operations} data={place} />
                    </TableRow>
                )))}
            </TableBody>
        </Table>
    </>;
}