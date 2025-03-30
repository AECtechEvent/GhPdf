using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace GhPdf
{
    public class PdFont
    {

        #region members

        public string Family = "Arial";
        public double Size = 12.0;
        public Sd.Color Color = Sd.Color.Black;

        #endregion

        #region constructors

        public PdFont()
        {

        }

        public PdFont(PdFont font)
        {
            this.Family = font.Family;
            this.Size = font.Size;
            this.Color = font.Color;
        }

        #endregion

        #region properties



        #endregion

        #region methods



        #endregion

        #region overrides



        #endregion

    }
}
