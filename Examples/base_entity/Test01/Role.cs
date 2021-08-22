using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using FreeSql;

namespace EMSServerModel.Model
{
	/// <summary>
	/// ��ɫ��
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public partial class Role : BaseEntity<Role>{
		/// <summary>
		/// ��ɫ���
		/// </summary>
		[JsonProperty, Column(IsPrimary = true, IsIdentity = true)]
		public long RoleId { get; set; }

		/// <summary>
		/// ��ɫ����
		/// </summary>
		[JsonProperty, Column(DbType = "varchar(50)")]
		public string RoleName { get; set; } = string.Empty;

		/// <summary>
		/// ��ɫ����
		/// </summary>
		[JsonProperty, Column(DbType = "varchar(50)")]
		public string RoleDesc { get; set; } = string.Empty;

		///// <summary>
		///// ����ʱ��
		///// </summary>
		//[JsonProperty, Column(DbType = "date")]
		//public DateTime CreateTime { get; set; } = DateTime.Now;

		/// <summary>
		/// ����
		/// </summary>
		[JsonProperty]
		public bool IsEnable { get; set; } = true;

		/// <summary>
		/// ��ɫ�û���Զർ��
		/// </summary>
		[Navigate(ManyToMany = typeof(UserRole))]
		public virtual ICollection<User> Users { get; protected set; }

	}

}
