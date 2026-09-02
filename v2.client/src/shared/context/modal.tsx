import React, { createContext, useContext, useState, ReactNode } from 'react';
import { Modal, Box, Button, useMediaQuery, useTheme } from '@mui/material';
import { GridCloseIcon } from '@mui/x-data-grid';

type ModalContextType = {
    showModal: (content: ReactNode) => void;
    hideModal: () => void;
    showSubModal: (content: ReactNode) => void;
    hideSubModal: () => void;
    closeTopMostModal: () => boolean;
};

const ModalContext = createContext<ModalContextType | undefined>(undefined);

export const useModal = () => {
    const context = useContext(ModalContext);
    if (!context) {
        throw new Error('useModal must be used within a ModalProvider');
    }
    return context;
};

export const ModalProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [open, setOpen] = useState(false);
    const [content, setContent] = useState<ReactNode>(null);
    const [openSub, setOpenSub] = useState(false);
    const [contentSub, setContentSub] = useState<ReactNode>(null);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const showModal = (modalContent: ReactNode) => {
        setContent(modalContent);
        setOpen(true);
    };

    const hideModal = () => {
        setOpen(false);
        setContent(null);
    };

    const showSubModal = (modalContent: ReactNode) => {
        setContentSub(modalContent);
        setOpenSub(true);
    };

    const hideSubModal = () => {
        setOpenSub(false);
        setContentSub(null);
    };

    const closeTopMostModal = () => {
        if (openSub) {
            hideSubModal();
            return true;
        }

        if (open) {
            hideModal();
            return true;
        }

        return false;
    };

    return (
        <ModalContext.Provider value={{ showModal, hideModal, showSubModal, hideSubModal, closeTopMostModal }}>
            {children}
            <Modal open={open} onClose={hideModal}>
                <>
                    <Box
                        sx={{
                            position: { xs: 'fixed', sm: 'absolute' },
                            top: { xs: 0, sm: '50%' },
                            left: { xs: 0, sm: '50%' },
                            transform: { xs: 'none', sm: 'translate(-50%, -50%)' },
                            bgcolor: 'background.paper',
                            boxShadow: 24,
                            boxSizing: 'border-box',
                            width: { xs: '100vw', sm: 'calc(100vw - 32px)' },
                            height: { xs: '100dvh', sm: 'auto' },
                            maxWidth: { xs: 'none', sm: 720 },
                            p: { xs: 2, sm: 4 },
                            borderRadius: { xs: 0, sm: 2 },
                            maxHeight: { xs: '100dvh', sm: '90vh' },
                            overflow: 'auto',
                        }}
                        style={{ backdropFilter: "blur(4px)" }}
                    >
                        {content}
                    </Box>
                    <Modal open={openSub} onClose={hideSubModal}>
                        <Box
                            sx={{
                                position: 'absolute',
                                top: '50%',
                                left: '50%',
                                transform: 'translate(-50%, -50%)',
                                bgcolor: 'background.paper',
                                boxShadow: 24,
                                p: 4,
                                borderRadius: 2,
                                minWidth: 300,
                                maxWidth: '90vw',
                                maxHeight: '90vh',
                                overflow: 'auto',
                            }}
                            style={{ backdropFilter: "blur(4px)" }}
                        >
                            {contentSub}
                        </Box>
                    </Modal>
                </>
            </Modal>
        </ModalContext.Provider>
    );
};
