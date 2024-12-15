using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBAgent.Models;

/// <summary>
/// Организация
/// </summary>
[Table("Организация")]
public partial class Organization
{
    public int Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    [Column("Название")]
    public string Name { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    [Column("Адрес")]
    public string Adress { get; set; }

    /// <summary>
    /// Руководитель организации
    /// </summary>
    [Column("РуководительОрганизации")]
    public string FactoryLeader { get; set; }

    /// <summary>
    /// Документ
    /// </summary>
    [Column("Документ")]
    public string Document { get; set; }

    /// <summary>
    /// Должность руководителя
    /// </summary>
    [Column("Должность руководителя")]
    public string Rank { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
