using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Numerics;
using System.Runtime.Versioning;
using PowerCMD;
using PowerQR;

namespace PowerQR.CLI
{
    [SupportedOSPlatform("windows")]
    class Program
    {
        static void Main(string[] Args)
        {
            List<string> ArgumentList = Environment.GetCommandLineArgs().ToList();
            ArgumentList.RemoveAt(0);
            string Arguments = String.Join(" ", ArgumentList);

            Registry Registry = new Registry();
            ExecutionSystem ExecutionSystem = new ExecutionSystem(Registry);
            Parser Parser = new Parser("\"", " ", "~NL~");

            Registry.Register("Write", new Action<string, string, string, string>(Write).Method, false);
            Registry.Register("Read", new Action<string, string, string, string>(Read).Method, false);


            string Command = String.Empty;
            if (String.IsNullOrEmpty(Arguments)) {
                Console.WriteLine("No arguments found. Type your command..");
                Console.Write("> ");

                Command = Console.ReadLine();
            }

            else {
                Command = Arguments;
            }

            Return Return = Parser.Parse(Command, ExecutionSystem);
            if (Return.Result == Result.Fail) Console.WriteLine("Operation Failed.");


            if (String.IsNullOrEmpty(Arguments)) {
                InputCommandLoop:
                Console.WriteLine();

                Console.Write("> ");
                Command = Console.ReadLine();
                Return = Parser.Parse(Command, ExecutionSystem);
                if (Return.Result == Result.Fail) Console.WriteLine("Operation Failed.");
                goto InputCommandLoop;
            }

            else {
                Thread.Sleep(6500);
            }
        }

        public static void Write(string Content, string Output, string OperationType, string NumericalText)
        {
            try
            {
                Stopwatch Stopwatch = new Stopwatch();
                Byte[] Bytes = null;
                try {
                    Bytes = File.ReadAllBytes(Path.GetFullPath(Content));
                }

                catch (Exception) {
                    Console.WriteLine("Couldn't use the content value as a path. The content value itself will be used.");
                    Bytes = Encoding.ASCII.GetBytes(Content);
                }

                switch (OperationType.ToUpper())
                {
                    case "IMAGE":
                        break;

                    case "TEXT":
                        break;


                    default:
                        Console.WriteLine("Invalid operation type!");
                        return;
                }

                bool NumericalTextWriting = false;
                if (OperationType.ToUpper() == "TEXT")
                {
                    try {
                        NumericalTextWriting = Convert.ToBoolean(NumericalText);
                    }

                    catch (Exception) {
                        Console.WriteLine("Couldn't parse value for text type, defaulting to Lexical Text.");
                    }
                }

                Stopwatch.Reset();
                Stopwatch.Start();
                Image QRImg = null;
                string QRCode = String.Empty;
                if (OperationType.ToUpper() == "IMAGE") {
                    QRImg = QR.Generate(Bytes);
                }

                else if (OperationType.ToUpper() == "TEXT") {
                    QRCode = Text.Generate(Bytes, NumericalTextWriting);
                }

                Stopwatch.Stop();
                Console.WriteLine($"Generated QR code in {Stopwatch.ElapsedMilliseconds} ms.");


                Bitmap BMP = null;
                if (OperationType.ToUpper() == "IMAGE")
                {
                    Stopwatch.Reset();
                    Stopwatch.Start();
                    BMP = new Bitmap((int)QRImg.Size.X, (int)QRImg.Size.Y);
                    for (int Y = 0; Y < BMP.Height; Y++)
                    {
                        for (int X = 0; X < BMP.Width; X++)
                        {
                            BMP.SetPixel(X, Y, System.Drawing.Color.FromArgb(QRImg.Pixels[X, Y].Color.A, QRImg.Pixels[X, Y].Color.R, QRImg.Pixels[X, Y].Color.G, QRImg.Pixels[X, Y].Color.B));
                        }
                    }

                    Stopwatch.Stop();
                    Console.WriteLine($"Composed bitmap in {Stopwatch.ElapsedMilliseconds} ms.");
                }

                Stopwatch.Reset();
                Stopwatch.Start();
                using (FileStream FS = File.Open(Output, FileMode.Create))
                {
                    if (OperationType.ToUpper() == "IMAGE") {
                        BMP.Save(FS, System.Drawing.Imaging.ImageFormat.Png);
                    }

                    else if (OperationType.ToUpper() == "TEXT")
                    {
                        for (int Y = 0; Y < QRCode.Length; Y++)
                        {
                            Byte NextByte = (Byte)QRCode[Y];

                            FS.Position = Y;
                            FS.WriteByte(NextByte);
                        }
                    }
                }

                Stopwatch.Stop();


                Console.WriteLine($"Wrote output in {Stopwatch.ElapsedMilliseconds} ms.");
            }

            /*catch (Exception E) {
                Console.WriteLine(E.Message);
                Console.WriteLine(E.StackTrace);

                throw;
            }*/

            finally {
            
            }
        }

        public static void Read(string ContentPath, string Output, string OperationType, string StripEndEOSBytes)
        {
            try
            {
                switch (OperationType.ToUpper()) {
                    case "IMAGE":
                        break;

                    case "TEXT":
                        break;


                    default:
                        Console.WriteLine("Invalid operation type!");
                        return;
                }

                bool StripEOS = false;
                try {
                    StripEOS = Convert.ToBoolean(StripEndEOSBytes);
                }

                catch (Exception) {
                    Console.WriteLine("Couldn't parse value for stipping end null bytes, defaulting to false.");
                }

                Stopwatch Stopwatch = new Stopwatch();
                Stopwatch.Start();
                string Content = ((OperationType.ToUpper() == "TEXT") ? File.ReadAllText(ContentPath) : String.Empty);
                Bitmap BMP = ((OperationType.ToUpper() == "IMAGE") ? new Bitmap(ContentPath) : null);
                Stopwatch.Stop();
                Console.WriteLine($"Read content in {Stopwatch.ElapsedMilliseconds} ms.");

                Image ReadImage = ((OperationType.ToUpper() == "IMAGE") ? new Image(new Vector2(BMP.Width, BMP.Height)) : null);
                if (OperationType.ToUpper() == "IMAGE")
                {
                    Stopwatch.Reset();
                    Stopwatch.Start();
                    for (int Y = 0; Y < ReadImage.Size.Y; Y++)
                    {
                        for (int X = 0; X < ReadImage.Size.X; X++)
                        {
                            System.Drawing.Color Color = BMP.GetPixel(X, Y);
                            if ((Color.R == Color.G) && (Color.G == Color.B) && (Color.R == Color.B))
                            {
                                Byte ByteValue = (Byte)((Color.R + Color.G + Color.B) / 3);
                                ReadImage.Set(new Vector2(X, Y), ByteValue);
                            }
                        }
                    }

                    Stopwatch.Stop();
                    Console.WriteLine($"Composed QR image from bitmap in {Stopwatch.ElapsedMilliseconds} ms.");
                }

                Stopwatch.Reset();
                Stopwatch.Start();

                Byte[] QRCodeBytes = ((OperationType.ToUpper() == "IMAGE") ? QR.Read(ReadImage, StripEOS) : Text.Read(Content));
                File.WriteAllBytes(Output, QRCodeBytes);
                Stopwatch.Stop();

                Console.WriteLine($"Wrote output to file in {Stopwatch.ElapsedMilliseconds} ms.");
            }

            /*catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine(E.StackTrace);

                throw;
            }*/

            finally {
            
            }
        }
    }
}