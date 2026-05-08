import React, { useEffect, useRef, useState } from "react";
import { Box, Paper, Typography, Grid } from "@mui/material";
import * as signalR from "@microsoft/signalr";
import { battleNpc } from "@/features/rpg";
import { GridTable } from "@/shared/components/gridTable";
import { ColumnDef, ColumnType, TableData } from "@/shared";
import DiceBox from "../components/dice/dice";
import { useSignalR } from "@/shared/hooks/use-signalR";

const rows = [0, 1, 2, 3, 4, 5, 6, 7];
const columns = [0, 1, 2, 3, 4, 5, 6, 7];

const BattlePage: React.FC<{
  players: battleNpc[],
  readonly?: boolean,
  background?: string,
  onChange?: (data: battleNpc[]) => void
}>
  = ({ players = [], readonly = false, onChange, background }) => {
    const [npcs, setNpcs] = useState<battleNpc[]>(players);
    const [draggedItem, setDraggedItem] = useState<battleNpc | null>(null);
    const [npcTable, setNpcTable] = useState<TableData<battleNpc>>({ data: npcs, total: npcs.length } as TableData<battleNpc>)

    useEffect(() => {
      setNpcs(players);
      setNpcTable({ data: players, total: players.length });
    }, [players, background]);


    const tableColumns: ColumnDef[] = [
      { field: "title", header: "rpg.hero.firstName", type: ColumnType.String },
      { field: "health", header: "rpg.hero.health", type: ColumnType.Number },
      { field: "row", header: "rpg.hero.pos", type: ColumnType.Number },
      { field: "column", header: "rpg.hero.pos", type: ColumnType.Number }
    ];

    const handleDragStart = (item: battleNpc) => {
      setDraggedItem(item);
    };

    const handleDrop = (columnIndex: number, rowIndex: number) => {
      if (!draggedItem) return;

      const newData = npcs.map((npc) =>
        npc.id === draggedItem.id ? { ...npc, row: rowIndex, column: columnIndex } : npc
      );
      setNpcs(newData);
      setNpcTable({ data: newData, total: npcs.length });
      setDraggedItem(null);
      onChange?.(newData);
    };

    const onTableChange = (o : TableData<battleNpc>) => {
      setNpcs((prev) => o.data.map((npc, idx) => ({ ...npc, row: prev.at(idx)?.row || 0 })));
      onChange?.(o.data);
    }

    return (
      <Grid container spacing={2} width={'100%'} flexDirection={"column"} alignItems={'center'}>
        <Grid container spacing={2} width="100%" flexDirection="column" alignItems="center">
          <Grid container spacing={2} width="100%" flexDirection="column" alignItems="center">
            <Paper
              sx={{
                width: "90vw",
                height: "70vh",
                position: "relative",
                backgroundImage: `url(${background})`,
                backgroundSize: "cover",
                backgroundPosition: "center",
                borderRadius: 4,
                overflow: "hidden"
              }}
            >
              <Grid>
                {columns.map((columnIndex) => (
                  <Box key={columnIndex} display="flex" gap={2} padding={1}>
                    {rows.map((rowIndex) => {
                      const npcsInCell = npcs.filter(
                        (npc: any) => npc.row === rowIndex && npc.column === columnIndex
                      );

                      return (
                        <Box
                          key={rowIndex}
                          onDragOver={(e) => !readonly && e.preventDefault()}
                          onDrop={() => !readonly && handleDrop(columnIndex, rowIndex)}
                          sx={{
                            flex: 1,
                            minHeight: 80,
                            width: 80,
                            backgroundColor: "rgba(255,255,255,0.08)",
                            backdropFilter: "blur(2px)",
                            border: "1px solid rgba(255,255,255,0.15)",
                            borderRadius: 1,
                            display: "flex",
                            flexWrap: "wrap",
                            alignItems: "flex-start",
                            gap: 1,
                            transition: "0.2s",
                            "&:hover": {
                              backgroundColor: readonly
                                ? "rgba(255,255,255,0.08)"
                                : "rgba(255,255,255,0.2)"
                            }
                          }}
                        >
                          {npcsInCell.map((npc: any) => (
                            <Box
                              key={npc.id ?? npc.title}
                              draggable={!readonly}
                              onDragStart={() => !readonly && handleDragStart(npc)}
                              title={`HP: ${npc.health}`} // tooltip shows HP
                              style={{
                                width: 70,
                                height: 70,
                                borderRadius: "50%",
                                background: `linear-gradient(135deg, ${npc.color || "black"}, #555)`,
                                border: "2px solid white",
                                fontSize: 12,
                                textAlign: "center",
                                padding: 4
                              }}
                            >
                              {npc.title}
                            </Box>
                          ))}
                        </Box>
                      );
                    })}
                  </Box>
                ))}
              </Grid>
            </Paper>
          </Grid>
        </Grid>
        {!readonly && (
          <Grid size={{ xs: 6 }}>
            <GridTable
              columns={tableColumns}
              data={npcTable}
              setData={(o) => onTableChange(o)}
            />
          </Grid>
        )}
        {!readonly && (
          <Grid size={{ xs: 3 }}>
            <DiceBox notation="1d20 + 1d6" />
          </Grid>
        )}
      </Grid>
    );
  };

export default BattlePage;