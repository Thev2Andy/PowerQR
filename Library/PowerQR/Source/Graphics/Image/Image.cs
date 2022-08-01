using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PowerQR
{
    #region Image Class XML
    /// <summary>
    /// A custom image class.
    /// </summary>
    #endregion
    [Serializable] public class Image
    {
        public Vector2 Size { get { return new Vector2((UInt64)Pixels.GetLongLength(0), (UInt64)Pixels.GetLongLength(1)); } }
        public Pixel[,] Pixels { get; internal set; }

        public static int HeightDimension { get { return 1; } }
        public static int WidthDimension { get { return 0; } }

        public event Action<Vector2> OnResize;

        #region Set Method XML
        /// <summary>
        /// Sets a byte into a pixel at a specified XY location.
        /// </summary>
        /// <param name="Location">The location of the pixel.</param>
        /// <param name="Byte">The byte to set.</param>
        #endregion
        public void Set(Vector2 Location, Byte Byte) {
            Pixels[(UInt64)Location.X, (UInt64)Location.Y] = new Pixel(Byte);
        }

        #region Resize Method XML
        /// <summary>
        /// A RGBA color class.
        /// </summary>
        /// <param name="Size">The new size of the image.</param>
        /// <exception cref="ArgumentException">Image size is set to a number less than or equal to 0.</exception>
        #endregion
        public void Resize(Vector2 Size) {
            if(Size.X <= 0 || Size.Y <= 0) {
                throw new ArgumentException("Image size cannot be less than or equal to 0.");
            }

            Pixel[,] NewPixels = new Pixel[(UInt64)Size.X, (UInt64)Size.Y];
            for (UInt64 y = 0; y < (UInt64)Pixels.GetLongLength(HeightDimension); y++)
            {
                for (UInt64 x = 0; x < (UInt64)Pixels.GetLongLength(WidthDimension); x++)
                {
                    NewPixels[x, y] = Pixels[x, y];
                }
            }

            OnResize?.Invoke(Size);
            Pixels = NewPixels;
        }

        #region Clear Method XML
        /// <summary>
        /// Clears the image.
        /// </summary>
        #endregion
        public void Clear() {
            Pixels = new Pixel[(UInt64)Size.X, (UInt64)Size.Y];
        }


        #region Image Constructor XML
        /// <summary>
        /// Initializes a new instance of the <c>Image</c> class of the appropiate size.
        /// </summary>
        /// <param name="Size"></param>
        /// <exception cref="ArgumentException">Image size is set to a number less than or equal to 0.</exception>
        #endregion
        public Image(Vector2 Size) {
            if(Size.X <= 0 || Size.Y <= 0) {
                throw new ArgumentException("Image size cannot be less than or equal to 0.");
            }

            Pixels = new Pixel[(UInt64)Size.X, (UInt64)Size.Y];
        }
    }
}
