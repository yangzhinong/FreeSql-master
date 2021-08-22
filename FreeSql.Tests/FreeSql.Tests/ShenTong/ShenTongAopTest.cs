﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace FreeSql.Tests.ShenTong
{
    public class ShenTongAopTest
    {

        class TestAuditValue
        {
            public Guid id { get; set; }
            [Now]
            public DateTime createtime { get; set; }
        }
        class NowAttribute: Attribute { }

        [Fact]
        public void AuditValue()
        {
            var date = DateTime.Now.Date;
            var item = new TestAuditValue();

            EventHandler<Aop.AuditValueEventArgs> audit = (s, e) =>
             {
                 if (e.Property.GetCustomAttribute<NowAttribute>(false) != null)
                     e.Value = DateTime.Now.Date;
             };
            g.shentong.Aop.AuditValue += audit;

            g.shentong.Insert(item).ExecuteAffrows();

            g.shentong.Aop.AuditValue -= audit;

            Assert.Equal(item.createtime, date);
        }
    }
}
