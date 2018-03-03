/*  
 *  Лабораторна робота №3 – Серіалізаця
    1.	Взяти об’єктну модель з 1ї роботи(у випадку виникнення проблем, можете створити свою модель, 
        або взяти інший варіант, обґрунтувати проблему)
    2.	Реалізувати збереження та зчитування у форматі JSON(3б)
    3.	Реалізувати збереження та зчитування у форматі Protobuf(3б)
    4.	Серіалізація з використанням BinaryFormatter або DataContractSerialzier(4б)
*/


using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Serilization
{
    // Object model
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

    public class Book
    {
        public Guid bookGuid { get; set; }
        public Guid storageGuid { get; set; }
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

    public class Storage
    {
        public Guid storageGuid { get; set; }
        public List<Book> books { get; set; }

        public Storage()
        {
            storageGuid = Guid.NewGuid();
        }
    }

    public static class JsonSerialization
    {
        // Writes the given object instance to a Json file.
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            System.IO.TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = Newtonsoft.Json.JsonConvert.SerializeObject(objectToWrite);
                writer = new System.IO.StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        // Reads an object instance from an Json file.
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            System.IO.TextReader reader = null;
            try
            {
                reader = new System.IO.StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fileContents);
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
            User user = new User() { name = "Lena", surname = "Oxford" };
            List<Book> books = new List<Book>
            {
                new Book(){ year = 1950, name = "1984",  author = "George Orwell" },
                new Book(){ year = 1998, name = "A Brief History of Time", author = "Stephen Hawking" },
            };

            // JSON Serialization
            JsonSerialization.WriteToJsonFile<User>("./user.txt", user);
            JsonSerialization.WriteToJsonFile<List<Book>>("./books.txt", books);

            // JSON Deserialization
            User userJson = JsonSerialization.ReadFromJsonFile<User>("./user.txt");
            List<Book> booksJson = JsonSerialization.ReadFromJsonFile<List<Book>>("./books.txt");

            Console.WriteLine("User: \n {0} {1}", userJson.name, userJson.surname);

            Console.WriteLine("Book List: ");
            foreach (var item in booksJson)
            {
                Console.WriteLine(" {0}, {1}, {2}", item.author, item.name, item.year);
            }
            Console.WriteLine("-----------------------------------------------------");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
