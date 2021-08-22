using FreeSql.DataAnnotations;
using System;
using System.Linq;
using Xunit;

namespace FreeSql.Tests.Linq
{
    class TestLinqToSql
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public int click { get; set; } = 10;

        public DateTime createtime { get; set; } = DateTime.Now;
    }
    class TestLinqToSqlComment
    {
        public Guid id { get; set; }

        public Guid TestLinqToSqlId { get; set; }
        public TestLinqToSql TEstLinqToSql { get; set; }

        public string text { get; set; }

        public DateTime createtime { get; set; } = DateTime.Now;
    }

    public class ISelectLinqToSqlTests
    {

        [Fact]
        public void Where()
        {
            var item = new TestLinqToSql { name = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSql>().AppendData(item).ExecuteAffrows();

            var t1 = (from a in g.sqlite.Select<TestLinqToSql>()
                      where a.id == item.id
                      select a).ToList();
            Assert.True(t1.Any());
            Assert.Equal(item.id, t1[0].id);
        }

        [Fact]
        public void OrderBy()
        {
            var item = new TestLinqToSql { name = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSql>().AppendData(item).ExecuteAffrows();

            var t1 = (from a in g.sqlite.Select<TestLinqToSql>()
                      where a.id == item.id
                      orderby a.id
                      select a).ToList();
            Assert.True(t1.Any());
            Assert.Equal(item.id, t1[0].id);

            Assert.Equal((from a in g.sqlite.Select<TestLinqToSql>()
                          where a.id == item.id
                          orderby a.id
                          select a).ToSql(),
                          (from a in g.sqlite.Select<TestLinqToSql>()
                           where a.id == item.id
                           orderby a.id ascending
                           select a).ToSql());

            Assert.Equal((from a in g.sqlite.Select<TestLinqToSql>()
                          where a.id == item.id
                          orderby a.id ascending
                          select a).ToSql(),
                          g.sqlite.Select<TestLinqToSql>().Where(a => a.id == item.id).OrderBy(a => a.id).ToSql());

            Assert.Equal((from a in g.sqlite.Select<TestLinqToSql>()
                          where a.id == item.id
                          orderby a.id descending
                          select a).ToSql(),
                          g.sqlite.Select<TestLinqToSql>().Where(a => a.id == item.id).OrderByDescending(a => a.id).ToSql());


            Assert.Equal((from a in g.sqlite.Select<TestLinqToSql>()
                          where a.id == item.id
                          orderby a.id, a.createtime ascending, a.name descending
                          select a).ToSql(),
                          (from a in g.sqlite.Select<TestLinqToSql>()
                           where a.id == item.id
                           orderby a.id, a.createtime, a.name descending
                           select a).ToSql());

            Assert.Equal((from a in g.sqlite.Select<TestLinqToSql>()
                          where a.id == item.id
                          orderby a.id ascending, a.createtime ascending, a.name descending
                          select a).ToSql(),
                          g.sqlite.Select<TestLinqToSql>().Where(a => a.id == item.id).OrderBy(a => a.id).OrderBy(a => a.createtime).OrderByDescending(a => a.name).ToSql());

            Assert.Equal((from a in g.sqlite.Select<TestLinqToSql>()
                          where a.id == item.id
                          orderby a.id descending, a.createtime ascending, a.name descending
                          select a).ToSql(),
                          g.sqlite.Select<TestLinqToSql>().Where(a => a.id == item.id).OrderByDescending(a => a.id).OrderBy(a => a.createtime).OrderByDescending(a => a.name).ToSql());
        }

        [Fact]
        public void Select()
        {
            var item = new TestLinqToSql { name = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSql>().AppendData(item).ExecuteAffrows();

            var t1 = (from a in g.sqlite.Select<TestLinqToSql>()
                      where a.id == item.id
                      select new { a.id }).ToList();
            Assert.True(t1.Any());
            Assert.Equal(item.id, t1[0].id);
        }

        [Fact]
        public void GroupBy()
        {
            var item = new TestLinqToSql { name = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSql>().AppendData(item).ExecuteAffrows();

            var t1 = (from a in g.sqlite.Select<TestLinqToSql>()
                      where a.id == item.id
                      group a by new { a.id, a.name } into g
                      select new
                      {
                          g.Key.id,
                          g.Key.name,
                          cou = g.Count(),
                          avg = g.Avg(g.Value.click),
                          sum = g.Sum(g.Value.click),
                          max = g.Max(g.Value.click),
                          min = g.Min(g.Value.click)
                      }).ToList();
            Assert.True(t1.Any());
            Assert.Equal(item.id, t1.First().id);
        }

        [Fact]
        public void CaseWhen()
        {
            var item = new TestLinqToSql { name = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSql>().AppendData(item).ExecuteAffrows();

            var t1 = (from a in g.sqlite.Select<TestLinqToSql>()
                      where a.id == item.id
                      select new
                      {
                          a.id,
                          a.name,
                          testsub = new
                          {
                              time = a.click > 10 ? "����" : "С�ڻ����"
                          }
                      }).ToList();
            Assert.True(t1.Any());
            Assert.Equal(item.id, t1[0].id);
            Assert.Equal("С�ڻ����", t1[0].testsub.time);
        }

        [Fact]
        public void Join()
        {
            var item = new TestLinqToSql { name = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSql>().AppendData(item).ExecuteAffrows();
            var comment = new TestLinqToSqlComment { TestLinqToSqlId = item.id, text = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSqlComment>().AppendData(comment).ExecuteAffrows();

            var t1 = (from a in g.sqlite.Select<TestLinqToSql>()
                      join b in g.sqlite.Select<TestLinqToSqlComment>() on a.id equals b.TestLinqToSqlId
                      orderby b.id descending
                      select a).ToList();
            Assert.True(t1.Any());
            //Assert.Equal(item.id, t1[0].id);

            var t2 = (from a in g.sqlite.Select<TestLinqToSql>()
                      join b in g.sqlite.Select<TestLinqToSqlComment>() on a.id equals b.TestLinqToSqlId
                      select new { a.id, bid = b.id }).ToList();
            Assert.True(t2.Any());
            //Assert.Equal(item.id, t2[0].id);
            //Assert.Equal(comment.id, t2[0].bid);

            var t3 = (from a in g.sqlite.Select<TestLinqToSql>()
                      join b in g.sqlite.Select<TestLinqToSqlComment>() on a.id equals b.TestLinqToSqlId
                      where a.id == item.id
                      select new { a.id, bid = b.id }).ToList();
            Assert.True(t3.Any());
            Assert.Equal(item.id, t3[0].id);
            Assert.Equal(comment.id, t3[0].bid);
        }

        [Fact]
        public void LeftJoin()
        {
            var item = new TestLinqToSql { name = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSql>().AppendData(item).ExecuteAffrows();
            var comment = new TestLinqToSqlComment { TestLinqToSqlId = item.id, text = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSqlComment>().AppendData(comment).ExecuteAffrows();

            var t1 = (from a in g.sqlite.Select<TestLinqToSql>()
                      join b in g.sqlite.Select<TestLinqToSqlComment>() on a.id equals b.TestLinqToSqlId into temp
                      from tc in temp.DefaultIfEmpty()
                      select a).ToList();
            Assert.True(t1.Any());
            //Assert.Equal(item.id, t1[0].id);

            var t2 = (from a in g.sqlite.Select<TestLinqToSql>()
                      join b in g.sqlite.Select<TestLinqToSqlComment>() on a.id equals b.TestLinqToSqlId into temp
                      from tc in temp.DefaultIfEmpty()
                      select new { a.id, bid = tc.id }).ToList();
            Assert.True(t2.Any());
            //Assert.Equal(item.id, t2[0].id);
            //Assert.Equal(comment.id, t2[0].bid);

            var t3 = (from a in g.sqlite.Select<TestLinqToSql>()
                      join b in g.sqlite.Select<TestLinqToSqlComment>() on a.id equals b.TestLinqToSqlId into temp
                      from tc in temp.DefaultIfEmpty()
                      where a.id == item.id
                      select new { a.id, bid = tc.id }).ToList();
            Assert.True(t3.Any());
            Assert.Equal(item.id, t3[0].id);
            Assert.Equal(comment.id, t3[0].bid);
        }

        [Fact]
        public void From()
        {
            var item = new TestLinqToSql { name = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSql>().AppendData(item).ExecuteAffrows();
            var comment = new TestLinqToSqlComment { TestLinqToSqlId = item.id, text = Guid.NewGuid().ToString() };
            g.sqlite.Insert<TestLinqToSqlComment>().AppendData(comment).ExecuteAffrows();

            var t1 = (from a in g.sqlite.Select<TestLinqToSql>()
                      from b in g.sqlite.Select<TestLinqToSqlComment>()
                      where a.id == b.TestLinqToSqlId
                      select a).ToList();
            Assert.True(t1.Any());
            //Assert.Equal(item.id, t1[0].id);

            var t2 = (from a in g.sqlite.Select<TestLinqToSql>()
                      from b in g.sqlite.Select<TestLinqToSqlComment>()
                      where a.id == b.TestLinqToSqlId
                      select new { a.id, bid = b.id }).ToList();
            Assert.True(t2.Any());
            //Assert.Equal(item.id, t2[0].id);
            //Assert.Equal(comment.id, t2[0].bid);

            var t3 = (from a in g.sqlite.Select<TestLinqToSql>()
                      from b in g.sqlite.Select<TestLinqToSqlComment>()
                      where a.id == b.TestLinqToSqlId
                      where a.id == item.id
                      select new { a.id, bid = b.id }).ToList();
            Assert.True(t3.Any());
            Assert.Equal(item.id, t3[0].id);
            Assert.Equal(comment.id, t3[0].bid);
        }
    }
}
