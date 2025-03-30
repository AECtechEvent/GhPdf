using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Sd = System.Drawing;

using MD = MigraDoc.DocumentObjectModel;
using PD = PdfSharp.Drawing;
using PF = PdfSharp.Pdf;
using System.IO;

namespace GhPdf
{
    public class PdBlock
    {

        #region members

        public enum Type { Text, Image, Drawing, Table };
        protected Type type = Type.Text;

        protected string text = string.Empty;
        protected PdFont font = new PdFont();

        protected List<List<string>> values = new List<List<string>>();

        protected Sd.Bitmap image = new Sd.Bitmap(10, 10);

        protected PdDrawing drawing = new PdDrawing();

        #endregion

        #region constructors

        public PdBlock(PdBlock block)
        {
            this.type = block.type;

            this.text = block.text;
            this.font = new PdFont(block.font);

            this.image = new Sd.Bitmap(block.image);

            this.drawing = new PdDrawing(block.drawing);

            this.values = block.values.Duplicate();
        }

        public PdBlock(string text)
        {
            this.type = Type.Text;
            this.text = text;
        }

        public PdBlock(string text, PdFont font)
        {
            this.type = Type.Text;
            this.text = text;
            this.font = new PdFont(font);
        }

        public PdBlock(Sd.Bitmap bitmap)
        {
            this.type = Type.Image;
            this.image = new Sd.Bitmap(bitmap);
        }

        public PdBlock(PdDrawing drawing)
        {
            this.type = Type.Drawing;
            this.drawing = new PdDrawing(drawing);
        }

        public PdBlock(List<List<string>> values)
        {
            this.type = Type.Table;
            this.values = values.Duplicate();
        }

        public PdBlock(List<List<string>> values, PdFont font)
        {
            this.type = Type.Table;
            this.values = values.Duplicate();
            this.font = new PdFont(font);
        }

        #endregion

        #region properties



        #endregion

        #region methods

        public void Render(MD.Section section)
        {

            switch (this.type)
            {
                default:
                    this.RenderParagraph(section);
                    break;
                case Type.Image:
                    this.RenderImage(section);
                    break;
                case Type.Drawing:
                    this.RenderDrawing(section);
                    break;
                case Type.Table:
                    this.RenderTable(section);
                    break;
            }
        }

        private void RenderParagraph(MD.Section section)
        {
            MD.Paragraph paragraph = section.AddParagraph(this.text);
            paragraph.Format.Font.Name = this.font.Family;
            paragraph.Format.Font.Size = this.font.Size;
            paragraph.Format.Font.Color = this.font.Color.ToMigraDoc();

        }

        private void RenderImage(MD.Section section)
        {
            MD.Unit effectiveWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;

            MD.Shapes.Image img = section.AddImage(this.image.ToBase64String("base64:"));
            img.LockAspectRatio = true;
            img.Width = effectiveWidth;
        }

        private void RenderTable(MD.Section section)
        {
            MD.Unit effectiveWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;

            MD.Tables.Table table = section.AddTable();
            int colCount = this.values.Count;
            int rowCount = this.values[0].Count;

            table.Format.Font.Name = this.font.Family;
            table.Format.Font.Size = this.font.Size;
            table.Format.Font.Color = this.font.Color.ToMigraDoc();

            for (int i = 0; i < colCount; i++)
            {
                MD.Tables.Column col = table.AddColumn();
                col.Borders.Visible = true;
                col.Width = effectiveWidth / colCount;

                rowCount = Math.Max(rowCount, values[i].Count);
            }

            for (int i = 0; i < rowCount; i++)
            {
                MD.Tables.Row row = table.AddRow();
                row.Borders.Visible = true;
            }

            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    table.Rows[j].Cells[i].AddParagraph(values[i][j]);
                }
            }
        }

        private void RenderDrawing(MD.Section section)
        {
            MD.Unit effectiveWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;
            var memoryStream = new MemoryStream();

            PF.PdfDocument tempDoc = new PF.PdfDocument();
            PF.PdfPage page = tempDoc.AddPage();
            page.Width = this.drawing.Width;
            page.Height = this.drawing.Height;
            PD.XGraphics gfx = PD.XGraphics.FromPdfPage(page);

            drawing.RenderBlocks(gfx);

            tempDoc.Save(memoryStream, false);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var image = section.AddImage("base64:" + Convert.ToBase64String(memoryStream.ToArray()));
            image.LockAspectRatio = true;
            image.Width = effectiveWidth;
            memoryStream.Close();
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            switch (this.type)
            {
                default:
                    if (this.text.Length < 16) return "Wd | " + this.type + " {" + this.text + "}";
                    return "Pdf Blk | " + this.type + " {" + this.text.Substring(0, 15) + "...}";
                case Type.Image:
                    return "Pdf Blk | " + this.type + " {" + this.image.Width + "w " + this.image.Height + "h}";
                case Type.Drawing:
                    return "Pdf Blk | " + this.type + " {" + this.drawing.Count + "s}";
                case Type.Table:
                    return "Pdf Blk | " + this.type + " {" + this.values.Count + "c " + this.values[0].Count + "r}";
            }
        }

        #endregion

    }
}