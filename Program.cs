using Syncfusion.Licensing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;
using System;
using System.Diagnostics;
using System.IO;
using Syncfusion.Drawing;

namespace MeasureTextWithPdfStringFormat
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			// Create a PDF Document.
			var doc = new PdfDocument();

			doc.PageSettings.Margins.All = 0;
			PdfPage page = doc.Pages.Add();

			PdfGraphics graphics = page.Graphics;
			PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Regular);

			var state = graphics.Save();

			var text = "Hello World!";
			var textSize = font.MeasureString(text);

			var clientSize = graphics.ClientSize;

			// define coordinates
			PdfUnitConverter converter = new PdfUnitConverter();
			var widthA4 = converter.ConvertUnits(21, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
			var heightA4 = converter.ConvertUnits(29.7f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
			var point10 = converter.ConvertUnits(10, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

			// draw coordinate lines
			graphics.DrawLine(PdfPens.LightGray, 0, point10, widthA4, point10);
			graphics.DrawLine(PdfPens.LightGray, point10, 0, point10, heightA4);

			// draw margin infos
			var margins = doc.PageSettings.Margins;
			graphics.DrawString($"{margins.Left},{margins.Right},{margins.Top},{margins.Bottom}", font, PdfBrushes.Black, new PointF(100, 100));

			// draw text height
			graphics.DrawString($"text height: {textSize.Height}", font, PdfBrushes.Black, new PointF(100, 120));
			graphics.DrawString($"text width: {textSize.Width}", font, PdfBrushes.Black, new PointF(100, 140));

			// draw client size
			graphics.DrawString($"clientSize height: {clientSize.Height}", font, PdfBrushes.Black, new PointF(100, 160));
			graphics.DrawString($"clientSize width: {clientSize.Width}", font, PdfBrushes.Black, new PointF(100, 180));



			//Restore
			graphics.Restore();


			// Rotate
			graphics.TranslateTransform(point10, point10);
			graphics.RotateTransform(-45);


			//activate: text and bound box without PdfStringFormat
			//graphics.DrawRectangle(PdfPens.Black, new RectangleF(point10, point10, textSize.Width, textSize.Height));
			//graphics.DrawString(text, font, PdfBrushes.Black, PointF.Empty);
			//graphics.DrawRectangle(PdfPens.Black, new RectangleF(point10, point10, textSize.Width, textSize.Height));

			// text and bound box with PdfStringFormat
			// rotate on Bottom left corner
			var pdfStringFormat = new PdfStringFormat();
			pdfStringFormat.LineAlignment = PdfVerticalAlignment.Bottom;
			pdfStringFormat.EnableBaseline = true;
			graphics.DrawString(text, font, PdfBrushes.Red, PointF.Empty, pdfStringFormat);

			// measure text with used PdfStringFormat
			var textSize2 = font.MeasureString(text, pdfStringFormat);

			graphics.DrawRectangle(PdfPens.Red, new RectangleF(0, 0, textSize2.Width, textSize2.Height));

			// save pdf and open
			var stream = File.Create($"{Guid.NewGuid()}.pdf");
			doc.Save(stream);
			var pdfPath = stream.Name;
			doc.Close(true);
			stream.Close();
			Process.Start(stream.Name);
		}
	}
}
