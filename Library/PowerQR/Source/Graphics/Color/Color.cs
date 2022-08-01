using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerQR
{
    #region Color Struct XML
    /// <summary>
    /// A RGBA color structute.
    /// </summary>
    #endregion
    [Serializable] public struct Color
    {
        public Byte R { get; private set; }
        public Byte G { get; private set; }
        public Byte B { get; private set; }
        public Byte A { get; private set; }

        #region Color Constructor XML
        /// <summary>
        /// Initializes an instace of the <c>Color</c> struct with the appropiate values.
        /// </summary>
        /// <param name="R">The R channel.</param>
        /// <param name="G">The G channel.</param>
        /// <param name="B">The B channel.</param>
        /// <param name="A">The A channel.</param>
        #endregion
        public Color(Byte R, Byte G, Byte B, Byte A) {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }
    }
}
