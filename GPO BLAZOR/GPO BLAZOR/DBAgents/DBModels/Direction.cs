using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;

/// <summary>
/// Направление
/// </summary>
[Table("Направление")]
public partial class Direction
{
    /// <summary>
    /// ID
    /// </summary>
    public decimal Id { get; set; }
    /// <summary>
    /// Код специальности
    /// </summary>
    [Column("Код")]
    public string Code { get; set; } = null!;
    /// <summary>
    /// Название специальности
    /// </summary>
    [Column("Название")]
    public string DirectionName { get; set; } = null!;
    /// <summary>
    /// Профиль подготовки
    /// </summary>
    [Column("Профиль")]
    public string DirectiinProfile { get; set; } = null!;
    /// <summary>
    /// Руководитель по направлению
    /// </summary>
    [Column("Руководитель")]
    public decimal? Leader { get; set; }

    public virtual ICollection<Groups> Groups { get; set; } = new List<Groups>();

    public virtual User? LeaderNavigation { get; set; }
}
