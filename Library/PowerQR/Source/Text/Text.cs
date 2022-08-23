using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PowerQR
{
    #region Text Class XML
    /// <summary>
    /// The class used to read and write QR texts.
    /// </summary>
    #endregion
    public static class Text
    {
        #region Generate Method XML
        /// <summary>
        /// Generates a string containing all the bytes/byte values from a byte array.
        /// </summary>
        /// <param name="Bytes">The target byte array.</param>
        /// <param name="NumericalValues">Should the string be generated using numerical values from 0 to 255?</param>
        /// <returns>The generated string.</returns>
        #endregion
        public static string Generate(Byte[] Bytes, bool NumericalValues)
        {
            List<Char> OutputCharacterBuffer = new List<Char>();
            for (int i = 0; i < Bytes.Length; i++) {
                OutputCharacterBuffer.Add((Char)Bytes[i]);
            }


            // Return the output.
            List<String> OutputTokens = new List<String>();
            for (int i = 0; i < OutputCharacterBuffer.Count; i++)
            {
                if (!NumericalValues) {
                    OutputTokens.Add(OutputCharacterBuffer[i].ToString());
                }

                else {
                    OutputTokens.Add(((Byte)OutputCharacterBuffer[i]).ToString());
                }
            }

            string Output = String.Join(((!NumericalValues) ? String.Empty : " "), OutputTokens.ToArray());
            return Output;
        }

        #region Read Method XML
        /// <summary>
        /// Reads a QR code from the specified text.
        /// </summary>
        /// <param name="Text">The target text.</param>
        /// <returns>The read byte array.</returns>
        #endregion
        public static Byte[] Read(String Text)
        {
            List<String> Tokens = Text.Split(' ').ToList();
            List<Byte> Output = new List<Byte>();

            List<Byte> NumericalBuffer = new List<Byte>();
            bool IsNumerical = true;
            for (int i = 0; i < Tokens.Count; i++)
            {
                try {
                    Byte Value = Convert.ToByte(Tokens[i]);
                    NumericalBuffer.Add(Value);
                }

                catch (FormatException) {
                    IsNumerical = false;
                }
            }


            if (!IsNumerical)
            {
                List<Byte> ReadBuffer = new List<Byte>();
                for (int i = 0; i < Text.Length; i++) {
                    Byte[] AsciiEncodingBuffer = Encoding.ASCII.GetBytes(Text[i].ToString());
                    ReadBuffer.Add(AsciiEncodingBuffer[0]);
                }

                Output.AddRange(ReadBuffer);
            }

            else {
                Output.AddRange(NumericalBuffer);
            }


            return Output.ToArray();
        }
    }
}