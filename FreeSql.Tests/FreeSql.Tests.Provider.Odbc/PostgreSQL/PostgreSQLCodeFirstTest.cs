using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Xunit;

namespace FreeSql.Tests.Odbc.PostgreSQL
{
    public class PostgreSQLCodeFirstTest
    {
        [Fact]
        public void StringLength()
        {
            var dll = g.pgsql.CodeFirst.GetComparisonDDLStatements<TS_SLTB>();
            g.pgsql.CodeFirst.SyncStructure<TS_SLTB>();
        }
        class TS_SLTB
        {
            public Guid Id { get; set; }
            [Column(StringLength = 50)]
            public string Title { get; set; }

            [Column(IsNullable = false, StringLength = 50)]
            public string TitleSub { get; set; }
        }

        [Fact]
        public void ���ı�_�ֶ�()
        {
            var sql = g.pgsql.CodeFirst.GetComparisonDDLStatements<�������ı�>();
            g.pgsql.CodeFirst.SyncStructure<�������ı�>();

            var item = new �������ı�
            {
                ���� = "���Ա���",
                ����ʱ�� = DateTime.Now
            };
            Assert.Equal(1, g.pgsql.Insert<�������ı�>().AppendData(item).ExecuteAffrows());
            Assert.NotEqual(Guid.Empty, item.���);
            var item2 = g.pgsql.Select<�������ı�>().Where(a => a.��� == item.���).First();
            Assert.NotNull(item2);
            Assert.Equal(item.���, item2.���);
            Assert.Equal(item.����, item2.����);

            item.���� = "���Ա������";
            Assert.Equal(1, g.pgsql.Update<�������ı�>().SetSource(item).ExecuteAffrows());
            item2 = g.pgsql.Select<�������ı�>().Where(a => a.��� == item.���).First();
            Assert.NotNull(item2);
            Assert.Equal(item.���, item2.���);
            Assert.Equal(item.����, item2.����);

            item.���� = "���Ա������_repo";
            var repo = g.pgsql.GetRepository<�������ı�>();
            Assert.Equal(1, repo.Update(item));
            item2 = g.pgsql.Select<�������ı�>().Where(a => a.��� == item.���).First();
            Assert.NotNull(item2);
            Assert.Equal(item.���, item2.���);
            Assert.Equal(item.����, item2.����);

            item.���� = "���Ա������_repo22";
            Assert.Equal(1, repo.Update(item));
            item2 = g.pgsql.Select<�������ı�>().Where(a => a.��� == item.���).First();
            Assert.NotNull(item2);
            Assert.Equal(item.���, item2.���);
            Assert.Equal(item.����, item2.����);
        }
        class �������ı�
        {
            [Column(IsPrimary = true)]
            public Guid ��� { get; set; }

            public string ���� { get; set; }

            public DateTime ����ʱ�� { get; set; }
        }

        [Fact]
        public void AddUniques()
        {
            var sql = g.pgsql.CodeFirst.GetComparisonDDLStatements<AddUniquesInfo>();
            g.pgsql.CodeFirst.SyncStructure<AddUniquesInfo>();
            g.pgsql.CodeFirst.SyncStructure(typeof(AddUniquesInfo), "AddUniquesInfo1");
        }
        [Table(Name = "AddUniquesInfo", OldName = "AddUniquesInfo2")]
        [Index("{tablename}_uk_phone", "phone", true)]
        [Index("{tablename}_uk_group_index", "group,index", true)]
        [Index("{tablename}_uk_group_index22", "group, index22", true)]
        class AddUniquesInfo
        {
            public Guid id { get; set; }
            public string phone { get; set; }

            public string group { get; set; }
            public int index { get; set; }
            public string index22 { get; set; }
        }

        [Fact]
        public void AddField()
        {
            var sql = g.pgsql.CodeFirst.GetComparisonDDLStatements<TopicAddField>();
            Assert.True(string.IsNullOrEmpty(sql)); //�����������κ�
            g.pgsql.Select<TopicAddField>();
            var id = g.pgsql.Insert<TopicAddField>().AppendData(new TopicAddField { }).ExecuteIdentity();
        }

        [Table(Name = "ccc.TopicAddField", OldName = "TopicAddField")]
        public class TopicAddField
        {
            [Column(IsIdentity = true)]
            public int Id { get; set; }

            public string name { get; set; } = "xxx";

            public int clicks { get; set; } = 10;
            //public int name { get; set; } = 3000;

            //[Column(DbType = "varchar(200) not null", OldName = "title")]
            //public string title222 { get; set; } = "333";

            //[Column(DbType = "varchar(200) not null")]
            //public string title222333 { get; set; } = "xxx";

            //[Column(DbType = "varchar(100) not null", OldName = "title122333aaa")]
            //public string titleaaa { get; set; } = "fsdf";


            [Column(IsIgnore = true)]
            public DateTime ct { get; set; } = DateTime.Now;
        }

        [Fact]
        public void GetComparisonDDLStatements()
        {

            var sql = g.pgsql.CodeFirst.GetComparisonDDLStatements<TableAllType>();
            g.pgsql.Select<TableAllType>();
        }

        IInsert<TableAllType> insert => g.pgsql.Insert<TableAllType>();
        ISelect<TableAllType> select => g.pgsql.Select<TableAllType>();

        [Fact]
        public void CurdAllField()
        {
            var item = new TableAllType { };
            item.Id = (int)insert.AppendData(item).ExecuteIdentity();

            var newitem = select.Where(a => a.Id == item.Id).ToOne();

            var item2 = new TableAllType
            {
                testFieldBool = true,
                testFieldBoolNullable = true,
                testFieldByte = byte.MaxValue,
                testFieldByteNullable = byte.MinValue,
                testFieldBytes = Encoding.UTF8.GetBytes("�����й���"),
                testFieldDateTime = DateTime.Now,
                testFieldDateTimeNullable = DateTime.Now.AddDays(-1),
                testFieldDecimal = 999.99M,
                testFieldDecimalNullable = 111.11M,
                testFieldDouble = 888.88,
                testFieldDoubleNullable = 222.22,
                testFieldEnum1 = TableAllTypeEnumType1.e3,
                testFieldEnum1Nullable = TableAllTypeEnumType1.e2,
                testFieldEnum2 = TableAllTypeEnumType2.f2,
                testFieldEnum2Nullable = TableAllTypeEnumType2.f3,
                testFieldFloat = 777.77F,
                testFieldFloatNullable = 333.33F,
                testFieldGuid = Guid.NewGuid(),
                testFieldGuidNullable = Guid.NewGuid(),
                testFieldInt = int.MaxValue,
                testFieldIntNullable = 123,
                testFieldLong = long.MaxValue,
                
                testFieldSByte = sbyte.MaxValue,
                testFieldSByteNullable = sbyte.MinValue,
                testFieldShort = short.MaxValue,
                testFieldShortNullable = short.MinValue,
                testFieldString = "�����й���string'\\?!@#$%^&*()_+{}}{~?><<>",
                testFieldChar = 'X',
                testFieldTimeSpan = TimeSpan.FromDays(1),
                testFieldTimeSpanNullable = TimeSpan.FromSeconds(90),
                testFieldUInt = uint.MaxValue,
                testFieldUIntNullable = uint.MinValue,
                testFieldULong = ulong.MaxValue,
                testFieldULongNullable = ulong.MinValue,
                testFieldUShort = ushort.MaxValue,
                testFieldUShortNullable = ushort.MinValue,
                testFielLongNullable = long.MinValue,
            };

            var sqlPar = insert.AppendData(item2).ToSql();
            var sqlText = insert.AppendData(item2).NoneParameter().ToSql();
            var item3NP = insert.AppendData(item2).NoneParameter().ExecuteInserted();

            var item3 = insert.AppendData(item2).ExecuteInserted().First();
            var newitem2 = select.Where(a => a.Id == item3.Id).ToOne();
            Assert.Equal(item2.testFieldString, newitem2.testFieldString);
            Assert.Equal(item2.testFieldChar, newitem2.testFieldChar);

            item3 = insert.NoneParameter().AppendData(item2).ExecuteInserted().First();
            newitem2 = select.Where(a => a.Id == item3.Id).ToOne();
            Assert.Equal(item2.testFieldString, newitem2.testFieldString);
            Assert.Equal(item2.testFieldChar, newitem2.testFieldChar);

            var items = select.ToList();
            var itemstb = select.ToDataTable();
        }

        [Table(Name = "tb_alltype")]
        class TableAllType
        {
            [Column(IsIdentity = true, IsPrimary = true)]
            public int Id { get; set; }

            public bool testFieldBool { get; set; }
            public sbyte testFieldSByte { get; set; }
            public short testFieldShort { get; set; }
            public int testFieldInt { get; set; }
            public long testFieldLong { get; set; }
            public byte testFieldByte { get; set; }
            public ushort testFieldUShort { get; set; }
            public uint testFieldUInt { get; set; }
            public ulong testFieldULong { get; set; }
            public double testFieldDouble { get; set; }
            public float testFieldFloat { get; set; }
            public decimal testFieldDecimal { get; set; }
            public TimeSpan testFieldTimeSpan { get; set; }
            public DateTime testFieldDateTime { get; set; }
            public byte[] testFieldBytes { get; set; }
            public string testFieldString { get; set; }
            public char testFieldChar { get; set; }
            public Guid testFieldGuid { get; set; }

            public bool? testFieldBoolNullable { get; set; }
            public sbyte? testFieldSByteNullable { get; set; }
            public short? testFieldShortNullable { get; set; }
            public int? testFieldIntNullable { get; set; }
            public long? testFielLongNullable { get; set; }
            public byte? testFieldByteNullable { get; set; }
            public ushort? testFieldUShortNullable { get; set; }
            public uint? testFieldUIntNullable { get; set; }
            public ulong? testFieldULongNullable { get; set; }
            public double? testFieldDoubleNullable { get; set; }
            public float? testFieldFloatNullable { get; set; }
            public decimal? testFieldDecimalNullable { get; set; }
            public TimeSpan? testFieldTimeSpanNullable { get; set; }
            public DateTime? testFieldDateTimeNullable { get; set; }
            public Guid? testFieldGuidNullable { get; set; }

            public TableAllTypeEnumType1 testFieldEnum1 { get; set; }
            public TableAllTypeEnumType1? testFieldEnum1Nullable { get; set; }
            public TableAllTypeEnumType2 testFieldEnum2 { get; set; }
            public TableAllTypeEnumType2? testFieldEnum2Nullable { get; set; }
        }

        public enum TableAllTypeEnumType1 { e1, e2, e3, e5 }
        [Flags] public enum TableAllTypeEnumType2 { f1, f2, f3 }
    }
}
