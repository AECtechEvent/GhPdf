using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg = Rhino.Geometry;
using MD = MigraDoc.DocumentObjectModel;
using PD = PdfSharp.Drawing;
using PF = PdfSharp.Pdf;
using System.IO;

namespace GhPdf
{
    public class PdDrawing
    {

        #region members

        protected List<PdShape> shapes = new List<PdShape>();

        #endregion

        #region constructors

        public PdDrawing()
        {

        }

        public PdDrawing(PdDrawing drawing)
        {
            foreach (PdShape shape in drawing.shapes) this.shapes.Add(new PdShape(shape));
        }

        public PdDrawing(List<PdShape> shapes)
        {
            foreach (PdShape shape in shapes) this.shapes.Add(new PdShape(shape));
        }

        #endregion

        #region properties

        public int Count
        {
            get { return shapes.Count; }
        }

        public virtual Rg.BoundingBox BoundingBox
        {
            get {
                Rg.BoundingBox output = Rg.BoundingBox.Unset;
                foreach(PdShape shape in shapes)
                {
                    output.Union(shape.BoundingBox);
                }
                return output;
            }
        }

        public virtual double Width
        {
            get { return this.BoundingBox.Diagonal.X; }
        }

        public virtual double Height
        {
            get { return this.BoundingBox.Diagonal.Y; }
        }

        #endregion

        #region methods

        public void RenderBlocks(PD.XGraphics graphics)
        {
            foreach (PdShape shape in this.shapes)
            {
                shape.Render(graphics);
            }
        }
        
        #endregion

        #region overrides



        #endregion

    }
}
