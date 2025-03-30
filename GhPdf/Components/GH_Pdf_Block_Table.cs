using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Sd = System.Drawing;

namespace GhPdf.Components
{
    public class GH_Pdf_Block_Table : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Pdf_Content_Table class.
        /// </summary>
        public GH_Pdf_Block_Table()
          : base("Table Block", "Pdf Table",
              "Create a Pdf Table Block Object",
              "CORE", "Pdf")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Values", "V", "A datatree of text values", GH_ParamAccess.tree);
            pManager[0].Optional = false;
            pManager.AddNumberParameter("Size", "*S", "Font Size", GH_ParamAccess.item, 12);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Family", "*F", "Font Family", GH_ParamAccess.item, "Arial");
            pManager[2].Optional = true;
            pManager.AddColourParameter("Color", "*C", "Font Color", GH_ParamAccess.item, System.Drawing.Color.Black);
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
            List<List<string>> dataSet = new List<List<string>>();
            if (!DA.GetDataTree(0, out GH_Structure<GH_String> ghData)) return;

            foreach (List<GH_String> data in ghData.Branches)
            {
                List<string> values = new List<string>();
                foreach (GH_String value in data)
                {
                    values.Add(value.Value);
                }
                dataSet.Add(values);
            }

            PdFont font = new PdFont();

            double size = 12.0;
            if (DA.GetData(1, ref size)) font.Size = size;

            string family = "Arial";
            if (DA.GetData(2, ref family)) font.Family = family;

            Sd.Color color = Sd.Color.Black;
            if (DA.GetData(3, ref color)) font.Color = color;

            PdBlock block = new PdBlock(dataSet,font);
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
                return Properties.Resources.Icons_Table;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("AF8832B2-95E6-447D-A6FF-E6455B5D5906"); }
        }
    }
}