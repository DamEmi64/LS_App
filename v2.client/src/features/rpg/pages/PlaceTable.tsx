import { format } from "react-string-format";
import { useTranslation } from "react-i18next";

import { useModal, call, Operations } from "@/shared";

import YesNoWindow from "@/shared/components/YesNoWindow";

import PlaceForm from "../components/PlaceForm";

import SelectChapter from "@/features/rpg/components/SelectChapter";

import { useAuth } from "@/features/auth/context/authProvider";

import { ExpandableTable } from "@/shared";

import {
    Chapter,
    Place,
    SessionDto
} from "@/features/rpg";

import { Image } from "@/features/system";

export type PlaceTableProps = {
    places: Place[];
    chapters: Chapter[];
    refresh: () => void;
};

export const PlaceTable: React.FC<PlaceTableProps> = ({
    places,
    chapters,
    refresh
}) => {

    const { t } = useTranslation();

    const modal = useModal();

    const { checkPermission } = useAuth();

    const operations: Operations<Place>[] = [
        {
            name: "opt.details",
            method: (o) => details(o)
        },
        {
            name: "opt.edit",
            method: (o) => editPlace(o),
            hidden: () =>
                !checkPermission(["rpg-write"])
        },
        {
            name: "opt.copy",
            method: (o) =>
                copyPlace(o as unknown as SessionDto),
            hidden: () =>
                !checkPermission(["rpg-write"])
        },
        {
            name: "opt.delete",
            method: (o) => del(o),
            hidden: () =>
                !checkPermission(["rpg-write"])
        }
    ];

    const details = (o: Place) => {

        const data =
            o as unknown as SessionDto;

        if (checkPermission(["rpg-write"])) {

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
                onSave={(s) =>
                    savePlace(s, o)
                }
                onDelete={() => del(o)}
                isEdit
            />
        );
    };

    const savePlace = (
        data: SessionDto,
        place: Place
    ) => {

        call(
            api => api.placesApi.updateById,
            {
                id: place.id,
                placeDto: data
            }
        ).then(() => refresh());
    };

    const copyPlace = (
        place: SessionDto
    ) => {

        modal.showModal(
            <SelectChapter
                chapters={chapters}
                onSelect={(chapterId) => {

                    call<Image>(
                        api => api.homeApi.getMedia,
                        {
                            id: place.imageId
                        }
                    ).then((res) => {

                        const imageData =
                            res.contentStr || "";

                        const newPlace = {
                            ...place,
                            id: undefined,
                            chapter: chapterId,
                            image: imageData
                        };

                        call(
                            api => api.placesApi.create,
                            newPlace
                        ).then(() =>
                            modal.hideModal()
                        );
                    });
                }}
            />
        );
    };

    const del = (data: Place) => {

        modal.showModal(
            <YesNoWindow
                message={format(
                    t("rpg.deleteConfirm.place"),
                    data.title
                )}
                yesMethod={() =>
                    delConfirm(data)
                }
                open
                onClose={modal.hideModal}
                noMethod={modal.hideModal}
            />
        );
    };

    const delConfirm = (
        data: Place
    ) => {

        call(
            api => api.placesApi.deleteById,
            {
                id: data.id
            }
        ).then(() => refresh());
    };

    return (
        <ExpandableTable
            rows={places}
            getRowId={(x) =>
                x.id ?? x.title
            }
            operations={operations}
            columns={[
                {
                    field: "title",
                    header: t("rpg.other.title")
                },
                {
                    field: "description",
                    header: t("rpg.other.description"),

                    render: (place) =>
                        place.description
                            ? place.description.length > 30
                                ? place.description.slice(0, 30) + "..."
                                : place.description
                            : "--"
                }
            ]}
        />
    );
};