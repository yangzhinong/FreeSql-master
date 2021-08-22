using FreeSql.DataAnnotations;
using FreeSql;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FreeSql.Tests
{
    public class UnitTest1
    {

        class testenumWhere
        {
            public Guid id { get; set; }
            public testenumWhereType type { get; set; }
        }
        public enum testenumWhereType { Menu, Class, Blaaa }

        [Fact]
        public void Include_ManyToMany()
        {
            g.sqlite.Delete<userinfo>().Where("1=1").ExecuteAffrows();
            g.sqlite.Delete<DEPARTMENTS>().Where("1=1").ExecuteAffrows();
            g.sqlite.Delete<dept_user>().Where("1=1").ExecuteAffrows();
            BaseEntity.Initialization(g.sqlite, null);

            userinfo user = new userinfo { userid = 1, badgenumber = "", Name="", IDCardNo="" };
            user.Insert();

            user.depts = new List<DEPARTMENTS>(
                new[] {
                    new DEPARTMENTS { deptid = 1, deptcode = "01", deptname = "" },
                    new DEPARTMENTS { deptid = 2, deptcode = "02", deptname = "" },
                    new DEPARTMENTS { deptid = 3, deptcode = "03" , deptname = ""},
                });
            user.SaveMany("depts");

            user.depts = new List<DEPARTMENTS>(
                new[] {
                    new DEPARTMENTS { deptid = 1, deptcode = "01", deptname = "" },
                    new DEPARTMENTS { deptid = 2, deptcode = "02", deptname = "" },
                    new DEPARTMENTS { deptid = 4, deptcode = "04", deptname = "" },
                });
            user.SaveMany("depts");

            user.depts = new List<DEPARTMENTS>(
                new[] {
                    new DEPARTMENTS { deptid = 2, deptcode = "02", deptname = "" },
                });
            user.SaveMany("depts");

            g.sqlite.CodeFirst.SyncStructure<Song_tag>();
            g.sqlite.CodeFirst.SyncStructure<Tag>();
            g.sqlite.CodeFirst.SyncStructure<Song>();

            var test150_01 = g.sqlite.GetRepository<Tag>()
                    .Select.From<Tag>((s, b) => s.InnerJoin(a => a.Id == b.Id))
                    .ToList((a, b) => new
                    {
                        a.Id,
                        a.Name,
                        id2 = b.Id,
                        name2 = b.Name
                    });


            using (var ctx = g.sqlite.CreateDbContext())
            {
                var setTag = ctx.Set<Tag>();
                var tags = setTag.Select.Limit(10).ToList();
                setTag.BeginEdit(tags);

                tags.Add(new Tag
                {
                    Ddd = DateTime.Now.Second,
                    Name = "test_manytoMany_01_�й�2234234"
                });
                tags[0].Name = "123123";
                tags.RemoveAt(1);

                //tags.Clear();

                Assert.Equal(3, setTag.EndEdit());

                var test150_02 = ctx.Set<Tag>()
                    .Select.From<Tag>((s, b) => s.InnerJoin(a => a.Id == b.Id))
                    .ToList((a, b) => new
                    {
                        a.Id,a.Name,
                        id2 = b.Id, name2 = b.Name
                    });

                var songs = ctx.Set<Song>().Select
                    .IncludeMany(a => a.Tags)
                    .ToList();

                var tag1 = new Tag
                {
                    Ddd = DateTime.Now.Second,
                    Name = "test_manytoMany_01_�й�"
                };
                var tag2 = new Tag
                {
                    Ddd = DateTime.Now.Second,
                    Name = "test_manytoMany_02_����"
                };
                var tag3 = new Tag
                {
                    Ddd = DateTime.Now.Second,
                    Name = "test_manytoMany_03_�ձ�"
                };
                ctx.AddRange(new[] { tag1, tag2, tag3 });

                var song1 = new Song
                {
                    Create_time = DateTime.Now,
                    Title = "test_manytoMany_01_�����й���.mp3",
                    Url = "http://ww.baidu.com/"
                };
                var song2 = new Song
                {
                    Create_time = DateTime.Now,
                    Title = "test_manytoMany_02_����һ����.mp3",
                    Url = "http://ww.163.com/"
                };
                var song3 = new Song
                {
                    Create_time = DateTime.Now,
                    Title = "test_manytoMany_03_ǧ���һ��.mp3",
                    Url = "http://ww.sina.com/"
                };
                ctx.AddRange(new[] { song1, song2, song3 });

                ctx.Orm.Select<Tag>().Limit(10).ToList();

                ctx.AddRange(
                    new[] {
                        new Song_tag { Song_id = song1.Id, Tag_id = tag1.Id },
                        new Song_tag { Song_id = song2.Id, Tag_id = tag1.Id },
                        new Song_tag { Song_id = song3.Id, Tag_id = tag1.Id },
                        new Song_tag { Song_id = song1.Id, Tag_id = tag2.Id },
                        new Song_tag { Song_id = song3.Id, Tag_id = tag2.Id },
                        new Song_tag { Song_id = song3.Id, Tag_id = tag3.Id },
                    }
                );
                ctx.SaveChanges();
            }
        }

        [Fact]
        public void Add()
        {

            g.sqlite.SetDbContextOptions(opt =>
            {
                //opt.EnableAddOrUpdateNavigateList = false;
            });

            g.mysql.Insert<testenumWhere>().AppendData(new testenumWhere { type = testenumWhereType.Blaaa }).ExecuteAffrows();

            var sql = g.mysql.Select<testenumWhere>().Where(a => a.type == testenumWhereType.Blaaa).ToSql();
            var tolist = g.mysql.Select<testenumWhere>().Where(a => a.type == testenumWhereType.Blaaa).ToList();

            //֧�� 1�Զ� ��������

            using (var ctx = g.sqlite.CreateDbContext())
            {
                ctx.Options.EnableAddOrUpdateNavigateList = true;
                var tags = ctx.Set<Tag>().Select.IncludeMany(a => a.Tags).ToList();

                var tag = new Tag
                {
                    Name = "testaddsublist",
                    Tags = new[] {
                        new Tag { Name = "sub1" },
                        new Tag { Name = "sub2" },
                        new Tag {
                            Name = "sub3",
                            Tags = new[] {
                                new Tag { Name = "sub3_01" }
                            }
                        }
                    }
                };
                ctx.Add(tag);

                var tags2 = ctx.Orm.Select<Tag>().IncludeMany(a => a.Tags).ToList();

                ctx.SaveChanges();
            }
        }

        [Fact]
        public void Update()
        {
            //��ѯ 1�Զ࣬�ټ�������

            using (var ctx = g.sqlite.CreateDbContext())
            {
                ctx.Options.EnableAddOrUpdateNavigateList = true;
                var tag = ctx.Set<Tag>().Select.First();
                tag.Tags.Add(new Tag { Name = "sub3" });
                tag.Name = Guid.NewGuid().ToString();
                ctx.Update(tag);
                var xxx = ctx.Orm.Select<Tag>().First();
                ctx.SaveChanges();
            }
        }

        public class Song
        {
            [Column(IsIdentity = true)]
            public int Id { get; set; }
            public DateTime? Create_time { get; set; }
            public bool? Is_deleted { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }

            public virtual ICollection<Tag> Tags { get; set; }

            [Column(IsVersion = true)]
            public long versionRow { get; set; }
        }
        public class Song_tag
        {
            public int Song_id { get; set; }
            public virtual Song Song { get; set; }

            public int Tag_id { get; set; }
            public virtual Tag Tag { get; set; }
        }

        public class Tag
        {
            [Column(IsIdentity = true)]
            public int Id { get; set; }
            public int? Parent_id { get; set; }
            public virtual Tag Parent { get; set; }

            public decimal? Ddd { get; set; }
            public string Name { get; set; }

            public virtual ICollection<Song> Songs { get; set; }
            public virtual ICollection<Tag> Tags { get; set; }
        }
    }
}

