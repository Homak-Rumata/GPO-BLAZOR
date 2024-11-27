using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;
/// <summary>
/// Таблица анкет
/// </summary>
[Table("Анкета")]
public partial class AskForm
{
    /// <summary>
    /// Студент
    /// </summary>
    [Column("Студент")]
    public decimal Student { get; set; }
    /// <summary>
    /// Группа
    /// </summary>
    [Column("Группа")]
    public string Group { get; set; } = null!;
    /// <summary>
    /// Вид и тип практики
    /// </summary>
    [Column("ВидИТип")]
    public decimal? TypeandViemType { get; set; }
    /// <summary>
    /// Номер договора
    /// </summary>
    [Column("Договор")]
    public decimal? Contract { get; set; }
    /// <summary>
    /// Руководитель и консультант
    /// </summary>
    [Column("РуководительКонсультант")]
    public decimal? SupervisorAndConsultant { get; set; }
    /// <summary>
    /// Руководитель практики организации
    /// </summary>
    [Column("РуководительПрактикиОрг")]
    public decimal? FactoryPracticeLeader { get; set; }
    /// <summary>
    /// Ответственные за заполнение
    /// </summary>
    [Column("ОтветственныеЗаЗаполнение")]
    public decimal ResponsibleForFilling { get; set; }
    /// <summary>
    /// Статус
    /// </summary>
    [Column("Статус")]
    public decimal Status { get; set; }
    /// <summary>
    /// Комментарий
    /// </summary>
    [Column("Комментарий")]
    public string? Commentary { get; set; }
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }

    public virtual TypeandViemType? TypeandViemTypeNavigation { get; set; }

    public virtual Contract? ContractNavigation { get; set; }

    public virtual User ResponsibleForFillingеNavigation { get; set; } = null!;

    public virtual User? SupervisorAndConsultantNavigation { get; set; }

    public virtual User? FactoryPracticeLeaderNavigation { get; set; }

    public virtual Status StatusNavigation { get; set; } = null!;
}
