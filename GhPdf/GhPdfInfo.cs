using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GhPdf
{
    public class GhPdfInfo : GH_AssemblyInfo
    {
        public override string Name => "GhPdf";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("AA70A43A-1D7F-47DD-8234-6DC7CFA5133F");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}