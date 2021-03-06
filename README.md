<h1 align="center"> ð¦ FreeSql </h1><div align="center">

FreeSql is a powerful O/RM component, supports .NET Core 2.1+, .NET Framework 4.0+, and Xamarin.

[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore) 
[![nuget](https://img.shields.io/nuget/v/FreeSql.svg?style=flat-square)](https://www.nuget.org/packages/FreeSql) 
[![stats](https://img.shields.io/nuget/dt/FreeSql.svg?style=flat-square)](https://www.nuget.org/stats/packages/FreeSql?groupby=Version) 
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/dotnetcore/FreeSql/master/LICENSE.txt)

<p>
    <span>English</span> |  
    <a href="README.zh-CN.md">ä¸­æ</a>
</p>

</div>

- ð  Support CodeFirst data migration.
- ð» Support DbFirst import entity class from database, or use [Generation Tool](https://github.com/dotnetcore/FreeSql/wiki/DbFirst).
- â³ Support advanced type mapping, such as PostgreSQL array type, etc.
- ð² Support expression functions, and customizable analysis.
- ð Support one-to-many and many-to-many navigation properties, include and lazy loading.
- ð Support Read/Write separation, Splitting Table/Database, Global filters, Optimistic and pessimistic locker.
- ð³ Support MySql/SqlServer/PostgreSQL/Oracle/Sqlite/Firebird/Dameng/äººå¤§éä»/Shentong Database/ç¿°é«/HUAWEI GaussDB/Access, etc.

QQ Groupsï¼4336577(full)ã8578575(full)ã**52508226(available)**

## ð Documentation

| |
| - |
| [Get started](https://www.cnblogs.com/FreeSql/p/11531300.html)&nbsp;&nbsp;\|&nbsp;&nbsp;[Select](https://github.com/dotnetcore/FreeSql/wiki/Query-Data)&nbsp;&nbsp;\|&nbsp;&nbsp;[Update](https://github.com/dotnetcore/FreeSql/wiki/Update-Data)&nbsp;&nbsp;\|&nbsp;&nbsp;[Insert](https://github.com/dotnetcore/FreeSql/wiki/Insert-Data)&nbsp;&nbsp;\|&nbsp;&nbsp;[Delete](https://github.com/dotnetcore/FreeSql/wiki/Delete-Data)&nbsp;&nbsp;\|&nbsp;&nbsp;[FAQ](https://github.com/dotnetcore/FreeSql/wiki/%E5%B8%B8%E8%A7%81%E9%97%AE%E9%A2%98)&nbsp;&nbsp;|
| [Expression](https://github.com/dotnetcore/FreeSql/wiki/%e8%a1%a8%e8%be%be%e5%bc%8f%e5%87%bd%e6%95%b0)&nbsp;&nbsp;\|&nbsp;&nbsp;[CodeFirst](https://github.com/dotnetcore/FreeSql/wiki/CodeFirst)&nbsp;&nbsp;\|&nbsp;&nbsp;[DbFirst](https://github.com/2881099/FreeSql/wiki/DbFirst)&nbsp;&nbsp;\|&nbsp;&nbsp;[Filters](https://github.com/dotnetcore/FreeSql/wiki/%e8%bf%87%e6%bb%a4%e5%99%a8)&nbsp;&nbsp;\|&nbsp;&nbsp;[AOP](https://github.com/2881099/FreeSql/wiki/AOP)&nbsp;&nbsp;|
| [Repository](https://github.com/dotnetcore/FreeSql/wiki/Repository)&nbsp;&nbsp;\|&nbsp;&nbsp;[UnitOfWork](https://github.com/dotnetcore/FreeSql/wiki/%e5%b7%a5%e4%bd%9c%e5%8d%95%e5%85%83)&nbsp;&nbsp;\|&nbsp;&nbsp;[DbContext](https://github.com/dotnetcore/FreeSql/wiki/DbContext)&nbsp;&nbsp;\|&nbsp;&nbsp;[ADO](https://github.com/2881099/FreeSql/wiki/ADO)&nbsp;&nbsp;|
| [Read/Write](https://github.com/dotnetcore/FreeSql/wiki/%e8%af%bb%e5%86%99%e5%88%86%e7%a6%bb)&nbsp;&nbsp;\|&nbsp;&nbsp;[Splitting Table](https://github.com/dotnetcore/FreeSql/wiki/%e5%88%86%e8%a1%a8%e5%88%86%e5%ba%93)&nbsp;&nbsp;\|&nbsp;&nbsp;[Hide tech](https://github.com/dotnetcore/FreeSql/wiki/%E9%AA%9A%E6%93%8D%E4%BD%9C)&nbsp;&nbsp;\|&nbsp;&nbsp;[*Update Notes*](https://github.com/dotnetcore/FreeSql/wiki/%e6%9b%b4%e6%96%b0%e6%97%a5%e5%bf%97)&nbsp;&nbsp;|

> Please select a development mode:

- Use FreeSql, keep the original usage.
- Use [FreeSql.Repository](https://github.com/dotnetcore/FreeSql/wiki/Repository), Repository + UnitOfWork.
- Use [FreeSql.DbContext](https://github.com/dotnetcore/FreeSql/wiki/DbContext), Like efcore.
- Use [FreeSql.BaseEntity](https://github.com/dotnetcore/FreeSql/tree/master/Examples/base_entity), Simple mode.

> Some open source projects that use FreeSql:

- [Zhontai.net Management System](https://github.com/zhontai/Admin.Core)
- [A simple CMS implemented by .NET5](https://github.com/luoyunchong/lin-cms-dotnetcore)

<p align="center">
  <img src="https://github.com/dotnetcore/FreeSql/raw/master/functions11.png"/>
</p>

## ð Quick start

> dotnet add package FreeSql.Provider.Sqlite

```csharp
static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
  .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=document.db")
  .UseAutoSyncStructure(true) //automatically synchronize the entity structure to the database
  .Build(); //be sure to define as singleton mode

class Song {
  [Column(IsIdentity = true)]
  public int Id { get; set; }
  public string Title { get; set; }
  public string Url { get; set; }
  public DateTime CreateTime { get; set; }
  
  public ICollection<Tag> Tags { get; set; }
}
class Song_tag {
  public int Song_id { get; set; }
  public Song Song { get; set; }
  
  public int Tag_id { get; set; }
  public Tag Tag { get; set; }
}
class Tag {
  [Column(IsIdentity = true)]
  public int Id { get; set; }
  public string Name { get; set; }
  
  public int? Parent_id { get; set; }
  public Tag Parent { get; set; }
  
  public ICollection<Song> Songs { get; set; }
  public ICollection<Tag> Tags { get; set; }
}
```

### ð Query
```csharp
//OneToOneãManyToOne
fsql.Select<Tag>().Where(a => a.Parent.Parent.Name == "English").ToList();

//OneToMany
fsql.Select<Tag>().IncludeMany(a => a.Tags, then => then.Where(sub => sub.Name == "foo")).ToList();

//ManyToMany
fsql.Select<Song>()
  .IncludeMany(a => a.Tags, then => then.Where(sub => sub.Name == "foo"))
  .Where(s => s.Tags.AsSelect().Any(t => t.Name == "Chinese"))
  .ToList();

//Other
fsql.Select<YourType>()
  .Where(a => a.IsDelete == 0)
  .WhereIf(keyword != null, a => a.UserName.Contains(keyword))
  .WhereIf(role_id > 0, a => a.RoleId == role_id)
  .Where(a => a.Nodes.AsSelect().Any(t => t.Parent.Id == t.UserId))
  .Count(out var total)
  .Page(page, size)
  .OrderByDescending(a => a.Id)
  .ToList()
```
[More..](https://github.com/dotnetcore/FreeSql/wiki/%e6%9f%a5%e8%af%a2)

```csharp
fsql.Select<Song>().Where(a => new[] { 1, 2, 3 }.Contains(a.Id)).ToList();

fsql.Select<Song>().Where(a => a.CreateTime.Date == DateTime.Today).ToList();

fsql.Select<Song>().OrderBy(a => Guid.NewGuid()).Limit(10).ToList();
```
[More..](https://github.com/dotnetcore/FreeSql/wiki/%e8%a1%a8%e8%be%be%e5%bc%8f%e5%87%bd%e6%95%b0) 

### ð Repository

> dotnet add package FreeSql.Repository

```csharp
[Transactional]
public void Add() {
  var repo = ioc.GetService<BaseRepository<Tag>>();
  repo.DbContextOptions.EnableAddOrUpdateNavigateList = true;

  var item = new Tag {
    Name = "testaddsublist",
    Tags = new[] {
      new Tag { Name = "sub1" },
      new Tag { Name = "sub2" }
    }
  };
  repo.Insert(item);
}
```

Reference: [Use `TransactionalAttribute` and `UnitOfWorkManager` in ASP.NET Core to Achieve the *Multiple Transaction Propagation*](https://github.com/dotnetcore/FreeSql/issues/289).

## ðª Performance

FreeSql Query & Dapper Query

```shell
Elapsed: 00:00:00.6733199; Query Entity Counts: 131072; ORM: Dapper

Elapsed: 00:00:00.4554230; Query Tuple Counts: 131072; ORM: Dapper

Elapsed: 00:00:00.6846146; Query Dynamic Counts: 131072; ORM: Dapper

Elapsed: 00:00:00.6818111; Query Entity Counts: 131072; ORM: FreeSql*

Elapsed: 00:00:00.6060042; Query Tuple Counts: 131072; ORM: FreeSql*

Elapsed: 00:00:00.4211323; Query ToList<Tuple> Counts: 131072; ORM: FreeSql*

Elapsed: 00:00:01.0236285; Query Dynamic Counts: 131072; ORM: FreeSql*
```

FreeSql ToList & Dapper Query

```shell
Elapsed: 00:00:00.6707125; ToList Entity Counts: 131072; ORM: FreeSql*

Elapsed: 00:00:00.6495301; Query Entity Counts: 131072; ORM: Dapper
```

[More..](https://github.com/dotnetcore/FreeSql/wiki/%e6%80%a7%e8%83%bd)

## ð¯ Contributors

<a href="https://contributors-img.web.app/image?repo=dotnetcore/FreeSql">
  <img src="https://contributors-img.web.app/image?repo=dotnetcore/FreeSql" />
</a>

And other friends who made important suggestions for this project, they include:

[systemhejiyong](https://github.com/systemhejiyong), 
[LambertW](https://github.com/LambertW), 
[mypeng1985](https://github.com/mypeng1985), 
[stulzq](https://github.com/stulzq), 
[movingsam](https://github.com/movingsam), 
[ALer-R](https://github.com/ALer-R), 
[zouql](https://github.com/zouql), 
æ·±å³|åè¶, 
[densen2014](https://github.com/densen2014), 
[LiaoLiaoWuJu](https://github.com/LiaoLiaoWuJu), 
[hd2y](https://github.com/hd2y), 
[tky753](https://github.com/tky753), 
[feijie999](https://github.com/feijie999), 
constantine, 
[JohnZhou2020](https://github.com/JohnZhou2020), 
[mafeng8](https://github.com/mafeng8), etc.

## ð Donation

L*y 58åãè±è± 88åãéº¦åå¾ä¹ 50åãç½ç»æ¥è 2000åãJohn 99.99åãalex 666åãbacongao 36åãæ å 100åãEternity 188åãæ å 10åãâ.Helper~..oO 66åãä¹ æ¯ä¸è¢«ä¹ æ¯ 100åãæ å 100åãè¡æå 88.88åãä¸­è®¯ç§æ 1000åãGood Good Work 24åãç½ç° 6.6åãNothing 100åãå°å·å¤©æèµµ 500åãåå©è·¯äº 300åã
æ å 100åãè°ä¼ 99.99åãTCYM 66.66åãMOTA 5åãLDZXG 30åãNear 30åãå»ºç½ 66åãæ å 200åãLambertWu 100åãæ å 18.88åãä¹é¾ 50åãæ å 100åãé³æ¼æ¼ 66.66åãé³æ¼æ¼ 66.66åãä¸æ·® 100åãæä¼å-Excelå¬åå 100åãç½ç 6.66åãå¥¹å¾®ç¬çè¸y 30åãEternityÂ²ÂºÂ²Â¹ 588åãå¤å½æ´é¨ 88åãè¡æå 666.66åã
*ç¤¼ 10åãlitrpa 88åãAlax CHOW 200åãDaily 66åãk\*t 66åãè 100åã*è 10åãçå½å¦æ­ 1000å

> Thank you for your donation

- [Alipay](https://www.cnblogs.com/FreeSql/gallery/image/338860.html)

- [WeChat](https://www.cnblogs.com/FreeSql/gallery/image/338859.html)

## ð License

[MIT](LICENSE)
