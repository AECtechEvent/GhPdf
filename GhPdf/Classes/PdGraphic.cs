using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace GhPdf
{
    public class PdGraphic
    {

        #region members

        public Sd.Color Fill = Sd.Color.Transparent;
        public Sd.Color Stroke = Sd.Color.Black;
        public double Weight = 1.0;

        #endregion

        #region constructors

        public PdGraphic()
        {

        }

        public PdGraphic(PdGraphic graphic)
        {
            this.Fill = graphic.Fill;
            this.Stroke = graphic.Stroke;
            this.Weight = graphic.Weight;
        }

        public PdGraphic(Sd.Color fill, Sd.Color stroke, double weight)
        {
            this.Fill = fill;
            this.Stroke = stroke;
            this.Weight = weight;
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
