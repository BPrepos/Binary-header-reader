using System;
using System.IO;

namespace EmediaProjekt1
{

    class Program
    {

        static void ReadJpegResolution(string nameOfFile, out int sizeY, out int sizeX)
        {
            sizeY = sizeX = 0;
            bool done = false;
            bool eof = false;

            FileStream stream = new FileStream(nameOfFile, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            while (!done || eof)
            {
                reader.ReadByte();
                byte marker = reader.ReadByte();
                int length = 0;
                switch (marker)
                {
                    case 0xD8:
                    case 0xD9:
                        length = 0;
                        break;
                    default:
                        int len1stbyte = reader.ReadByte();
                        int len2ndbyte = reader.ReadByte();
                        length = (len1stbyte << 8 | len2ndbyte) - 2;
                        break;
                }
                if (marker == 0xD9)
                    eof = true;

                if (length > 0)
                {
                    byte[] data = reader.ReadBytes(length);
                    if (marker == 0xC0)
                    {
                        sizeX = data[1] << 8 | data[2];
                        sizeY = data[3] << 8 | data[4];
                        done = true;
                    }
                }
            }
            reader.Close();
            stream.Close();
        }

        static void Main(string[] args)
        {
            string nameOfFile;
            Console.WriteLine("Podaj nazwe pliku: ");
            nameOfFile = Console.ReadLine();
            int width, height;
            ReadJpegResolution(nameOfFile , out width, out height);
            System.Console.WriteLine(width + " x " + height);
            Console.Read();
        }
    }
}