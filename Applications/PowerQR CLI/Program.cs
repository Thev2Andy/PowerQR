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

            Registry.Register("Write", new Action<string, string>(Write).Method, false);
            Registry.Register("Read", new Action<string, string, string>(Read).Method, false);


            string Command = String.Empty;
            if (String.IsNullOrEmpty(Arguments)) {
                Console.WriteLine("No arguments found. Type your command..");
                Console.Write("> ");

                Command = Console.ReadLine();
            }

            else {
                Command = Arguments;
            }

            Parser.Parse(Command, ExecutionSystem);


            Thread.Sleep(6500);
        }

        public static void Write(string Content, string Output)
        {
            Stopwatch Stopwatch = new Stopwatch();
            Byte[] Bytes = null;
            try {
                Bytes = File.ReadAllBytes(Path.GetFullPath(Content));
            }

            catch (Exception) {
                Console.WriteLine("Couldn't use content value as a path. The content value itself will be used.");
                Bytes = Encoding.ASCII.GetBytes(Content);
            }

            Stopwatch.Reset();
            Stopwatch.Start();
            Image QRImg = QR.Generate(Bytes);
            Stopwatch.Stop();
            Console.WriteLine($"Generated QR code in {Stopwatch.ElapsedMilliseconds} ms.");

            Stopwatch.Reset();
            Stopwatch.Start();
            Bitmap BMP = new Bitmap((int)QRImg.Size.X, (int)QRImg.Size.Y);
            for (int Y = 0; Y < BMP.Height; Y++)
            {
                for (int X = 0; X < BMP.Width; X++)
                {
                    BMP.SetPixel(X, Y, System.Drawing.Color.FromArgb(QRImg.Pixels[X, Y].Color.A, QRImg.Pixels[X, Y].Color.R, QRImg.Pixels[X, Y].Color.G, QRImg.Pixels[X, Y].Color.B));
                }
            }

            Stopwatch.Stop();
            Console.WriteLine($"Composed bitmap in {Stopwatch.ElapsedMilliseconds} ms.");

            Stopwatch.Reset();
            Stopwatch.Start();
            using (FileStream FS = File.Open(Output, FileMode.Create))
            {
                BMP.Save(FS, System.Drawing.Imaging.ImageFormat.Png);
            }
            Stopwatch.Stop();

            Console.WriteLine($"Wrote bitmap in {Stopwatch.ElapsedMilliseconds} ms.");
        }

        public static void Read(string ImagePath, string Output, string StripTrailingNullBytes)
        {
            try
            {
                Stopwatch Stopwatch = new Stopwatch();
                Bitmap BMP = new Bitmap(ImagePath);
                Stopwatch.Stop();
                Console.WriteLine($"Created bitmap in {Stopwatch.ElapsedMilliseconds} ms.");

                Stopwatch.Reset();
                Stopwatch.Start();
                Image ReadImage = new Image(new Vector2(BMP.Width, BMP.Height));
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

                Stopwatch.Reset();
                Stopwatch.Start();
                bool StripEndNullBytes = false;
                try {
                    StripEndNullBytes = Convert.ToBoolean(StripTrailingNullBytes);
                }

                catch (FormatException) {
                    Console.WriteLine($"Invalid boolean value at parameter `{nameof(StripTrailingNullBytes)}`, defaulting to `{StripTrailingNullBytes}`..");
                }

                Byte[] QRCodeBytes = QR.Read(ReadImage, StripEndNullBytes);
                File.WriteAllBytes(Output, QRCodeBytes);
                Stopwatch.Stop();

                Console.WriteLine($"Wrote output to file in {Stopwatch.ElapsedMilliseconds} ms.");
            }

            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine(E.StackTrace);
            }
        }
    }
}