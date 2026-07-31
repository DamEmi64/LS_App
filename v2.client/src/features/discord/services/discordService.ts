import { call } from "@/shared";
import { ResponseList } from "@/shared/api/extension";
import { DiscordCmd } from "@/shared/api/generated";

export interface DiscordCommandRow {
  id: string;
  command: string;
  response: string;
  active: boolean;
}

export async function loadDiscordCommands() {
  const data = await call<ResponseList<DiscordCmd>>(api => api.discordClient.get, {});

  return (data.data || []).map(item => ({
    id: item.id || "",
    command: item.cmd || "",
    response: item.response || "",
    active: item.active || false,
  }));
}

export async function saveDiscordCommand(row: DiscordCommandRow, updates: Partial<DiscordCommandRow>) {
  const payload: DiscordCmd = {
    id: row.id,
    cmd: row.command,
    response: updates.response ?? row.response,
    active: updates.active ?? row.active,
  };

  await call(api => api.discordClient.updateById, { id: row.id, discordCmd: payload });
}
