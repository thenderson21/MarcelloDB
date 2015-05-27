﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

using MarcelloDB;
using MarcelloDB.Test.Classes;
using MarcelloDB.Collections;
using MarcelloDB.Storage;
using MarcelloDB.Exceptions;

namespace MarcelloDB.Test
{
    [TestFixture]
    public class MarcelloIntegrationTest
    {
        Session _session;
        Collection<Article> _articles;
        InMemoryStreamProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new InMemoryStreamProvider();
            _session = new Session(_provider);
            _articles = _session.Collection<Article>();
        }
            
        [Test]
        public void Collection_Returns_A_Collection()
        {
            var collection = _articles;
            Assert.NotNull(collection, "Collection should not be null");
        }

        [Test]
        public void Collections_Are_Reused_Per_Session()
        {
            Assert.AreSame(_session.Collection<Article>(), _session.Collection<Article>());
        }

        [Test]
        public void Insert_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            _articles.Persist(toiletPaper);
            var article = _articles.Find(toiletPaper.ID);

            Assert.AreEqual(toiletPaper.Name, article.Name, "First article");
        }            

        [Test]
        public void Insert_2_Objects()
        {
            var toiletPaper = Article.ToiletPaper;

            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist (spinalTapDvd);

            var articleNames = _articles.All.Select(a => a.Name).ToList();

            Assert.AreEqual(new List<string>{toiletPaper.Name, spinalTapDvd.Name}, articleNames, "Should return 2 article names");
        }

        [Test()]
        public void Insert_Multiple_Objects()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(barbieDoll);
        
            var articleNames = _articles.All.Select(a => a.Name).ToList();
            Assert.AreEqual(new List<string>{toiletPaper.Name, spinalTapDvd.Name, barbieDoll.Name}, articleNames, "Should return multiple article names");
        }

        [Test]
        public void Update()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            toiletPaper.Name += "Extra Strong ToiletPaper";

            _articles.Persist(toiletPaper);
        
            var reloadedArticle = _articles.All.Where (a => a.ID == Article.ToiletPaper.ID).FirstOrDefault ();

            Assert.AreEqual(toiletPaper.Name, reloadedArticle.Name, "Should return updated article name");
        }

        [Test]
        public void Small_Update()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            toiletPaper.Name = "Short";
            _articles.Persist(toiletPaper);

            var articleNames = _articles.All.Select(a => a.Name).ToList();
            articleNames.Sort();

            Assert.AreEqual(new List<string>{spinalTapDvd.Name, toiletPaper.Name}, articleNames, "Should return updated article names");
        }

        [Test]
        public void Large_Update()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            toiletPaper.Name += "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus in lorem porta, mollis odio sit amet, tincidunt leo. Aliquam suscipit sapien nec orci fermentum imperdiet. Sed id est ante. Aliquam nec nibh id purus fermentum lobortis. Morbi posuere ullamcorper diam, in tincidunt mi pulvinar ut. Nam imperdiet mi a viverra congue. Proin eros metus, vehicula tempus eros vitae, pulvinar posuere nisi. Sed volutpat laoreet tortor. Sed sagittis nunc sed dui sollicitudin porta. Donec non neque ut erat commodo convallis vel ac dolor. Quisque eu lectus dapibus, varius sem non, semper dolor. Morbi at venenatis tellus. Integer efficitur neque ornare, lobortis nisi suscipit, consequat purus. Aliquam erat volutpat.";

            _articles.Persist(toiletPaper);

            var articleNames = _articles.All.Select(a => a.Name).ToList();
            articleNames.Sort();

            Assert.AreEqual(new List<string>{spinalTapDvd.Name, toiletPaper.Name}, articleNames, "Should return updated article names");
        }

        [Test]
        public void Large_Update_For_Last_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            spinalTapDvd.Name += "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus in lorem porta, mollis odio sit amet, tincidunt leo. Aliquam suscipit sapien nec orci fermentum imperdiet. Sed id est ante. Aliquam nec nibh id purus fermentum lobortis. Morbi posuere ullamcorper diam, in tincidunt mi pulvinar ut. Nam imperdiet mi a viverra congue. Proin eros metus, vehicula tempus eros vitae, pulvinar posuere nisi. Sed volutpat laoreet tortor. Sed sagittis nunc sed dui sollicitudin porta. Donec non neque ut erat commodo convallis vel ac dolor. Quisque eu lectus dapibus, varius sem non, semper dolor. Morbi at venenatis tellus. Integer efficitur neque ornare, lobortis nisi suscipit, consequat purus. Aliquam erat volutpat.";

            _articles.Persist(spinalTapDvd);

            var articleNames = _articles.All.Select(a => a.Name).ToList();
            articleNames.Sort();

            Assert.AreEqual(new List<string>{spinalTapDvd.Name, toiletPaper.Name}, articleNames, "Should return updated article names");
        }
            
        [Test]
        public void Destroy_Only_Object()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);

            _articles.Destroy(toiletPaper);
        
            Assert.AreEqual (0, _articles.All.Count());
        }

        [Test]
        public void Destroy_First_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            _articles.Destroy(toiletPaper);

            Assert.AreEqual(1, _articles.All.Count());
            Assert.AreEqual(spinalTapDvd.ID, _articles.All.First().ID);
        }

        [Test]
        public void Destroy_Middle_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(barbieDoll);

            _articles.Destroy(spinalTapDvd);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Destroy_Last_Object()
        {

            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(barbieDoll);

            _articles.Destroy(barbieDoll);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(spinalTapDvd.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Destroy_Virgin_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);

            Assert.DoesNotThrow(() =>
                {
                _articles.Destroy(spinalTapDvd);
                });
        }

        [Test]
        public void Destroy_Last_Insert_New()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Destroy(spinalTapDvd);

            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(barbieDoll);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Destroy_Only_Insert_New()
        {        
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(spinalTapDvd);
            _articles.Destroy(spinalTapDvd);

            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(barbieDoll);

            Assert.AreEqual(1, _articles.All.Count());
            Assert.AreEqual(barbieDoll.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Destroy_And_Insert_Reuse_Storage_Space()
        {
            var barbieDoll = Article.BarbieDoll;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(barbieDoll);
            _articles.Persist(spinalTapDvd);

            _session.Journal.Apply (); //make sure the journal is applied to the backing stream

            var storageSize = ((InMemoryStream)_provider.GetStream("Article")).BackingStream.Length;
            _articles.Destroy(barbieDoll);
            _articles.Persist(barbieDoll);

            _session.Journal.Apply (); //make sure the journal is applied to the backing stream

            var newStorageSize = ((InMemoryStream)_provider.GetStream("Article")).BackingStream.Length;
            Assert.AreEqual(storageSize, newStorageSize);
        }

        [Test]
        public void Multiple_Delete_And_Insert_Reuse_Storage_Space()
        {
            var barbieDoll = Article.BarbieDoll;
            var spinalTapDvd = Article.SpinalTapDvd;
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(barbieDoll);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(toiletPaper);

            _session.Journal.Apply (); //make sure the journal is applied to the backing stream

            var storageSize = ((InMemoryStream)_provider.GetStream("Article")).BackingStream.Length;
            _articles.Destroy(barbieDoll);
            _articles.Destroy(toiletPaper);

            _articles.Persist(barbieDoll);
            _articles.Persist(toiletPaper);
            _session.Journal.Apply (); //make sure the journal is applied to the backing stream

            var newStorageSize = ((InMemoryStream)_provider.GetStream("Article")).BackingStream.Length;
            Assert.AreEqual(storageSize, newStorageSize);
        }

        [Test]
        public void Can_Handle_Subclasses()
        {            
            var bread = Food.Bread;
            _articles.Persist(bread);
            var breadFromDB = (Food) _articles.All.First();
            Assert.NotNull(breadFromDB);
            Assert.AreEqual(bread.Expires.ToString(), breadFromDB.Expires.ToString());
            Assert.AreEqual(bread.Name, breadFromDB.Name );
        }

        [Test]
        public void Save_To_File_Stream()
        {
            EnsureFolder("data");
            var fileStreamProvider =  new FileStorageStreamProvider("./data/");
            var session = new Session(fileStreamProvider);

            var articles = session.Collection<Article>();

            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            articles.Persist(toiletPaper);
            articles.Persist(spinalTapDvd);
            articles.Persist(barbieDoll);

            var articleNames = articles.All.Select(a => a.Name).ToList();

            Assert.AreEqual(new List<string>{toiletPaper.Name, spinalTapDvd.Name, barbieDoll.Name }, articleNames);
        }

        [Test]
        public void Add1000()
        {
            EnsureFolder("data");
            var fileStreamProvider =  new FileStorageStreamProvider("./data/");            
            var session = new Session(fileStreamProvider);
            var articles = session.Collection<Article>();

            for (int i = 1; i < 1000; i++)
            {
                var a = new Article{ID = i, Name = "Article " + i.ToString()};
                articles.Persist(a);
            }

            for (int i = 1; i < 1000; i++)
            {
                var a = articles.Find(i);
                Assert.AreEqual(i, a.ID, "Article " + i.ToString() + " should have been found.");
            }      
        }

        [Test]
        public void Throw_IDMissingException_When_Object_Has_No_ID_Property()
        {
            Assert.Throws(typeof(IDMissingException), () =>
                {
                    _session.Collection<object>().Persist(new {Name = "Object Without ID"});
                });
        }
            
        private void EnsureFolder(string path)
        {
            if(System.IO.Directory.Exists("data")){
                System.IO.Directory.Delete("data", true);
            }
            System.IO.Directory.CreateDirectory("data");                       
        }
    }
}

