import { useModal, useApiConnect, Operations } from "@/shared";
import OperationCell from "@/shared/components/operationCell";
import YesNoWindow from "@/shared/components/YesNoWindow";

import { format } from 'react-string-format';
import { useTranslation } from "react-i18next";

import PlaceForm from "../components/PlaceForm";
import { Chapter, Place, SessionDto, HeroDto } from "../types";
import SelectChapter from "@/features/rpg/components/SelectChapter";
import { Image } from "@/features/system";

import {
    Table,
    TableBody,
    TableCell,
    TableRow,
    Box,
    useMediaQuery,
    useTheme
} from "@mui/material";
import { useAuth } from "@/features/auth/context/authProvider";

export type PlaceTableProps = {
    places: Place[],
    chapters: Chapter[],
    refresh: () => void
};

export const PlaceTable: React.FC<PlaceTableProps> = ({
    places,
    chapters,
    refresh
}) => {
    const { t } = useTranslation();
    const modal = useModal();
    const api = useApiConnect();
    const { checkPermission } = useAuth();

    // 📱 RESPONSIVE
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const operations: Operations<Place>[] = [
        { name: 'opt.details', method: (o) => details(o) },
        { name: 'opt.edit', method: (o) => editPlace(o), hidden: (o) => !checkPermission(['rpg_write']) },
        { name: 'opt.delete', method: (o) => del(o), hidden: (o) => !checkPermission(['rpg_write']) }
    ];

    const details = (o: Place) => {
        const data = o as unknown as SessionDto;

        if (checkPermission(['rpg_write'])) {
            modal.showModal(
                <PlaceForm
                    data={data}
                    onSave={() => editPlace(o)}
                    onDelete={() => del(o)}
                    onCopy={(s) => copyPlace(s)}
                />
            );
        } else {
            modal.showModal(
                <PlaceForm
                    data={data}
                />
        );
        }
    };

    const editPlace = (o: Place) => {
        const data = {
            ...o,
            image: o.image
        } as unknown as SessionDto;

        modal.showModal(
            <PlaceForm
                data={data}
                onSave={(s) => savePlace(s, o)}
                onDelete={() => del(o)}
                isEdit
            />
        );
    };

    const savePlace = (data: SessionDto, place: Place) => {
        api.put('rpg_place_edit', data, null, place.id)
            .then(() => refresh());
    };

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

                            api.post('rpg_place_new', newPlace, null)
                                .then(() => modal.hideModal());
                        });
                }}
            />
        );
    };

    const del = (data: Place) => {
        modal.showModal(
            <YesNoWindow
                message={format(t('rpg.deleteConfirm.place'), data.title)}
                yesMethod={() => delConfirm(data)}
                open
                onClose={modal.hideModal}
                noMethod={modal.hideModal}
            />
        );
    };

    const delConfirm = (data: Place) => {
        api.del('rpg_place_del', null, data.id)
            .then(() => refresh());
    };

    return (
        <Box sx={{ width: "100%", overflowX: "auto" }}>
            <Table size={isMobile ? "small" : "medium"}>

                <TableBody>

                    {/* HEADER */}
                    <TableRow>
                        <TableCell sx={{ fontWeight: "bold" }}>
                            {t('rpg.other.title')}
                        </TableCell>

                        <TableCell sx={{ fontWeight: "bold" }}>
                            {t('rpg.other.description')}
                        </TableCell>

                        <TableCell />
                    </TableRow>

                    {/* DATA */}
                    {places?.map((place: Place) => (
                        <TableRow key={place.id ?? place.title}>

                            <TableCell sx={{ py: isMobile ? 1 : 1.5 }}>
                                {place.title}
                            </TableCell>

                            <TableCell sx={{ py: isMobile ? 1 : 1.5 }}>
                                {place.description
                                    ? place.description.length > 30
                                        ? place.description.slice(0, 30) + "..."
                                        : place.description
                                    : "--"}
                            </TableCell>

                            <TableCell sx={{ whiteSpace: "nowrap" }}>
                                <OperationCell operations={operations} data={place} />
                            </TableCell>

                        </TableRow>
                    ))}

                </TableBody>
            </Table>
        </Box>
    );
};