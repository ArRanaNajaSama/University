/*  Лабораторна робота №2 – Файлові потоки
 1. Створити два класи(можна взяти з попереднього завдання)
 2. Запис/зчитування цих об’єктів в/з текстового файлу (3б)
 3. Запис/зчитування з викорстанням бінарного файлу (4б) */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileStreaming
{

    [Serializable]
    public class User
    {
        public Guid userGuid { get; set; }
        public string name { get; set; }
        public string surname { get; set; }

        public User()
        {
            userGuid = Guid.NewGuid();
        }
    }

    [Serializable]
    public class Book
    {
        public Guid bookGuid { get; set; }
        public Guid userGuid { get; set; }
        public uint year { get; set; }
        public string author { get; set; }
        public string name { get; set; }

        public Book()
        {
            bookGuid = Guid.NewGuid();
            userGuid = Guid.Empty; // default value is null
        }
    }

    //Functions for performing common binary Serialization operations.
    public static class BinarySerialization
    {
        // Writes the given object instance to a binary file.
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// Reads an object instance from a binary file.
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }

    public static class XmlSerialization
    {
        // Writes the given object instance to an XML file.
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        // Reads an object instance from an XML file.
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }

    class Programm
    {
        static void Main()
        {
            User user = new User(){ name = "Lena", surname = "Oxford"};
            List<Book> books = new List<Book>
            {
                new Book(){ year = 1950, name = "1984",  author = "George Orwell" },
                new Book(){ year = 1998, name = "A Brief History of Time", author = "Stephen Hawking" },
            };

            // writting in binary
            BinarySerialization.WriteToBinaryFile<User>("./user.bin", user);
            BinarySerialization.WriteToBinaryFile<List<Book>>("./books.bin", books);

            //writting a txt file
            XmlSerialization.WriteToXmlFile<User>("./user.txt", user);
            XmlSerialization.WriteToXmlFile<List<Book>>("./book.txt", books);

            //reading from binary
            User user2 = BinarySerialization.ReadFromBinaryFile<User>("./user.bin");
            List<Book> books2 = BinarySerialization.ReadFromBinaryFile<List<Book>>("./books.bin");

            //reading from txt
            User user3 = XmlSerialization.ReadFromXmlFile<User>("./user.txt");
            List<Book> books3 = XmlSerialization.ReadFromXmlFile<List<Book>>("./book.txt");

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("User: \n {0} {1}", user2.name, user2.surname);

            Console.WriteLine("Book List: ");
            foreach (var item in books2)
            {
                Console.WriteLine(" {0}, {1}, {2}", item.author, item.name, item.year);
            }

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("User: \n {0} {1}", user3.name, user3.surname);

            Console.WriteLine("Book List: ");
            foreach (var item in books3)
            {
                Console.WriteLine(" {0}, {1}, {2}", item.author, item.name, item.year);
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
