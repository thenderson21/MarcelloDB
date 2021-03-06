
<img  width="60%" src="http://markmeeus.github.io/MarcelloDB/images/logo/logo_blue.svg"/>

[![Build Status](https://travis-ci.org/markmeeus/MarcelloDB.svg?branch=master)](https://travis-ci.org/markmeeus/MarcelloDB)

MarcelloDB is a mobile NoSql database for Xamarin, Windows(Phone) 8.1/10 and regular .net/mono.
It is very light-weight with minimal memory footprint.

MarcelloDB saves plain C# objects, including child objects, lists and dictionaries.
Not having to map your objects to the relational model can save you hundreds of lines of code.

It's a pure C# implementation, no need to package other binaries.

#Documentation
Read the docs here: [http://www.marcellodb.org](http://www.marcellodb.org)

#Current Status

Current version: 0.4.0. See [the roadmap](http://www.marcellodb.org/roadmap.html).

Although still under heavy development, both the api and the file format are already quite stable.

Be carefull. Backwards compatibility with existing data will not be guaranteed untill v1.x

###Upgrading to 0.4
There are a few breaking changes from 0.3 to 0.4, read about them [here](http://www.marcellodb.org/upgrade04.html)

###Installation
```cs
PM > Install-Package MarcelloDB
```

###A simple console app to get you started.

```cs

  public class Book{
      public string BookId { get; set; }
      public string Title { get; set; }
  }

  class MainClass
  {
      public static void Main (string[] args)
      {
          //Create a session
          var platform =  new Platform();
          var session = new MarcelloDB.Session(platform, ".");

          var productsFile = session["products.data"];

          var bookCollection = productsFile.Collection<Book, string>("books", book => book.BookId);

          var newBook = new Book(){ BookId = "123",  Title = "The Girl With The Dragon Tattoo" };

          bookCollection.Persist(newBook);

          Console.WriteLine("Enumerating all books");
          foreach(var book in bookCollection.All)
          {
              Console.WriteLine(book.Title);
          }

          var theBook = bookCollection.Find("123");
          Console.WriteLine("Found book: " + theBook.Title);
          
          bookCollection.Destroy("123");
          
          Console.ReadKey();
      }
  }

```
