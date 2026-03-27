import React, { useRef, useState } from "react";
import { Box, Paper, Typography, Grid } from "@mui/material";
import index from '@/assets/files.jpg';

// ===== Types from your app =====
import { battleNpc } from "@/features/rpg";
import { GridTable } from "@/shared/components/gridTable";
import { ColumnDef, ColumnType, TableData } from "@/shared";
import DiceBox from "../components/dice/dice";

const rows = [0, 1, 2, 3, 4, 5, 6, 7];
const columns = [0, 1, 2, 3, 4, 5, 6, 7];

const BattlePage = () => {
  const [npcs, setNpcs] = useState<battleNpc[]>([]);
  const [draggedItem, setDraggedItem] = useState<battleNpc | null>(null);
  const [npcTable, setNpcTable] = useState<TableData<battleNpc>>({ data: npcs, total: npcs.length } as TableData<battleNpc>)

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

    var newData = npcs.map((npc) =>
      npc.id === draggedItem.id ? { ...npc, row: rowIndex, column: columnIndex } : npc
    );
    setNpcs(newData);
    setNpcTable({ data: newData, total: npcs.length });
    setDraggedItem(null);
  };

  // Ensure every npc has a row (default 0)
  const npcsWithRows = npcs.map((n) => ({ ...n, row: (n as any).row ?? 0, column: (n as any).column ?? 0 }));

  return (
    <Grid container spacing={2} width={'100%'} flexDirection={"column"} alignItems={'center'}>
      <Grid container spacing={2} width="100%" flexDirection="column" alignItems="center">
        <Grid container spacing={2} width="100%" flexDirection="column" alignItems="center">
          <Paper sx={{backgroundColor:'transparent', backgroundSize:'cover', width:'90vw',height:'70vh'}}>
          <Grid>
            {columns.map((columnIndex) => (
              <Box key={columnIndex} display="flex" gap={2} padding={1}>
                {rows.map((rowIndex) => {
                  const npcsInCell = npcsWithRows.filter(
                    (npc: any) => npc.row === rowIndex && npc.column === columnIndex
                  );

                  return (
                    <Box
                      key={rowIndex}
                      onDragOver={(e) => e.preventDefault()}
                      onDrop={() => handleDrop(columnIndex, rowIndex)}
                      sx={{
                        flex: 1,
                        minHeight: 80,
                        width: 80,
                        backgroundColor: "#f5f5f5",
                        opacity: 0.8,
                        padding: 1,
                        borderRadius: 2,
                        display: "flex",
                        flexWrap: "wrap", // allow multiple small boxes to wrap
                        alignItems: "flex-start",
                        gap: 1,
                        overflowY: "auto",
                      }}
                    >
                      {npcsInCell.map((npc: any) => (
                        <Box
                          key={npc.id ?? npc.title}
                          draggable
                          onDragStart={() => handleDragStart(npc)}
                          title={`HP: ${npc.health}`} // tooltip shows HP
                          sx={{
                            width: 80, // small square
                            height: 80,
                            backgroundColor: "black",
                            color: "white",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            borderRadius: 1,
                            cursor: "grab",
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
      <Grid size={{ xs: 6 }}>
        <GridTable
          columns={tableColumns}
          data={npcTable}
          setData={(o) => setNpcs((prev) => o.data.map((npc, idx) => ({ ...npc, row: prev.at(idx)?.row || 0 })))}
        />
      </Grid>
      <Grid size={{ xs: 3 }}>
        <DiceBox notation="1d20 + 1d6" />
      </Grid>
    </Grid>
  );
};

export default BattlePage;