using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace GhPdf.Components
{
    public class GH_Pdf_Document_Set : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Pdf_Document_Set class.
        /// </summary>
        public GH_Pdf_Document_Set()
          : base("Set Pdf Contents", "Pdf Set",
              "Sequentially add Blocks to a Pdf Document",
              "CORE", "Pdf")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Pdf Document", "Pdf", "A Pdf Document Object", GH_ParamAccess.item);
            pManager[0].Optional = false;
            pManager.AddGenericParameter("Block", "Blk", "A list of Pdf Block Objects to add to the Pdf Document", GH_ParamAccess.list);
            pManager[1].Optional = false;
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
            IGH_Goo gooA = null;
            if (!DA.GetData(0, ref gooA)) return;

            if (!gooA.CastTo<PdDocument>(out PdDocument document))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Pdf input must be a Pdf Document Object");
                return;
            }
            document = new PdDocument(document);

            List<IGH_Goo> gooB = new List<IGH_Goo>();
            if (!DA.GetDataList(1, gooB)) return;

            foreach (IGH_Goo goo in gooB)
            {
                if (goo.CastTo<PdBlock>(out PdBlock block))
                {
                    document.Blocks.Add(block);
                }
                else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Blk input must all be Pdf Block Objects");
                    return;
                }
            }

            DA.SetData(0, document);
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
                return Properties.Resources.Icons_Set_Content;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("AE33270E-5AA6-47A0-AD41-957A3DE23AE6"); }
        }
    }
}