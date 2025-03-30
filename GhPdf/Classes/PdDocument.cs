using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PD = PdfSharp.Pdf;
using MD = MigraDoc.DocumentObjectModel;
using MR = MigraDoc.Rendering;

namespace GhPdf
{
    public class PdDocument
    {

        #region members

        public string Name = "Document 1";
        public double Width = 210;
        public double Height = 297;
        public Units Units = Units.Millimeter;

        public List<PdBlock> Blocks = new List<PdBlock>();

        #endregion

        #region constructors

        public PdDocument()
        {
        }

        public PdDocument(string name)
        {
            this.Name = name;
        }

        public PdDocument(string name, double width, double height, Units units)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.Units = units;
        }

        public PdDocument(PdDocument document)
        {
            this.Name = document.Name;
            this.Width = document.Width;
            this.Height = document.Height;
            this.Units = document.Units;

            foreach (PdBlock block in document.Blocks) this.Blocks.Add(new PdBlock(block));

        }

        #endregion

        #region properties



        #endregion

        #region methods

        public string Save(string directory)
        {
            string path = System.IO.Path.Combine(directory, Name + ".pdf");

            MD.Document document = new MD.Document();
            MD.Section section = document.AddSection();

            section.PageSetup.PageWidth = this.Width.ToUnit(this.Units);
            section.PageSetup.PageHeight = this.Height.ToUnit(this.Units);
            section.PageSetup.TopMargin = MD.Unit.FromMillimeter(25);
            section.PageSetup.BottomMargin = MD.Unit.FromMillimeter(25);
            section.PageSetup.LeftMargin = MD.Unit.FromMillimeter(20);
            section.PageSetup.RightMargin = MD.Unit.FromMillimeter(20);

            foreach (PdBlock block in Blocks)
            {
                block.Render(section);
            }

            MR.PdfDocumentRenderer pdfRenderer = new MR.PdfDocumentRenderer();
            pdfRenderer.Document = document;

            pdfRenderer.RenderDocument();
            
            pdfRenderer.PdfDocument.Save(path);

            return path;
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Pdf Doc | " + this.Name;
        }

        #endregion

    }
}
