/*
7.	Бібліотечні карти: 
    Класи: Користувач, Книга, Книгосховище
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books_linq
{
    public class User
    {
        public Guid userGuid { get; set; }
        public string name { get; set; }
        public List<Book> books { get; set; }

        public User(string _name)
        {
            userGuid = Guid.NewGuid();
            name = _name;
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

        public Book(uint _year, string _name, string _author)
        {
            bookGuid = Guid.NewGuid();
            userGuid = Guid.Empty; // default value is null
            year = _year;
            author = _author;
            name = _name;
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

    // i. Створити стаичний клас LibrayManager, який буде містити список книг та користувачів
    public static class LibraryManager
    {

        public static List<Book> bookList = new List<Book>
        {
            new Book(1950, "1984", "George Orwell"),
            new Book(1998, "A Brief History of Time", "Stephen Hawking"),
            new Book(2012, "The Grand Design", "Stephen Hawking"),
            new Book(1984, "Alice's Adventures in Wonderland & Through the Looking-Glass", "Lewis Carroll"),
            new Book(2012, "Fahrenheit 451", "Ray Bradbury"),
            new Book(2016, "Fantastic Beasts and Where to Find Them", "J.K. Rowling"),
        };

        public static List<User> userList = new List<User>
        {
            new User("Ana"),
            new User("Lena"),
            new User("Tom"),
            new User("Jerry"),
        };

        // Матиме метод видачі книги користувачу, і повернення книги
        public static bool giveBook(string bname, string uname)
        {
            var query = from user in userList
                        select user;

            foreach (var person in query)
            {
                if (person.name == uname)
                {
                    foreach (var book in bookList)
                    {
                        if (book.name == bname)
                        {
                            if (book.userGuid == Guid.Empty)
                            {
                                book.userGuid = person.userGuid;
                                Console.WriteLine("Update. Book {0} is given to {1}", bname, uname);
                                return true;
                            }
                        }
                    }
                    Console.WriteLine("Error! No such book {0}", bname);
                    return false;
                }
            }
            Console.WriteLine("Error! No such user: {0} ", uname);
            return false;
        }

        public static bool returnBook(string bname)
        {
            var query = from book in bookList
                        select book;
            foreach (var item in query)
            {
                if (item.name == bname)
                {
                    item.userGuid = Guid.Empty;
                    Console.WriteLine("Update. Book {0} is returned back.", bname);
                    return true;
                }
            }
            Console.WriteLine("Error! No such book {0}", bname);
            return false;
        }

        // Метод пошуку книг, які в даний момент на руках у користувачів
        public static void areGiven()
        {
            var query = from book in bookList
                        where book.userGuid != Guid.Empty
                        join user in userList on book.userGuid equals user.userGuid
                        select new { bookName = book.name, currUser = user.name};
            foreach (var item in query)
                Console.WriteLine("Book {0} is given to {1}", item.bookName, item.currUser);
        }
    }

    class Programm
    {
        static void Main()
        {
            // Create storage for books:
            Storage storage = new Storage();

            // Create books
            var bookList = LibraryManager.bookList;

            // Create users
            var userList = LibraryManager.userList;
            var us_query = from user in userList select user;

            // Книгосховище і користувач мають список книг
            Console.WriteLine("Full list of books in Storage:");
            storage.books = bookList.Select(ptr => new Book(ptr.year, ptr.author, ptr.name) { bookGuid = ptr.bookGuid }).ToList();
            foreach (Book book in storage.books)
                Console.WriteLine("{0} {1} {2} {3}", book.bookGuid, book.year, book.author, book.name);

            Console.WriteLine("Full list of books at User:");
            foreach (var item in us_query)
            {
                Console.WriteLine("User: {0}", item.name);
                item.books = bookList.Select(ptr => new Book(ptr.year, ptr.author, ptr.name) { bookGuid = ptr.bookGuid }).ToList();
                foreach (Book book in item.books)
                    Console.WriteLine("{0} {1} {2} {3}", book.bookGuid, book.year, book.author, book.name);
            }

            // Книга містить посилання на книгосховище (навіть коли вона на руках у користувача, вона «знає» звідки її взяли
            Console.WriteLine("Book has a reference to Storage. All the time");
            foreach (var book in bookList)
            {
                book.storageGuid = storage.storageGuid;
                Console.WriteLine("{0} is in {1}", book.bookGuid, storage.storageGuid);
            }

            // Відфільтрувати об’єкти методом Where
            Console.WriteLine("Exampleof using \"where\" & \"orderby\":");
            var book_query = from books in bookList
                             where books.year < 2000
                             orderby books.name
                             select books;
            foreach (var item in book_query)
            {
                Console.WriteLine("{0} by {1}", item.name, item.author);
            }

            // Застосувати методи GroupBy, OrderBy
            Console.WriteLine("Exampleof using \"group by\":");
            var bookGroupQuery = from books in bookList
                                 orderby books.author
                                 group books by books.author;
            foreach (var bookGroup in bookGroupQuery)
            {
                Console.WriteLine("{0}: ", bookGroup.Key);
                foreach (var book in bookGroup)
                {
                    Console.WriteLine(" - {0}:{1}", book.year, book.name);
                }
            }

            // Use static class to give book to user:
            bool isGiven = LibraryManager.giveBook("Random", "Lena"); // Wrong book case
            isGiven = LibraryManager.giveBook("1984", "Oleh"); // Wrong name case
            isGiven = LibraryManager.giveBook("1984", "Lena"); // All OK
            isGiven = LibraryManager.giveBook("A Brief History of Time", "Tom"); // All OK

            // Use static class to list all books currently on hand:
            LibraryManager.areGiven();
            
            // Use static class to give book to user:
            bool isReturned = LibraryManager.returnBook("Random"); // Wrong book case
            isGiven = LibraryManager.returnBook("1984"); // All OK

            // Use static class to list all books currently on hand:
            LibraryManager.areGiven();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
