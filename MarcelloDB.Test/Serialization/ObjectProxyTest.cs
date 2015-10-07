﻿using System;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{   
    [TestFixture]
    public class ObjectProxyTest
    {
        [Test]
        public void Returns_Null_If_No_ID()
        {
            Assert.AreEqual(null, new ObjectProxy<object>(new {name = "123"}).ID, "Should return null when no ID");
        }
            
        [Test]
        public void Returns_ID()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithID>(new ClassWithID(){ID = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Returns_ID_From_Subclass()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithID>(new SubclassWithID(){ID = 1}).ID, "Should return null when no ID");
        }


        [Test]
        public void Returns_Id()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithId>(new ClassWithId(){Id = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Returns_Id_From_Subclass()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithId>(new SubclassWithId(){Id = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Returns_id()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithid>(new ClassWithid(){id = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Returns_id_From_Subclass()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithid>(new SubclassWithid(){id = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_ClassID()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithClassID>(new ClassWithClassID(){ClassWithClassIDID = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_ClassID_From_Subclass()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithClassID>(new SubclassWithClassID(){ClassWithClassIDID = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_ClassId()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithClassId>(new ClassWithClassId(){ClassWithClassIdId = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_ClassId_From_Subclass()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithClassId>(new SubclassWithClassId(){ClassWithClassIdId = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_Classid()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithClassid>(new ClassWithClassid(){ClassWithClassidid = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_Classid_From_Subclass()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithClassid>(new SubclassWithClassid(){ClassWithClassidid = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_Attributed_Id()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithAttrID>(new ClassWithAttrID(){AttributedID = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_Attributed_Id_From_Subclass()
        {
            Assert.AreEqual(1, new ObjectProxy<ClassWithAttrID>(new SubclassWithAttrID(){AttributedID = 1}).ID, "Should return null when no ID");
        }
    }
}