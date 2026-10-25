import { useCallback } from 'react';
import { useTranslation } from 'react-i18next';
import { notify } from '@/shared/components/NotificationListener';
import { EVENT_MESSAGES, useEvent } from '@/shared/context/EventContext';
import { getInvoicesToPay } from '../services/invoiceService';
import { INVOICE_STATUS } from '../types';

const UnpaidInvoiceLoginListener = () => {
    const { t } = useTranslation();
    const checkInvoices = useCallback(() => {
        void getInvoicesToPay()
            .then(invoices => {
                const hasUnpaidInvoices = invoices.some(invoice =>
                    invoice.positions.some(position => position.status !== INVOICE_STATUS.paid)
                );

                if (hasUnpaidInvoices) {
                    notify('warning', t('invoices.unpaidAfterLogin'));
                }
            })
            .catch(() => {
                // Invoice availability should not interfere with logging in.
            });
    }, [t]);

    useEvent(EVENT_MESSAGES.UserLogIn, checkInvoices);

    return null;
};

export default UnpaidInvoiceLoginListener;
