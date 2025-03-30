using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace GhPdf.Components
{
    public class GH_Pdf_Document : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Pdf_Document class.
        /// </summary>
        public GH_Pdf_Document()
          : base("Pdf Document", "Pdf Doc",
              "Create an empty Pdf Document",
              "CORE", "Pdf")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "The Document Name", GH_ParamAccess.item, "Document 1");
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Width", "W", "The PDF Document width in the chosen units", GH_ParamAccess.item, 210);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Height", "H", "The PDF Document height in the chosen units", GH_ParamAccess.item, 297);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Units", "U", "The PDF Document units", GH_ParamAccess.item, 0);
            pManager[3].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[3];
            foreach (Units value in Enum.GetValues(typeof(Units)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Pdf Document", "Pdf", "A Pdf Document Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = string.Empty;
            if (!DA.GetData(0, ref name)) return;

            double width = 210;
            DA.GetData(1, ref width);

            double height = 297;
            DA.GetData(2, ref height);

            int unit = 0;
            DA.GetData(3, ref unit);

            DA.SetData(0,new PdDocument(name, width, height, (Units)unit));
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
                return Properties.Resources.Icons_Document;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2005DEA0-D6AC-47CF-AA4F-405F89E46B01"); }
        }
    }
}