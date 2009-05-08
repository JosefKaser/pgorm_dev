using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Npgsql;
using System.Diagnostics;
using System.IO;
using System.Reflection;
namespace PGORM.Tests
{
#if ORM_GENERATED

using PGORM_TEST.Core;
using PGORM_TEST.PUBLIC.Entities;
using PGORM_TEST.PUBLIC.Factory;
using PGORM_TEST.PUBLIC.RecordSet;

    [TestFixture]
    public class PGORM_TEST
    {
        NpgsqlConnection Connection;

    #region InitializeDatabase
        [TestFixtureSetUp]
        public void InitializeDatabase()
        {
            DataAccess.InitializeDatabase("server=localhost;database=PGORM_TEST;username=postgres;password=postgres", true);
            Connection = DataAccess.Connection;
        }
        #endregion

    #region Test0001_ConnectionTest
        [Test]
        public void Test0001_ConnectionTest()
        {
            Assert.NotNull(Connection);
        } 
        #endregion

    #region Test0002_InsertTest
        [Test]
        public void Test0002_InsertTest()
        {
            test1Object obj = new test1Object();
            obj.field1 = "Hello";
            test1_ObjectFactory.Insert(ref obj);
            Assert.AreEqual((int)obj.id, 1);
        } 
        #endregion

    #region Test0003_GetSingleTest
        [Test]
        public void Test0003_GetSingleTest()
        {
            test2Object obj = test2_ObjectFactory.GetBy_id(1);
            Assert.True(obj.field1 == "TEST OK");
        } 
        #endregion

    #region Test0004_DeleteSingle
        [Test]
        public void Test0004_DeleteSingle()
        {
            Assert.True(test2_ObjectFactory.DeleteBy_id(1) == 1);
        } 
        #endregion

    #region Test0005_DeleteAll
        [Test]
        public void Test0005_DeleteAll()
        {
            test2_ObjectFactory.DeleteAll();

            for (int a = 0; a != 10; a++)
            {
                test2Object obj = new test2Object();
                obj.field1 = a.ToString();
                test2_ObjectFactory.Insert(ref obj);
            }
            Assert.True(test2_ObjectFactory.DeleteAll() == 10);
        } 
        #endregion

    #region Test0006_GetList
        [Test]
        public void Test0006_GetList()
        {
            List<test3Object> r = test3_ObjectFactory.GetList();
            Assert.IsTrue(r.Count() == 3);
        } 
        #endregion

    #region Test0007_GetMany
        [Test]
        public void Test0007_GetMany()
        {
            List<test3Object> r = test3_ObjectFactory.GetManyBy_field2(2);
            Assert.IsTrue(r.Count() == 2);
        } 
        #endregion

    #region Test0008_CountRecords
        [Test]
        public void Test0008_CountRecords()
        {
            Assert.IsTrue(test3_ObjectFactory.CountRecords() == 3);
        } 
        #endregion

    #region Test0009_DeleteSingle
        [Test]
        public void Test0009_DeleteSingle()
        {
            test3_ObjectFactory.DeleteBy_id(2);
            List<test3Object> r = test3_ObjectFactory.GetList();
            Assert.IsTrue(r.Count() == 2);
            Assert.IsTrue(r[0].field1 == "TEST1");
            Assert.IsTrue(r[1].field1 == "TEST3");
        } 
        #endregion

    #region Test0010_UpdateSingle
        [Test]
        public void Test0010_UpdateSingle()
        {
            test3Object obj = test3_ObjectFactory.GetBy_id(1);
            obj.field1 = "UPDATED";
            obj.field2 = 9;
            test3_ObjectFactory.UpdateBy_id(ref obj, 1);

            test3Object obj2 = test3_ObjectFactory.GetBy_id(1);
            Assert.IsTrue(obj2.field1 == "UPDATED");
            Assert.IsTrue((int)obj2.field2 == 9);
        } 
        #endregion

    #region Test0011_CopyIn
        [Test]
        public void Test0011_CopyIn()
        {
            test2_ObjectFactory.DeleteAll();
            List<test2Object> import = new List<test2Object>();
            for (int a = 0; a != 5; a++)
            {
                test2Object obj = new test2Object();
                obj.id = 100 + a;
                obj.field1 = Guid.NewGuid().ToString();
                import.Add(obj);
            }
            test2_ObjectFactory.CopyIn(import);

            List<test2Object> r = test2_ObjectFactory.GetList();
            for (int a = 0; a != r.Count(); a++)
            {
                Assert.IsTrue((int)r[a].id == (100 + a));
            }
        } 
        #endregion

    #region Test0012_INParameters
        [Test]
        public void Test0012_INParameters()
        {
            test2_ObjectFactory.DeleteAll();
            List<test2Object> import = new List<test2Object>();
            for (int a = 0; a != 10; a++)
            {
                test2Object obj = new test2Object();
                obj.id = 100 + a;
                obj.field1 = Guid.NewGuid().ToString();
                import.Add(obj);
            }
            test2_ObjectFactory.CopyIn(import);

            string sql = "select * from test2 where id in @values";
            List<int> ids = new List<int>();
            ids.AddRange(new int[] { 100, 102, 104 });
            List<test2Object> r = DataAccess.ExecuteObjectQuery<test2Object>(
                sql,
                test2_ObjectFactory.CreateFromReader<test2Object>,
                null,
                DataAccess.NewINParameter("@values", ids));
            Assert.IsTrue(r.Count() == 3);
            Assert.IsTrue((int)r[0].id == 100);
            Assert.IsTrue((int)r[1].id == 102);
            Assert.IsTrue((int)r[2].id == 104);
        } 
        #endregion
    }
#endif
}
