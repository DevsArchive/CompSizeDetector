using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SonicRetro.KensSharp;

namespace CompSizeDetector
{
    internal class Program
    {
        enum CompressType
        {
            None,
            Enigma,
            Kosinski,
            KosinskiMod,
            Nemesis,
            Saxman
        }

        static List<string> files = new List<string>();
        static CompressType compressType = CompressType.None;
        static bool error = false;

        static void SetCompression(CompressType type)
        {
            if (compressType == CompressType.None)
                compressType = type;
            else
            {
                Console.WriteLine("Error: Pick only one compression type.");
                error = true;
            }
        }

        static void Help()
        {
            Console.WriteLine($"CompSizeDetector [files] [-c/--comper] [-e/--enigma] [-k/--kosinski] " +
                    "[-k/--kosinski-mod] [-n/--nemesis] [-s/--saxman]\n\n" +
                    "files              - Files to detect compressed data size from\n" +
                    "-e/--enigma        - Treat data as compressed in Enigma\n" +
                    "-k/--kosinski      - Treat data as compressed in Kosinski\n" +
                    "-km/--kosinski-mod - Treat data as compressed in Kosinski Moduled\n" +
                    "-n/--nemesis       - Treat data as compressed in Nemesis\n" +
                    "-s/--saxman        - Treat data as compressed in Saxman\n");
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Help();
            }
            else
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-e":
                        case "--enigma":
                            SetCompression(CompressType.Enigma);
                            break;

                        case "-k":
                        case "--kosinski":
                            SetCompression(CompressType.Kosinski);
                            break;

                        case "-km":
                        case "--kosinski-mod":
                            SetCompression(CompressType.KosinskiMod);
                            break;

                        case "-n":
                        case "--nemesis":
                            SetCompression(CompressType.Nemesis);
                            break;

                        case "-s":
                        case "--saxman":
                            SetCompression(CompressType.Saxman);
                            break;

                        default:
                            files.Add(arg);
                            break;
                    }
                }

                if (files.Count == 0)
                {
                    Console.WriteLine("Error: No input files.");
                    error = true;
                }

                if (compressType == CompressType.None)
                {
                    Console.WriteLine("Error: No compression type picked.");
                    error = true;
                }

                if (!error)
                {
                    foreach (string fileName in files)
                    {
                        if (File.Exists(fileName))
                        {
                            try
                            {
                                using (FileStream input = File.OpenRead(fileName))
                                using (MemoryStream output = new MemoryStream())
                                {
                                    switch (compressType)
                                    {
                                        case CompressType.Enigma:
                                            Enigma.Decompress(input, output, Endianness.BigEndian);
                                            break;

                                        case CompressType.Kosinski:
                                            Kosinski.Decompress(input, output);
                                            break;

                                        case CompressType.KosinskiMod:
                                            ModuledKosinski.Decompress(input, output, Endianness.BigEndian);
                                            break;

                                        case CompressType.Nemesis:
                                            Nemesis.Decompress(input, output);
                                            break;

                                        case CompressType.Saxman:
                                            Saxman.Decompress(input, output);
                                            break;
                                    }

                                    long leftoverSize = input.Length - input.Position;

                                    Console.WriteLine($"[{fileName}]");
                                    Console.WriteLine($"Compressed data size:   0x{input.Position:X} ({input.Position}) bytes");
                                    Console.WriteLine($"Decompressed data size: 0x{output.Position:X} ({output.Position}) bytes");
                                    Console.WriteLine($"Leftover data size:     0x{leftoverSize:X} ({leftoverSize}) bytes\n");
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"Error: Failed to decompress data in \"{fileName}\".");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error: \"{fileName}\" does not exist.");
                        }
                    }
                }
            }
        }
    }
}
