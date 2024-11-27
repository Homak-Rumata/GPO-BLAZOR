using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;

/// <summary>
/// Таблица атрибутов кафедры
/// </summary>
[Table("Кафедра")]
public partial class Cafedral
{
    /// <summary>
    /// ID
    /// </summary>
    public decimal Id { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    [Column("Название")]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Заведующий кафедры
    /// </summary>
    [Column("Заведующий")]
    public decimal Leader { get; set; }

    public virtual ICollection<Groups> Groups { get; set; } = new List<Groups>();

    public virtual User LeaderNavigation { get; set; } = null!;
}
