using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;

/// <summary>
/// Статус
/// </summary>
[Table("Статус")]
public partial class Status
{
    /// <summary>
    /// ID
    /// </summary>
    public decimal Id { get; set; }
    /// <summary>
    /// Статус1
    /// </summary>
    [Column("Статус1")]
    public string StatusFirst { get; set; } = null!;

    public virtual ICollection<AskForm> AskForms { get; set; } = new List<AskForm>();
}
