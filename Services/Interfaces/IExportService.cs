﻿using Designer_Offer.Data;

namespace Designer_Offer.Services.Interfaces
{
    internal interface IExportService
    {
        bool ExportToExcel(Offers offer, bool summarySheet = true,
                                         bool internalUse = true);
    }
}
