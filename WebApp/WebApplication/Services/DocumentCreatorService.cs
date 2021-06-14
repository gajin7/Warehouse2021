using System;
using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Services
{
    public class DocumentCreatorService : IDocumentCreatorService
    {
        private readonly IItemRepository _itemRepository;
        public DocumentCreatorService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        public OperationResult CreateReceiptPdf(Receipt receipt,IEnumerable<ReceiptItem> receiptItems)
        {
            try
            {
                var pdf = new PdfDocument();
                var pdfPage = pdf.AddPage();
                var graph = XGraphics.FromPdfPage(pdfPage);
                var font = new XFont("Verdana", 14, XFontStyle.Regular);
                const int x = 20;
                var y = 100;
                var dy = (int)font.GetHeight() * 1; //change this factor to control line spacing


                graph.DrawString("Items", font, XBrushes.Black,
                    new XRect(x, y, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.Center);
                y += dy;
                graph.DrawString("Name      Quantity     Type     Amount", font, XBrushes.Black,
                    new XRect(x, y, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.Center);
                y += dy;
                foreach (var item in receiptItems)
                {
                    var itemValue = _itemRepository.GetItem(item.ItemId);
                    var value = itemValue?.Name + " " + item.Quantity + " " + itemValue?.Type + " " +
                              itemValue?.Amount * item.Quantity;

                    graph.DrawString(value, font, XBrushes.Black,
                        new XRect(x, y, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.Center);
                    y += dy;
                }

                graph.DrawString("Receipt: " + receipt.Id, new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
                    new XRect(25, 25, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                graph.DrawString("Company: " + receipt.Company, new XFont("Verdana", 16, XFontStyle.Regular), XBrushes.Black,
                    new XRect(25, 80, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                graph.DrawLine(new XPen(XColor.FromName("black")),new XPoint(),new XPoint());
                graph.DrawString(receipt.Date, new XFont("Verdana", 16, XFontStyle.Regular), XBrushes.Black,
                    new XRect(25, 150, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                graph.DrawString("Amount: " + receipt.Amount, new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
                    new XRect(-10, -10, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.BottomRight);

                var path = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\Receipts\Receipt" + receipt.Id + ".pdf";
                pdf.Save(path);


                return new OperationResult
                {
                    Success = true,
                    Message = path
                };
            }
            catch (Exception e)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Something went wrong, please try again",
                    ErrorMessage = e.Message
                };
            }

        }

        public OperationResult CreateReportPdf(Report report, IEnumerable<ItemReport> reportItems)
        {
            try
            {
                var pdf = new PdfDocument();
                var pdfPage = pdf.AddPage();
                var graph = XGraphics.FromPdfPage(pdfPage);
                var font = new XFont("Verdana", 14, XFontStyle.Regular);
                const int x = 20;
                var y = 100;
                var dy = (int)font.GetHeight() * 1; //change this factor to control line spacing


                graph.DrawString("Items", font, XBrushes.Black,
                    new XRect(x, y, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.Center);
                y += dy;
                graph.DrawString("Name      Quantity     Type     Amount", font, XBrushes.Black,
                    new XRect(x, y, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.Center);
                y += dy;
                foreach (var item in reportItems)
                {
                    var itemValue = _itemRepository.GetItem(item.ItemId);
                    var value = itemValue?.Name + " " + item.Quantity + " " + itemValue?.Type + " " +
                              itemValue?.Amount * item.Quantity;

                    graph.DrawString(value, font, XBrushes.Black,
                        new XRect(x, y, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.Center);
                    y += dy;
                }

                graph.DrawString("Report: " + report.Id, new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
                    new XRect(25, 25, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                graph.DrawString("Type: " + report.Type, new XFont("Verdana", 16, XFontStyle.Regular), XBrushes.Black,
                    new XRect(25, 80, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                graph.DrawLine(new XPen(XColor.FromName("black")), new XPoint(), new XPoint());
                graph.DrawString(report.Date, new XFont("Verdana", 16, XFontStyle.Regular), XBrushes.Black,
                    new XRect(25, 150, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                var path = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\Reports\Report" + report.Id + ".pdf";
                pdf.Save(path);


                return new OperationResult
                {
                    Success = true,
                    Message = path
                };
            }
            catch (Exception e)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Something went wrong, please try again",
                    ErrorMessage = e.Message
                };
            }

        }
    }
}