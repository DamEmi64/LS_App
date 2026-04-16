import { onChangeParams } from "@/shared";
import { useEffect, useState } from "react";
import { useApiConnect } from "@/shared/context/apiConnect";
import { SessionTable } from "./SessionTable";
import { Story } from "@/features/rpg";

const RPG: React.FC<{ draft: boolean }> = ({ draft }) => {
    const [data, setData] = useState<any[]>([{ id: '1', title: 'test', chapters: [{ id: '12', title: "test chapter" }], places: [{ id: '123', title: "test places" }], heroes: [{ id: '22', firstName: "test", lastName: "hero" }] }]);
    const [rowCount, setRowCount] = useState<number>(0);
    const [loading, setLoading] = useState<boolean>(true);
    const api = useApiConnect();

    const updateData = async (paramsObj: onChangeParams) => {
        const { page, pageSize, orderBy, order, filters } = paramsObj;
        const params = new URLSearchParams({
            page: page?.toString() || '1',
            pageSize: pageSize?.toString() || '10',
            orderBy: orderBy || '',
            order: order || '',
        });

        (filters || []).forEach(filter => {
            params.append(filter.field, filter.value.toLocaleString());
        });

        var result = await api.get<Story[]>(draft ? 'rpg_stories_drafts' : 'rpg_stories_data', { params });
        setData(result.data);
        setRowCount(result.total);
        setLoading(false);
    }

    // Always use updateData for initial load
    useEffect(() => {
        updateData({ page: 0, pageSize: 10, orderBy: '', order: 'asc', filters: [] });
    }, []);

    return (
        <SessionTable data={data} updateData={updateData} rowCount={rowCount} setRowCount={setRowCount} draft={draft} />
    );
};

export default RPG;