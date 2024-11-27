using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;
/// <summary>
/// Вид и тип практики
/// </summary>
[Table("ВидИТипПрактики")]
public partial class TypeandViemType
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

    public virtual ICollection<AskForm> AskForms { get; set; } = new List<AskForm>();
}
