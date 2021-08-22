using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using FreeSql;

namespace EMSServerModel.Model
{
	/// <summary>
	/// �û���ɫ��ϵ��aa111
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public partial class UserRole : BaseEntity<UserRole>{
		/// <summary>
		/// ��ɫ���1
		/// </summary>
		[JsonProperty]
		public long RoleId { get; set; }
		/// <summary>
		/// ��ɫ����
		/// </summary>
		[Navigate("RoleId")]
		public Role Roles { get; set; }

		/// <summary>
		/// �û����
		/// </summary>
		[JsonProperty, Column(DbType = "varchar(50)")]
		public string UserId { get; set; }
		/// <summary>
		/// �û�����
		/// </summary>
		[Navigate("UserId")]
		public User Users { get; set; }

	}

}
