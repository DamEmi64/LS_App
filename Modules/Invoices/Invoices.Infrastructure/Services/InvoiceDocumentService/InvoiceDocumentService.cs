using Invoices.Domain.Entities;
using Microsoft.Playwright;
using Razor.Templating.Core;

namespace Invoices.Infrastructure.Services.InvoiceDocumentService
{
    public sealed class InvoiceDocumentService : IInvoiceDocumentService, IAsyncDisposable
    {
        private const string TemplatePath = "/Views/InvoiceTemplate.cshtml";
        private readonly SemaphoreSlim _browserLock = new(1, 1);
        private IPlaywright? _playwright;
        private IBrowser? _browser;

        public Task<string> GenerateHtml(Invoice invoice)
        {
            ArgumentNullException.ThrowIfNull(invoice);
            return RazorTemplateEngine.RenderAsync(TemplatePath, invoice);
        }

        public async Task<byte[]> GeneratePdf(Invoice invoice)
        {
            var html = await GenerateHtml(invoice);
            var browser = await GetBrowser();
            var page = await browser.NewPageAsync();
            try
            {
                await page.SetContentAsync(html);
                return await page.PdfAsync(new PagePdfOptions
                {
                    Format = "A4",
                    PrintBackground = true,
                    PreferCSSPageSize = true
                });
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_browser is not null)
                await _browser.CloseAsync();
            _playwright?.Dispose();
            _browserLock.Dispose();
        }

        private async Task<IBrowser> GetBrowser()
        {
            if (_browser is { IsConnected: true })
                return _browser;

            await _browserLock.WaitAsync();
            try
            {
                if (_browser is { IsConnected: true })
                    return _browser;

                _playwright ??= await Playwright.CreateAsync();
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true
                });
                return _browser;
            }
            finally
            {
                _browserLock.Release();
            }
        }
    }
}
