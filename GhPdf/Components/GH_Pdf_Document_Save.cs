using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace GhPdf.Components
{
    public class GH_Pdf_Document_Save : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Pdf_Document_Save class.
        /// </summary>
        public GH_Pdf_Document_Save()
          : base("Save Pdf Document", "Pdf Save",
              "Save a Pdf Document to a file",
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
            pManager.AddTextParameter("Directory", "D", "The directory location to save the file", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddBooleanParameter("Activate", "_A", "If true, the component will be activated.", GH_ParamAccess.item, false);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Filepath", "F", "The full filepath to the written Pdf document", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool activate = false;
            if (DA.GetData(2, ref activate))
            {
                if (activate)
                {
                    IGH_Goo gooA = null;
                    if (!DA.GetData(0, ref gooA)) return;

                    if (!gooA.CastTo<PdDocument>(out PdDocument document))
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Doc input must be a Pdf Document Object");
                        return;
                    }
                    document = new PdDocument(document);

                    string directory = string.Empty;

                    if (DA.GetData(1, ref directory))
                    {
                        if (!System.IO.Directory.Exists(directory))
                        {
                            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The specified directory does not exist");
                            return;
                        }
                    }

                    DA.SetData(0,document.Save(directory));
                }
            }
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
                return Properties.Resources.Icons_Save_Pdf;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("DF9B5C0B-5D20-4655-82FF-D5C6BB634D45"); }
        }
    }
}