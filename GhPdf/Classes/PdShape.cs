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
    public class PdShape
    {

        #region members

        protected Rg.NurbsCurve curve = null;
        protected PdGraphic graphic = new PdGraphic();

        #endregion

        #region constructors

        public PdShape()
        {

        }

        public PdShape(PdShape shape)
        {
            this.curve = shape.curve.DuplicateCurve().ToNurbsCurve();
            this.graphic = new PdGraphic(shape.graphic);
        }

        public PdShape(Rg.Curve curve, PdGraphic graphic)
        {
            this.curve = curve.DuplicateCurve().ToNurbsCurve();
            this.graphic = new PdGraphic(graphic);
        }

        #endregion

        #region properties

        public virtual Rg.BoundingBox BoundingBox
        {
            get { return this.curve.GetBoundingBox(false); }
        }

        #endregion

        #region methods

        public void Render(PD.XGraphics graphics)
        {
            PD.XGraphicsPath path = new PD.XGraphicsPath();
            path.StartFigure();
            if (this.curve.Degree == 1)
            {
                path.AddPolygon(curve.ToBezierXPoints());
            }
            else
            {
                path.AddBeziers(curve.ToBezierXPoints());
            }
            if (curve.IsClosed) path.CloseFigure();

            if (this.graphic.Fill.A != 0)
            {
                graphics.DrawPath(this.graphic.ToPen(), this.graphic.ToBrush(), path);
            }
            else
            {
                graphics.DrawPath(this.graphic.ToPen(), path);
            }
        }

        #endregion

        #region overrides



        #endregion

    }
}
