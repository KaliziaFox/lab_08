using library07;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Diagnostics;

internal class Task1
{
    private static void Main()
    {
        Pig pig1 = new Pig("Pig", "Africa", "Pumba", "I'm Symba's best friend!");
        Pig pig2 = new Pig("Pig", "Russia", "Vanya", "I'm really beautiful pig");
        Pig[] animals = { pig1, pig2 };
        XmlSerializer serializer = new XmlSerializer(typeof(Pig[]));
        //сериализация
        using (FileStream xdoc = new FileStream("serialized.xml", FileMode.OpenOrCreate))
            serializer.Serialize(xdoc, animals);
        //Десериализация
        using (FileStream xdoc = new FileStream("serialized.xml", FileMode.OpenOrCreate))
        {
            Pig[] pigs = serializer.Deserialize(xdoc) as Pig[];
            foreach (Pig pig in pigs) Console.WriteLine($"{pig.WhatAnimal} {pig.Country} {pig.Name} {pig.HideFromOtherAnimals}");
        }
    }
}

internal class Task2
{
    public static void search(DirectoryInfo mydir, FileInfo goal)
    {
        DirectoryInfo[] subdir = mydir.GetDirectories();
        FileInfo[] files = mydir.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Name == goal.Name)
            {
                Console.WriteLine($"found in {file.FullName}");
                //вывод содержимого файла
                using (FileStream fs = new FileStream(file.FullName, FileMode.OpenOrCreate))
                {
                    byte[] buffer = new byte[file.Length];
                    fs.Read(buffer, 0, Convert.ToInt32(file.Length));
                    Console.WriteLine(Encoding.Default.GetString(buffer));
                }
                //Сжатие файла
                using (FileStream originalFileStream = File.OpenRead(file.FullName))
                {
                    using (FileStream compressedFileStream = File.Create(file.DirectoryName + "\\compressed.zip"))
                    {
                        using (ZipArchive myarchive = new ZipArchive(compressedFileStream,
                           ZipArchiveMode.Create))
                        {
                            ZipArchiveEntry entry = myarchive.CreateEntry(file.Name);
                            using (Stream entryStream = entry.Open()) originalFileStream.CopyTo(entryStream);
                        }
                    }
                }

                return;
            }

        }
        foreach (DirectoryInfo sub in subdir) { search(sub, goal); }

    }
    //private static void Main()
    //{
    //    Console.WriteLine("Input directory's name:");
    //    DirectoryInfo mydir = new DirectoryInfo(Console.ReadLine());
    //    Console.WriteLine("Input file's name:");
    //    FileInfo goal = new FileInfo(Console.ReadLine());
    //    if (mydir.Exists)
    //    {
    //        DirectoryInfo[] subdir = mydir.GetDirectories();
    //        FileInfo[] files = mydir.GetFiles();
    //        foreach (FileInfo file in files)
    //        {
    //            if (file.Name == goal.Name) Console.WriteLine($"found in {file.FullName}");
    //        }
    //        foreach (DirectoryInfo sub in subdir) { search(sub, goal); }
    //    }
    //}
}