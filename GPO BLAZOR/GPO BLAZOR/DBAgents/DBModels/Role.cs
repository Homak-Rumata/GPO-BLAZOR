using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;

/// <summary>
/// Роль
/// </summary>
[Table("Роль")]
public partial class Role
{
    /// <summary>
    /// ID
    /// </summary>
    public decimal Id { get; set; }
    /// <summary>
    /// Название роли
    /// </summary>
    [Column("Название")]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
