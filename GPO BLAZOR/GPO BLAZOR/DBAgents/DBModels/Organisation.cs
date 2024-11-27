using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;
/// <summary>
/// Таблица атрибутов организаций
/// </summary>
[Table("Организация")]
public partial class Organisation
{
    /// <summary>
    /// ID
    /// </summary>
    public decimal Id { get; set; }
    /// <summary>
    /// Название организации
    /// </summary>
    [Column("Название")]
    public string Namr { get; set; } = null!;
    /// <summary>
    /// Адресс местонахождения организации
    /// </summary>
    [Column("Адрес")]
    public string Address { get; set; } = null!;
    /// <summary>
    /// Название руководителя организациивании
    /// </summary>
    [Column("НазРуководительОрганизациивание")]
    public string FactoryLeaderName { get; set; } = null!;
    /// <summary>
    /// Название документа
    /// </summary>
    [Column("Документ")]
    public string Documentary { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
