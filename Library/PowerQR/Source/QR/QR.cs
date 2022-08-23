using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PowerQR
{
    #region QR Class XML
    /// <summary>
    /// The class used to read and write QR codes.
    /// </summary>
    #endregion
    public static class QR
    {
        #region Generate Method XML
        /// <summary>
        /// Generates an image from a byte array.
        /// </summary>
        /// <param name="Bytes">The target byte array.</param>
        /// <returns>The generated image.</returns>
        #endregion
        public static Image Generate(Byte[] Bytes)
        {
            // Define 2 images.
            Image TemporaryBuffer = new Image(new Vector2(MathF.Ceiling(MathF.Sqrt(Bytes.Length))));
            Image Output = new Image(new Vector2(MathF.Ceiling(MathF.Sqrt(Bytes.Length))));

            // Fill the temporary buffer with null bytes.
            for (int Y = 0; Y < TemporaryBuffer.Size.Y; Y++)
            {
                for (int X = 0; X < TemporaryBuffer.Size.X; X++)
                {
                    TemporaryBuffer.Set(new Vector2(X, Y), 0x00);
                }
            }

            // Generate code.
            try
            {
                for (int Y = 0; Y < TemporaryBuffer.Size.Y; Y++)
                {
                    for (int X = 0; X < TemporaryBuffer.Size.X; X++)
                    {
                        TemporaryBuffer.Set(new Vector2(X, Y), Bytes[(int)MathF.Floor((Y * TemporaryBuffer.Size.X + X))]);
                    }
                }
            }

            catch (IndexOutOfRangeException) { }
            
            finally {
                Output = TemporaryBuffer;
            }


            // Return the output.
            return Output;
        }

        #region Read Method XML
        /// <summary>
        /// Reads a QR code from the specified image.
        /// </summary>
        /// <param name="Image">The target image.</param>
        /// <param name="StripEndEOSBytes">Determines if the trailing null bytes are removed, this can yield a file artificially bigger (by a few bytes, depends on the file size) if disabled, but can (in rare occasions) corrupt files if enabled.</param>
        /// <returns>The read byte array.</returns>
        #endregion
        public static Byte[] Read(Image Image, bool StripEndEOSBytes)
        {
            // Define 2 buffers.
            List<Byte> TemporaryByffer = new List<byte>();
            List<Byte> Output = new List<Byte>();

            // Read the image.
            for (int Y = 0; Y < Image.Size.Y; Y++)
            {
                for (int X = 0; X < Image.Size.X; X++)
                {
                    TemporaryByffer.Add(Image.Pixels[X, Y].Byte);
                }
            }

            // Strip the trailing null bytes / end of string characters introduced by the image generation system.
            if (StripEndEOSBytes) {
                Output = TemporaryByffer.TakeWhile((V, Index) => TemporaryByffer.Skip(Index).Any(W => W != 0x00)).ToList();
            }

            else {
                Output = TemporaryByffer;
            }

            return Output.ToArray();
        }
    }
}