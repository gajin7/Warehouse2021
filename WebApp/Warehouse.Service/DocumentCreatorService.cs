using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Data;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;
using Warehouse.Service.Abstractions;
using Warehouse.Web.Model.Response;

namespace Warehouse.Service
{
    public class DocumentCreatorService : IDocumentCreatorService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IPricelistRepository _pricelistRepository;
        public DocumentCreatorService(IItemRepository itemRepository, IPricelistRepository pricelistRepository)
        {
            _itemRepository = itemRepository;
            _pricelistRepository = pricelistRepository;
        }
        public OperationResult CreateReceiptPdf(Receipt receipt,IEnumerable<ReceiptItem> receiptItems)
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\Receipts\Receipt" + receipt.Id + ".pdf";
                PdfDocument doc = new PdfDocument();

                //Add a page.

                PdfPage page = doc.Pages.Add();

                //Create a PdfGrid.

                PdfGrid pdfGrid = new PdfGrid
                {
                    Style = new PdfGridStyle()
                    {
                        Font = new PdfStandardFont(PdfFontFamily.Helvetica,12),
                    }
                };

                PdfGraphics graphics = page.Graphics;
                graphics.DrawString("Receipt", new PdfStandardFont(PdfFontFamily.Helvetica, 36),
                    PdfBrushes.Black, PointF.Empty);
                graphics.DrawString("ID: " + receipt.Id, new PdfStandardFont(PdfFontFamily.Helvetica, 16),
                    PdfBrushes.Black, new PointF(0, 50));
                graphics.DrawString("Company: " + receipt.Company, new PdfStandardFont(PdfFontFamily.Helvetica, 20),
                    PdfBrushes.Black, new PointF(0, 100));
                graphics.DrawString("Date: " + receipt.Date, new PdfStandardFont(PdfFontFamily.Helvetica, 20),
                    PdfBrushes.Black, new PointF(0, 150));
                graphics.DrawString("Amount: " + receipt.Amount + " $",
                    new PdfStandardFont(PdfFontFamily.Helvetica, 36),
                    PdfBrushes.Black, new PointF(250, 200));

                graphics.DrawString("Items ", new PdfStandardFont(PdfFontFamily.Helvetica, 20),
                    PdfBrushes.Black, new PointF(0, 250));

                //Create a DataTable.

                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("Quantity");
                dataTable.Columns.Add("Type");
                dataTable.Columns.Add("Amount per item");
                dataTable.Columns.Add("Total amount");

                //Add rows to the DataTable.

                foreach (var item in receiptItems)
                {

                    var itemValue = _itemRepository.GetItem(item.ItemId);
                    var amount = _pricelistRepository.GetPriceForItem(itemValue.Id);
                    dataTable.Rows.Add(new object[]
                    {
                        itemValue?.Name, item.Quantity, itemValue?.Type, amount + "$",
                        amount * item.Quantity  + "$"
                    });
                }


                //Assign data source.

                pdfGrid.DataSource = dataTable;

                //Draw grid to the page of PDF document.

                pdfGrid.Draw(page, new PointF(0, 290));

                //Save the document.

                doc.Save(path);

                //close the document

                doc.Close(true);

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
                var path = AppDomain.CurrentDomain.BaseDirectory + @"\Documents\Reports\Report" + report.Id + ".pdf";

                PdfDocument doc = new PdfDocument();

                //Add a page.

                PdfPage page = doc.Pages.Add();

                //Create a PdfGrid.

                PdfGrid pdfGrid = new PdfGrid
                {
                    Style = new PdfGridStyle()
                    {
                        Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12),
                    }
                };

                PdfGraphics graphics = page.Graphics;
                graphics.DrawString("Report", new PdfStandardFont(PdfFontFamily.Helvetica, 36),
                    PdfBrushes.Black, PointF.Empty);
                graphics.DrawString("ID: " + report.Id, new PdfStandardFont(PdfFontFamily.Helvetica, 16),
                    PdfBrushes.Black, new PointF(0, 50));
                graphics.DrawString("Type: " + report.Type, new PdfStandardFont(PdfFontFamily.Helvetica, 20),
                    PdfBrushes.Black, new PointF(0, 100));
                graphics.DrawString("Date: " + report.Date, new PdfStandardFont(PdfFontFamily.Helvetica, 20),
                    PdfBrushes.Black, new PointF(0, 150));

                graphics.DrawString("Items ", new PdfStandardFont(PdfFontFamily.Helvetica, 20),
                    PdfBrushes.Black, new PointF(0, 250));

                //Create a DataTable.

                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("Quantity");
                dataTable.Columns.Add("Type");
                dataTable.Columns.Add("Amount per item");
                dataTable.Columns.Add("Total amount");

                //Add rows to the DataTable.

                foreach (var item in reportItems)
                {

                    var itemValue = _itemRepository.GetItem(item.ItemId);
                    var amount = _pricelistRepository.GetPriceForItem(itemValue.Id, DateTime.Parse(report.Date));
                    dataTable.Rows.Add(new object[]
                    {
                        itemValue?.Name, item.Quantity, itemValue?.Type, amount + "$",
                        amount * item.Quantity  + "$" 
                    });
                }


                //Assign data source.

                pdfGrid.DataSource = dataTable;

                //Draw grid to the page of PDF document.

                pdfGrid.Draw(page, new PointF(0, 290));

                //Save the document.

                doc.Save(path);

                //close the document

                doc.Close(true);


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