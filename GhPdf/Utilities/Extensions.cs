using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Rg = Rhino.Geometry;

using PD = PdfSharp.Drawing;
using PF = PdfSharp.Pdf;
using MD = MigraDoc.DocumentObjectModel;
using System.IO;

namespace GhPdf
{
    public static class Extensions
    {

        public static List<List<string>> Duplicate(this List<List<string>> input)
        {
            List<List<string>> output = new List<List<string>>();
            foreach (List<string> vals in input)
            {
                List<string> outputA = new List<string>();
                foreach (string val in vals)
                {
                    outputA.Add(val);
                }
                output.Add(outputA);
            }
            return output;
        }

        public static MD.Color ToMigraDoc(this Sd.Color input)
        {
            return new MD.Color(input.A, input.R, input.G, input.B);
        }

        public static MD.Unit ToUnit(this double input, Units units)
        {
            switch (units)
            {
                default:
                    return MD.Unit.FromMillimeter(input);
                case Units.Centimeter:
                    return MD.Unit.FromCentimeter(input);
                case Units.Inch:
                    return MD.Unit.FromInch(input);
                case Units.Point:
                    return MD.Unit.FromPoint(input);
            }
        }

        public static string ToBase64String(this Sd.Bitmap input, string prefix = "")
        {
            MemoryStream stream = new MemoryStream();

            input.Save(stream, Sd.Imaging.ImageFormat.Png);
            stream.Position = 0;
            byte[] buffer = stream.ToArray();
            stream.Close();

            string output = Convert.ToBase64String(buffer);
            buffer = null;

            return prefix + output;
        }

        #region pdf sharp

        #region -graphics
        public static PD.XColor ToXColor(this Sd.Color input)
        {
            return PD.XColor.FromArgb(input.A, input.R, input.G, input.B);
        }

        public static PD.XPen ToPen(this PdGraphic input)
        {
            return new PD.XPen(input.Stroke.ToXColor(), input.Weight);
        }

        public static PD.XBrush ToBrush(this PdGraphic input)
        {
            return new PD.XSolidBrush(input.Fill.ToXColor());
        }

        #endregion

        #region -geometry

        public static PD.XPoint[] ToBezierXPoints(this Rg.NurbsCurve input)
        {
            List<PD.XPoint> output = new List<PD.XPoint>();
            Rg.BezierCurve[] beziers = Rg.BezierCurve.CreateCubicBeziers(input, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, Rhino.RhinoDoc.ActiveDoc.PageAngleToleranceRadians);
            foreach (Rg.BezierCurve bezier in beziers)
            {
                output.Add(bezier.GetControlVertex3d(0).ToXPoint());
                output.Add(bezier.GetControlVertex3d(1).ToXPoint());
                output.Add(bezier.GetControlVertex3d(2).ToXPoint());
            }
            output.Add(beziers[beziers.Length - 1].GetControlVertex3d(3).ToXPoint());

            return output.ToArray();
        }

        public static PD.XPoint[] ToXPoints(this Rg.NurbsCurve input)
        {
            List<PD.XPoint> output = new List<PD.XPoint>();
            foreach(Rg.ControlPoint controlPoint in input.Points)
            {
                output.Add(controlPoint.Location.ToXPoint());
            }

            return output.ToArray();
        }

        public static PD.XPoint ToXPoint(this Rg.Point3d input)
        {
            return new PD.XPoint(input.X, input.Y);
        }

        #endregion

        #endregion
    }
}

