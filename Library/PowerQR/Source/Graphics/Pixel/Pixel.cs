using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerQR
{
    #region Pixel Struct XML
    /// <summary>
    /// A pixel struct.
    /// </summary>
    #endregion
    [Serializable] public struct Pixel
    {
        public Byte Byte { get; private set; }
        public Color Color {
            get {
                return new Color(Byte, Byte, Byte, 0xFF);
            }
        }

        #region Pixel Constructor XML
        /// <summary>
        /// Initializes a new instance of the <c>Pixel</c> struct.
        /// </summary>
        /// <param name="Byte">The byte value of the pixel.</param>
        #endregion
        public Pixel(Byte Byte) {
            this.Byte = Byte;
        }
    }
}
