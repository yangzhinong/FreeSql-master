using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace FreeSql.Tests.Extensions
{
    public class LambadaExpressionExtensionsTest
    {

        [Fact]
        public void And()
        {
            Expression<Func<testExpAddOr, bool>> where = a => a.id == Guid.Empty;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"id\" = '00000000-0000-0000-0000-000000000000' AND a.\"num\" > 0)", g.sqlite.Select<testExpAddOr>().Where(where.And(b => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"id\" = '00000000-0000-0000-0000-000000000000')", g.sqlite.Select<testExpAddOr>().Where(where.And(false, b => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"id\" = '00000000-0000-0000-0000-000000000000' AND a.\"num\" = 1 AND a.\"num\" = 2)", g.sqlite.Select<testExpAddOr>().Where(where.And(b => b.num == 1).And(b => b.num == 2)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"id\" = '00000000-0000-0000-0000-000000000000')", g.sqlite.Select<testExpAddOr>().Where(where.And(false, b => b.num == 1).And(false, c => c.num == 2)).ToSql().Replace("\r\n", ""));

            where = null;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"num\" > 0)", g.sqlite.Select<testExpAddOr>().Where(where.And(b => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a", g.sqlite.Select<testExpAddOr>().Where(where.And(false, b => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"num\" = 1 AND a.\"num\" = 2)", g.sqlite.Select<testExpAddOr>().Where(where.And(b => b.num == 1).And(b => b.num == 2)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a", g.sqlite.Select<testExpAddOr>().Where(where.And(false, b => b.num == 1).And(false, c => c.num == 2)).ToSql().Replace("\r\n", ""));
        }
        [Fact]
        public void Or()
        {
            Expression<Func<testExpAddOr, bool>> where = a => a.id == Guid.Empty;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE ((a.\"id\" = '00000000-0000-0000-0000-000000000000' OR a.\"num\" > 0))", g.sqlite.Select<testExpAddOr>().Where(where.Or(b => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"id\" = '00000000-0000-0000-0000-000000000000')", g.sqlite.Select<testExpAddOr>().Where(where.Or(false, b => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (((a.\"id\" = '00000000-0000-0000-0000-000000000000' OR a.\"num\" = 1) OR a.\"num\" = 2))", g.sqlite.Select<testExpAddOr>().Where(where.Or(b => b.num == 1).Or(b => b.num == 2)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"id\" = '00000000-0000-0000-0000-000000000000')", g.sqlite.Select<testExpAddOr>().Where(where.Or(false, b => b.num == 1).Or(false, c => c.num == 2)).ToSql().Replace("\r\n", ""));

            where = null;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"num\" > 0)", g.sqlite.Select<testExpAddOr>().Where(where.Or(b => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a", g.sqlite.Select<testExpAddOr>().Where(where.Or(false, b => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE ((a.\"num\" = 1 OR a.\"num\" = 2))", g.sqlite.Select<testExpAddOr>().Where(where.Or(b => b.num == 1).Or(b => b.num == 2)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a", g.sqlite.Select<testExpAddOr>().Where(where.Or(false, b => b.num == 1).Or(false, c => c.num == 2)).ToSql().Replace("\r\n", ""));
        }
        [Fact]
        public void Not()
        {
            Expression<Func<testExpAddOr, bool>> where = a => a.id == Guid.Empty;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (not(a.\"id\" = '00000000-0000-0000-0000-000000000000'))", g.sqlite.Select<testExpAddOr>().Where(where.Not()).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a WHERE (a.\"id\" = '00000000-0000-0000-0000-000000000000')", g.sqlite.Select<testExpAddOr>().Where(where.Not(false)).ToSql().Replace("\r\n", ""));
        }

        class testExpAddOr
        {
            public Guid id { get; set; }

            public int num { get; set; }
        }
        class testExpAddOr2
        {
            public Guid id { get; set; }

            public int num { get; set; }
        }

        [Fact]
        public void And2()
        {
            Expression<Func<testExpAddOr, testExpAddOr2, bool>> where = (a, b) => a.id == b.id;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (a.\"id\" = b.\"id\" AND b.\"num\" > 0)", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.And((a, b) => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (a.\"id\" = b.\"id\")", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.And(false, (a, b) => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (a.\"id\" = b.\"id\" AND b.\"num\" = 1 AND b.\"num\" = 2)", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.And((a, b) => b.num == 1).And((a, b) => b.num == 2)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (a.\"id\" = b.\"id\")", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.And(false, (a, b) => b.num == 1).And(false, (a, c) => c.num == 2)).ToSql().Replace("\r\n", ""));

            where = null;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (b.\"num\" > 0)", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.And((a, b) => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.And(false, (a, b) => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (b.\"num\" = 1 AND b.\"num\" = 2)", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.And((a, b) => b.num == 1).And((a, b) => b.num == 2)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.And(false, (a, b) => b.num == 1).And(false, (a, c) => c.num == 2)).ToSql().Replace("\r\n", ""));
        }
        [Fact]
        public void Or2()
        {
            Expression<Func<testExpAddOr, testExpAddOr2, bool>> where = (a, b) => a.id == b.id;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE ((a.\"id\" = b.\"id\" OR b.\"num\" > 0))", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Or((a, b) => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (a.\"id\" = b.\"id\")", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Or(false, (a, b) => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (((a.\"id\" = b.\"id\" OR b.\"num\" = 1) OR b.\"num\" = 2))", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Or((a, b) => b.num == 1).Or((a, b) => b.num == 2)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (a.\"id\" = b.\"id\")", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Or(false, (a, b) => b.num == 1).Or(false, (a, c) => c.num == 2)).ToSql().Replace("\r\n", ""));

            where = null;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (b.\"num\" > 0)", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Or((a, b) => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Or(false, (a, b) => b.num > 0)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE ((b.\"num\" = 1 OR b.\"num\" = 2))", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Or((a, b) => b.num == 1).Or((a, b) => b.num == 2)).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Or(false, (a, b) => b.num == 1).Or(false, (a, c) => c.num == 2)).ToSql().Replace("\r\n", ""));
        }
        [Fact]
        public void Not2()
        {
            Expression<Func<testExpAddOr, testExpAddOr2, bool>> where = (a, b) => a.id == b.id;
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (not(a.\"id\" = b.\"id\"))", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Not()).ToSql().Replace("\r\n", ""));
            Assert.Equal("SELECT a.\"id\", a.\"num\" FROM \"testExpAddOr\" a, \"testExpAddOr2\" b WHERE (a.\"id\" = b.\"id\")", g.sqlite.Select<testExpAddOr, testExpAddOr2>().Where(where.Not(false)).ToSql().Replace("\r\n", ""));
        }
    }
}
