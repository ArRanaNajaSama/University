/*  Лабораторна робота №2 – Файлові потоки
 1.	Створити два класи(можна взяти з попереднього завдання)
 3.	Запис/зчитування з викорстанням бінарного файлу (4б) */

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

        public User(string _name, string _surname)
        {
            userGuid = Guid.NewGuid();
            name = _name;
            surname = _surname;
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

        public Book(uint _year, string _name, string _author)
        {
            bookGuid = Guid.NewGuid();
            userGuid = Guid.Empty; // default value is null
            year = _year;
            author = _author;
            name = _name;
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

    class Programm
    {
        static void Main()
        {
            User user = new User("Lena", "Oxford");
            List<Book> books = new List<Book>
            {
                new Book(1950, "1984", "George Orwell"),
                new Book(1998, "A Brief History of Time", "Stephen Hawking"),
                new Book(2012, "The Grand Design", "Stephen Hawking"),
                new Book(1984, "Alice's Adventures in Wonderland & Through the Looking-Glass", "Lewis Carroll"),
                new Book(2012, "Fahrenheit 451", "Ray Bradbury"),
                new Book(2016, "Fantastic Beasts and Where to Find Them", "J.K. Rowling"),
            };

            // writting in binary
            BinarySerialization.WriteToBinaryFile<List<Book>>("./books.bin", books);
            BinarySerialization.WriteToBinaryFile<User>("./user.bin", user);

            //reading from binary
            User user2 = BinarySerialization.ReadFromBinaryFile<User>("./user.bin");
            List<Book> books2 = BinarySerialization.ReadFromBinaryFile<List<Book>>("./books.bin");

            Console.WriteLine("User: \n {0} {1}", user2.name, user2.surname);

            Console.WriteLine("Book List: ");
            foreach (var item in books2)
            {
                Console.WriteLine(" {0}, {1}, {2}", item.author, item.name, item.year);
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
