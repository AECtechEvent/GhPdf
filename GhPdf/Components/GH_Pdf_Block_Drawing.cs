using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Sd = System.Drawing;

namespace GhPdf.Components
{
    public class GH_Pdf_Block_Drawing : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Pdf_Content_Drawing class.
        /// </summary>
        public GH_Pdf_Block_Drawing()
          : base("Drawing Block", "Pdf Drawing",
              "Create a Pdf Drawing Block Object",
              "CORE", "Pdf")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "Curve Objects", GH_ParamAccess.list);
            pManager[0].Optional = false;
            pManager.AddColourParameter("Fills", "*F", "Optional Fill Color corresponding to each Curve", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddColourParameter("Strokes", "*S", "Optional Stroke Color corresponding to each Curve", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Weights", "*W", "Optional Stoke Width corresponding to each Curve", GH_ParamAccess.list);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Pdf Block", "Blk", "A Pdf Block Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> curves = new List<Curve>();
            if (!DA.GetDataList(0, curves)) return;

            List<Sd.Color> fills = new List<Sd.Color>();
            DA.GetDataList(1, fills);
            if (fills.Count == 0) fills.Add(Sd.Color.Transparent);
            for (int i = fills.Count; i < curves.Count; i++) fills.Add(fills[fills.Count - 1]);

            List<Sd.Color> strokes = new List<Sd.Color>();
            DA.GetDataList(2, strokes);
            if (strokes.Count == 0) strokes.Add(Sd.Color.Black);
            for (int i = strokes.Count; i < curves.Count; i++) strokes.Add(strokes[strokes.Count - 1]);

            List<double> weights = new List<double>();
            DA.GetDataList(3, weights);
            if (weights.Count == 0) weights.Add(1.0);
            for (int i = weights.Count; i < curves.Count; i++) weights.Add(weights[weights.Count - 1]);

            List<PdShape> shapes = new List<PdShape>();
            for (int i = 0; i < curves.Count; i++) shapes.Add(new PdShape(curves[i], new PdGraphic(fills[i], strokes[i], weights[i])));

            PdBlock block = new PdBlock(new PdDrawing(shapes));
            DA.SetData(0, block);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.Icons_Drawing;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9F9A6700-B7F7-4C50-B5E7-14BFC284429D"); }
        }
    }
}