﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using H264Sharp;
using System.Threading;
using System.Drawing.Imaging;

namespace EncoderTest
{
    internal class Program
    {
        static H264Sharp.Encoder encoder;
        static H264Sharp.Decoder decoder;
        static void Main(string[] args)
        {
            //const string DllName32 = "openh264-2.3.1-win64.dll";
            const string DllName32 = "openh264-2.3.1-win64.dll";
           // const string DllName32 = "openh264-2.3.1-win32.dll";
            var img = System.Drawing.Image.FromFile("ocean.jpg");
            int w = img.Width;
            int h = img.Height;
            var bmp = new Bitmap(img);
          //  Thread.Sleep(1000);
            //var orig = bmp;
            //Bitmap clone = new Bitmap(orig.Width, orig.Height,
            //    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            //using (Graphics gr = Graphics.FromImage(clone))
            //{
            //    gr.DrawImage(orig, new Rectangle(0, 0, clone.Width, clone.Height));
            //}
            //bmp = clone;
            //bmp.Save("K.jpg");

            Console.WriteLine($"Image Loaded with Width{w}, Height:{h}");

            decoder = new H264Sharp.Decoder();

            encoder = new H264Sharp.Encoder();
            try
            {
                encoder.Initialize(w, h, bps: 200_000_000, fps: 30, H264Sharp.Encoder.ConfigType.CameraBasic);

            }
            catch { }
            byte[] bb = new byte[1000000];
            // Emulating video frames
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"[Frame {i}] Encoding");
                if(encoder.Encode(bmp, out EncodedFrame[] frames))
                {
                    foreach (var frame in frames)
                    {
                        //frame.CopyTo(bb,0);
                        //Encoded(bb, frame.Length, frame.Type);
                        Encoded(frame.Data, frame.Length, frame.Type);
                    }
                   
                }
            }
           Console.WriteLine("\n Time: "+sw.ElapsedMilliseconds);

            Console.ReadLine();
        }

        private static void Encoded(IntPtr data, int length, FrameType type)
        {
            Console.WriteLine($"Encoded image to {length} bytes");
          
            if (decoder.Decode(data, length, noDelay:true, out DecodingState statusCode, out Bitmap bmp)) 
            {
               // bmp.Save("t.bmp");
                //Console.WriteLine($"Decoded image with Width:{bmp.Width}, Height:{bmp.Height}");
            }

        }
    }
}
