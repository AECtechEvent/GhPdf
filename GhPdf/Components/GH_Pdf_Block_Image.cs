using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Sd = System.Drawing;

namespace GhPdf.Components
{
    public class GH_Pdf_Block_Image : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Pdf_Content_Image class.
        /// </summary>
        public GH_Pdf_Block_Image()
          : base("Image Block", "Pdf Image",
              "Create a Pdf Image Block Object",
              "CORE", "Pdf")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "A System Drawing Bitmap or full FilePath to an image file", GH_ParamAccess.item);
            pManager[0].Optional = false;
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
            IGH_Goo gooA = null;
            if (!DA.GetData(0, ref gooA)) return;

            bool isValid = false;
            if (gooA.CastTo<Sd.Bitmap>(out Sd.Bitmap image)) isValid = true;

            if (!isValid)
            {
                if (gooA.CastTo<string>(out string filepath))
                {
                    if (System.IO.File.Exists(filepath))
                    {
                        image = new Sd.Bitmap(filepath);
                        isValid = true;
                    }
                    else
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "I input must be a System Drawing Bitmap or a full file path to an image file");
                        return;
                    }
                }
                else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "I input must be a System Drawing Bitmap or a full file path to an image file");
                    return;
                }
            }

            if (isValid)
            {
                PdBlock block = new PdBlock(image);
                DA.SetData(0, block);
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
                return Properties.Resources.Icons_Image;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("C02E754E-9FEC-459E-86CA-4F677E467AA0"); }
        }
    }
}