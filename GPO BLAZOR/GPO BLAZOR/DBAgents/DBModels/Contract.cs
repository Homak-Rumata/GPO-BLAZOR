using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;
/// <summary>
/// Договор
/// </summary>
[Table("Договор")]
public partial class Contract
{
    /// <summary>
    /// Id
    /// </summary>
    public decimal Id { get; set; }
    /// <summary>
    /// Номер договора о практике
    /// </summary>
    [Column("НомерДоговораОПрактике")]
    public string PracticeContractNumber { get; set; } = null!;
    /// <summary>
    /// Дата начала практики
    /// </summary>
    [Column("ДатаНачалаПрактики")]
    public DateOnly DataPracticeStyle { get; set; }
    /// <summary>
    /// Дата окончания практики
    /// </summary>
    [Column("ДатаОкончанияПрактики")]
    public DateOnly DataPracticeEnd { get; set; }
    /// <summary>
    /// Дата договора о практике
    /// </summary>
    [Column("ДатаДоговораОПрактике")]
    public DateOnly DataContract { get; set; }
    /// <summary>
    /// Организация
    /// </summary>
    [Column("Организация")]
    public decimal Organisation { get; set; }
    /// <summary>
    /// Помещение
    /// </summary>
    [Column("Помещение")]
    public string Room { get; set; } = null!;
    /// <summary>
    /// Материально техническое обеспечение
    /// </summary>
    [Column("МатериальноТехническоеОбеспече")]
    public string Equipment { get; set; } = null!;

    public virtual ICollection<AskForm> AskForms { get; set; } = new List<AskForm>();

    public virtual Organisation OrganisationNavigation { get; set; } = null!;
}
