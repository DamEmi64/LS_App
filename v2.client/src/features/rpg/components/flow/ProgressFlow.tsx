import React, { useCallback, useRef, useState, useTransition } from "react";
import ReactFlow, {
    Handle,
    Position,
    MiniMap,
    Controls,
    Background,
    addEdge,
    useNodesState,
    useEdgesState,
    Connection,
    Edge,
    Node,
    ReactFlowInstance,
} from "reactflow";
import "reactflow/dist/style.css";

import {t} from 'i18next';
import { Box, Button } from "@mui/material";

import { RPGNodeData, RPGFlowProps } from "./definitions";
import { useModal } from "@/shared";
import { ProgressNodeView } from "./ProgressNodeView";
import { ProgressEdit } from "./ProgressEdit";
import { toPng } from "html-to-image";

const nodeTypes = { custom: ProgressNodeView };

// ---------------- MAIN ----------------
export const ProgressFlow = ({ readonly = false, initialNodes = [], initialEdges = [], onSave }: RPGFlowProps) => {

    const modal = useModal();

    const createStartNode = (): Node<RPGNodeData> => ({
        id: "start",
        type: "custom",
        position: { x: 0, y: 0 },
        data: { title: "Start", status: "default", kind: "event", editable: false },
    });

    const safeInitialNodes =
        initialNodes.length === 0
            ? [createStartNode()]
            : initialNodes.some((n) => n.id === "start")
                ? initialNodes
                : [createStartNode(), ...initialNodes];

    const [nodes, setNodes, onNodesChange] = useNodesState<RPGNodeData>(safeInitialNodes);
    const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);

    const wrapperRef = useRef<HTMLDivElement>(null);
    const rfInstance = useRef<ReactFlowInstance | null>(null);
    const connectingNodeId = useRef<string | null>(null);

    const onConnect = useCallback((params: Connection) => setEdges((eds) => addEdge(params, eds)), []);

    const onConnectStart = (_: any, { nodeId }: { nodeId: string }) => {
        connectingNodeId.current = nodeId;
    };

    const openModal = (node: Node<RPGNodeData>) => {
        modal.showSubModal(
            <ProgressEdit
                node={node}
                onChange={(data) => {
                    setNodes((nds) => nds.map((n) => (n.id === node.id ? { ...n, data: { ...data, editable: !readonly, onEdit: () => openModal(n) } } : n)));
                }}
            />
        );
    };

    const onConnectEnd = useCallback(
        (event: MouseEvent | TouchEvent) => {
            if (readonly || !rfInstance.current || !connectingNodeId.current) return;

            const sourceId = connectingNodeId.current;

            const target = event.target as HTMLElement;
            const isPane = target.classList.contains("react-flow__pane");
            if (!isPane) {
                connectingNodeId.current = null;
                return;
            }

            const bounds = wrapperRef.current?.getBoundingClientRect();
            if (!bounds) return;

            const clientX = "changedTouches" in event ? event.changedTouches[0].clientX : event.clientX;
            const clientY = "changedTouches" in event ? event.changedTouches[0].clientY : event.clientY;

            const position = rfInstance.current.project({
                x: clientX - bounds.left,
                y: clientY - bounds.top,
            });

            const id = `${Date.now()}`;

            const newNode: Node<RPGNodeData> = {
                id,
                type: "custom",
                position,
                data: { title: "New Node", status: "default", kind: "quest", editable: !readonly },
            };

            setNodes((nds) => nds.concat(newNode));

            setTimeout(() => {
                setEdges((eds) =>
                    addEdge(
                        {
                            id: `${sourceId}-${id}`,
                            source: sourceId,
                            target: id,
                        },
                        eds
                    )
                );
            }, 0);

            setTimeout(() => openModal(newNode), 0);

            connectingNodeId.current = null;
        },
        [readonly]
    );

    const addNodeInCenter = () => {
        if (readonly || !rfInstance.current || !wrapperRef.current) return;

        const bounds = wrapperRef.current.getBoundingClientRect();
        const position = rfInstance.current.project({ x: bounds.width / 2, y: bounds.height / 2 });

        const id = `${Date.now()}`;
        const newNode: Node<RPGNodeData> = {
            id,
            type: "custom",
            position,
            data: { title: "New Node", status: "default", kind: "quest", editable: !readonly },
        };

        setNodes((nds) => nds.concat(newNode));
        setTimeout(() => openModal(newNode), 0);
    };

    return (
        <Box sx={{width:'70vw',height:'70vh'}} ref={wrapperRef}>
            <Box sx={{ position: "absolute", zIndex: 10, p: 1 }}>
                {!readonly && (
                    <>
                        <Button variant="contained" onClick={() => onSave?.({ nodes, edges })}>
                            {t('opt.save')}
                        </Button>
                        <Button variant="contained" sx={{ ml: 1 }} onClick={addNodeInCenter}>
                            {t('rpg.flow.add_node')}
                        </Button>
                    </>
                )}
            </Box>

            <ReactFlow
                nodes={nodes.map((n) => ({
                    ...n,
                    draggable: n.id !== "start",
                    data: { ...n.data, editable: !readonly, onEdit: () => openModal(n) },
                }))}
                edges={edges}
                onNodesChange={onNodesChange}
                onEdgesChange={onEdgesChange}
                onConnect={onConnect}
                onConnectStart={onConnectStart}
                onConnectEnd={onConnectEnd}
                onInit={(i) => (rfInstance.current = i)}
                nodeTypes={nodeTypes}
                fitView
                panOnScroll
            >
                <MiniMap />
                <Controls />
                <Background />
            </ReactFlow>
        </Box>
    );
}
