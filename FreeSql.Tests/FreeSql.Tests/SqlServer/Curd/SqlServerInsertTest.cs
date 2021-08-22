using FreeSql.DataAnnotations;
using FreeSql.Tests.DataContext.SqlServer;
using SaleIDO.Entity.Storeage;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FreeSql.Tests.SqlServer
{
    [Collection("SqlServerCollection")]
    public class SqlServerInsertTest
    {
        SqlServerFixture _sqlserverFixture;

        public SqlServerInsertTest(SqlServerFixture sqlserverFixture)
        {
            _sqlserverFixture = sqlserverFixture;
        }

        IInsert<Topic> insert => g.sqlserver.Insert<Topic>(); //��������

        [Table(Name = "tb_topic")]
        class Topic
        {
            [Column(IsIdentity = true, IsPrimary = true)]
            public int Id { get; set; }
            public int Clicks { get; set; }
            public int TypeGuid { get; set; }
            public TestTypeInfo Type { get; set; }
            public string Title { get; set; }
            public DateTime CreateTime { get; set; }
        }

        [Fact]
        public void AppendData()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });

            var sql = insert.IgnoreColumns(a => a.TypeGuid).AppendData(items.First()).ToSql();
            Assert.Equal("INSERT INTO [tb_topic]([Clicks], [Title], [CreateTime]) VALUES(@Clicks_0, @Title_0, @CreateTime_0)", sql);

            sql = insert.IgnoreColumns(a => a.TypeGuid).AppendData(items).ToSql();
            Assert.Equal("INSERT INTO [tb_topic]([Clicks], [Title], [CreateTime]) VALUES(@Clicks_0, @Title_0, @CreateTime_0), (@Clicks_1, @Title_1, @CreateTime_1), (@Clicks_2, @Title_2, @CreateTime_2), (@Clicks_3, @Title_3, @CreateTime_3), (@Clicks_4, @Title_4, @CreateTime_4), (@Clicks_5, @Title_5, @CreateTime_5), (@Clicks_6, @Title_6, @CreateTime_6), (@Clicks_7, @Title_7, @CreateTime_7), (@Clicks_8, @Title_8, @CreateTime_8), (@Clicks_9, @Title_9, @CreateTime_9)", sql);

            sql = insert.AppendData(items).InsertColumns(a => a.Title).ToSql();
            Assert.Equal("INSERT INTO [tb_topic]([Title]) VALUES(@Title_0), (@Title_1), (@Title_2), (@Title_3), (@Title_4), (@Title_5), (@Title_6), (@Title_7), (@Title_8), (@Title_9)", sql);

            sql = insert.IgnoreColumns(a => new { a.CreateTime, a.TypeGuid }).AppendData(items).ToSql();
            Assert.Equal("INSERT INTO [tb_topic]([Clicks], [Title]) VALUES(@Clicks_0, @Title_0), (@Clicks_1, @Title_1), (@Clicks_2, @Title_2), (@Clicks_3, @Title_3), (@Clicks_4, @Title_4), (@Clicks_5, @Title_5), (@Clicks_6, @Title_6), (@Clicks_7, @Title_7), (@Clicks_8, @Title_8), (@Clicks_9, @Title_9)", sql);
        }

        [Fact]
        public void InsertColumns()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });

            var sql = insert.AppendData(items).InsertColumns(a => a.Title).ToSql();
            Assert.Equal("INSERT INTO [tb_topic]([Title]) VALUES(@Title_0), (@Title_1), (@Title_2), (@Title_3), (@Title_4), (@Title_5), (@Title_6), (@Title_7), (@Title_8), (@Title_9)", sql);

            sql = insert.AppendData(items).InsertColumns(a => new { a.Title, a.Clicks }).ToSql();
            Assert.Equal("INSERT INTO [tb_topic]([Clicks], [Title]) VALUES(@Clicks_0, @Title_0), (@Clicks_1, @Title_1), (@Clicks_2, @Title_2), (@Clicks_3, @Title_3), (@Clicks_4, @Title_4), (@Clicks_5, @Title_5), (@Clicks_6, @Title_6), (@Clicks_7, @Title_7), (@Clicks_8, @Title_8), (@Clicks_9, @Title_9)", sql);
        }
        [Fact]
        public void IgnoreColumns()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });

            var sql = insert.AppendData(items).IgnoreColumns(a => new { a.CreateTime, a.TypeGuid }).ToSql();
            Assert.Equal("INSERT INTO [tb_topic]([Clicks], [Title]) VALUES(@Clicks_0, @Title_0), (@Clicks_1, @Title_1), (@Clicks_2, @Title_2), (@Clicks_3, @Title_3), (@Clicks_4, @Title_4), (@Clicks_5, @Title_5), (@Clicks_6, @Title_6), (@Clicks_7, @Title_7), (@Clicks_8, @Title_8), (@Clicks_9, @Title_9)", sql);

            sql = insert.AppendData(items).IgnoreColumns(a => new { a.Title, a.CreateTime, a.TypeGuid }).ToSql();
            Assert.Equal("INSERT INTO [tb_topic]([Clicks]) VALUES(@Clicks_0), (@Clicks_1), (@Clicks_2), (@Clicks_3), (@Clicks_4), (@Clicks_5), (@Clicks_6), (@Clicks_7), (@Clicks_8), (@Clicks_9)", sql);

            g.sqlserver.Delete<TopicIgnore>().Where("1=1").ExecuteAffrows();
            var itemsIgnore = new List<TopicIgnore>();
            for (var a = 0; a < 2072; a++) itemsIgnore.Add(new TopicIgnore { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });
            g.sqlserver.Insert<TopicIgnore>().AppendData(itemsIgnore).IgnoreColumns(a => new { a.Title }).ExecuteAffrows();
            Assert.Equal(2072, itemsIgnore.Count);
            Assert.Equal(2072, g.sqlserver.Select<TopicIgnore>().Where(a => a.Title == null).Count());
        }
        [Table(Name = "tb_topicIgnoreColumns")]
        class TopicIgnore
        {
            [Column(IsIdentity = true, IsPrimary = true)]
            public int Id { get; set; }
            public int Clicks { get; set; }
            public string Title { get; set; }
            public DateTime CreateTime { get; set; }
        }
        [Fact]
        public void ExecuteAffrows()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });

            Assert.Equal(1, insert.AppendData(items.First()).ExecuteAffrows());
            Assert.Equal(10, insert.AppendData(items).NoneParameter().ExecuteAffrows());

            Assert.Equal(10, g.sqlserver.Select<Topic>().Limit(10).InsertInto(null, a => new Topic
            {
                Title = a.Title
            }));

            //items = Enumerable.Range(0, 9989).Select(a => new Topic { Title = "newtitle" + a, CreateTime = DateTime.Now }).ToList();
            //Assert.Equal(9989, g.sqlserver.Insert<Topic>(items).ExecuteAffrows());

            //var bttype = new TestBatchInsertType { title = "testbttitle1" };
            //bttype.id = (int)g.sqlserver.Insert(bttype).ExecuteIdentity();
            //Assert.True(bttype.id > 0);
            //var bttopic = Enumerable.Range(0, 10000).Select(a => new TestBatchInsertTopic { TypeId = bttype.id, Text = $"testtopic{a}" }).ToArray();
            //Assert.Equal(bttopic.Length, g.sqlserver.Insert<TestBatchInsertTopic>(bttopic).ExecuteAffrows());

            //g.sqlserver.Transaction(() =>
            //{
            //    bttype = new TestBatchInsertType { title = "transaction_testbttitle2" };
            //    bttype.id = (int)g.sqlserver.Insert(bttype).ExecuteIdentity();
            //    Assert.True(bttype.id > 0);
            //    bttopic = Enumerable.Range(0, 10000).Select(a => new TestBatchInsertTopic { TypeId = bttype.id, Text = $"transaction_testtopic{a}" }).ToArray();
            //    Assert.Equal(bttopic.Length, g.sqlserver.Insert<TestBatchInsertTopic>(bttopic).ExecuteAffrows());
            //});

            g.sqlserver.Transaction(() =>
            {
                var order = new AdjustPriceOrder {  };
                order.Id = (int)g.sqlserver.Insert(order).NoneParameter().ExecuteIdentity();
                Assert.True(order.Id > 0);
                var detail = Enumerable.Range(0, 10000).Select(a => new AdjustPriceDetail {   Remark = $"transaction_testdetail{a}" }).ToArray();
                Assert.Equal(detail.Length, g.sqlserver.Insert<AdjustPriceDetail>(detail).NoneParameter().ExecuteAffrows());
            });
        }
        class TestBatchInsertType { 
            [Column(IsIdentity = true)]
            public int id { get; set; }
            public string title { get; set; }
        }
        class TestBatchInsertTopic
        {
            public Guid id { get; set; }
            public int TypeId { get; set; }
            public string Text { get; set; }
        }
        [Fact]
        public void ExecuteIdentity()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });

            Assert.NotEqual(0, insert.AppendData(items.First()).ExecuteIdentity());


            //items = Enumerable.Range(0, 9999).Select(a => new Topic { Title = "newtitle" + a, CreateTime = DateTime.Now }).ToList();
            //var lastId = g.sqlite.Select<Topic>().Max(a => a.Id);
            //Assert.NotEqual(lastId, g.sqlserver.Insert<Topic>(items).ExecuteIdentity());
        }
        [Fact]
        public void ExecuteInserted()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });

            var items2 = insert.AppendData(items).ExecuteInserted();

            items = Enumerable.Range(0, 90).Select(a => new Topic { Title = "newtitle" + a, CreateTime = DateTime.Now }).ToList();
            var itemsInserted = g.sqlserver.Insert<Topic>(items).ExecuteInserted();
            Assert.Equal(items.First().Title, itemsInserted.First().Title);
            Assert.Equal(items.Last().Title, itemsInserted.Last().Title);
        }
        [Fact]
        public void ExecuteSqlBulkCopy()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });

            insert.AppendData(items).InsertIdentity().ExecuteSqlBulkCopy();
            //insert.AppendData(items).IgnoreColumns(a => new { a.CreateTime, a.Clicks }).ExecuteSqlBulkCopy();
            // System.NotSupportedException:“DataSet does not support System.Nullable<>.”
        }

        [Fact]
        public void AsTable()
        {
            var items = new List<Topic>();
            for (var a = 0; a < 10; a++) items.Add(new Topic { Id = a + 1, Title = $"newtitle{a}", Clicks = a * 100, CreateTime = DateTime.Now });

            var sql = insert.IgnoreColumns(a => a.TypeGuid).AppendData(items.First()).AsTable(a => "tb_topicAsTable").ToSql();
            Assert.Equal("INSERT INTO [tb_topicAsTable]([Clicks], [Title], [CreateTime]) VALUES(@Clicks_0, @Title_0, @CreateTime_0)", sql);

            sql = insert.IgnoreColumns(a => a.TypeGuid).AppendData(items).AsTable(a => "tb_topicAsTable").ToSql();
            Assert.Equal("INSERT INTO [tb_topicAsTable]([Clicks], [Title], [CreateTime]) VALUES(@Clicks_0, @Title_0, @CreateTime_0), (@Clicks_1, @Title_1, @CreateTime_1), (@Clicks_2, @Title_2, @CreateTime_2), (@Clicks_3, @Title_3, @CreateTime_3), (@Clicks_4, @Title_4, @CreateTime_4), (@Clicks_5, @Title_5, @CreateTime_5), (@Clicks_6, @Title_6, @CreateTime_6), (@Clicks_7, @Title_7, @CreateTime_7), (@Clicks_8, @Title_8, @CreateTime_8), (@Clicks_9, @Title_9, @CreateTime_9)", sql);

            sql = insert.IgnoreColumns(a => a.TypeGuid).AppendData(items).InsertColumns(a => a.Title).AsTable(a => "tb_topicAsTable").ToSql();
            Assert.Equal("INSERT INTO [tb_topicAsTable]([Title]) VALUES(@Title_0), (@Title_1), (@Title_2), (@Title_3), (@Title_4), (@Title_5), (@Title_6), (@Title_7), (@Title_8), (@Title_9)", sql);

            sql = insert.IgnoreColumns(a => new { a.CreateTime, a.TypeGuid }).AppendData(items).AsTable(a => "tb_topicAsTable").ToSql();
            Assert.Equal("INSERT INTO [tb_topicAsTable]([Clicks], [Title]) VALUES(@Clicks_0, @Title_0), (@Clicks_1, @Title_1), (@Clicks_2, @Title_2), (@Clicks_3, @Title_3), (@Clicks_4, @Title_4), (@Clicks_5, @Title_5), (@Clicks_6, @Title_6), (@Clicks_7, @Title_7), (@Clicks_8, @Title_8), (@Clicks_9, @Title_9)", sql);

            sql = insert.IgnoreColumns(a => new { a.Title, a.TypeGuid }).InsertColumns(a => a.Title).AppendData(items).AsTable(a => "tb_topicAsTable").ToSql();
            Assert.Equal("INSERT INTO [tb_topicAsTable]([Title]) VALUES(@Title_0), (@Title_1), (@Title_2), (@Title_3), (@Title_4), (@Title_5), (@Title_6), (@Title_7), (@Title_8), (@Title_9)", sql);

            sql = insert.IgnoreColumns(a => a.TypeGuid).AppendData(items).InsertColumns(a => new { a.Title, a.Clicks }).AsTable(a => "tb_topicAsTable").ToSql();
            Assert.Equal("INSERT INTO [tb_topicAsTable]([Clicks], [Title]) VALUES(@Clicks_0, @Title_0), (@Clicks_1, @Title_1), (@Clicks_2, @Title_2), (@Clicks_3, @Title_3), (@Clicks_4, @Title_4), (@Clicks_5, @Title_5), (@Clicks_6, @Title_6), (@Clicks_7, @Title_7), (@Clicks_8, @Title_8), (@Clicks_9, @Title_9)", sql);

            sql = insert.IgnoreColumns(a => new { a.CreateTime, a.TypeGuid }).AppendData(items).AsTable(a => "tb_topicAsTable").ToSql();
            Assert.Equal("INSERT INTO [tb_topicAsTable]([Clicks], [Title]) VALUES(@Clicks_0, @Title_0), (@Clicks_1, @Title_1), (@Clicks_2, @Title_2), (@Clicks_3, @Title_3), (@Clicks_4, @Title_4), (@Clicks_5, @Title_5), (@Clicks_6, @Title_6), (@Clicks_7, @Title_7), (@Clicks_8, @Title_8), (@Clicks_9, @Title_9)", sql);

            sql = insert.IgnoreColumns(a => new { a.CreateTime, a.Title, a.TypeGuid }).AppendData(items).AsTable(a => "tb_topicAsTable").ToSql();
            Assert.Equal("INSERT INTO [tb_topicAsTable]([Clicks]) VALUES(@Clicks_0), (@Clicks_1), (@Clicks_2), (@Clicks_3), (@Clicks_4), (@Clicks_5), (@Clicks_6), (@Clicks_7), (@Clicks_8), (@Clicks_9)", sql);
        }
    }
}
