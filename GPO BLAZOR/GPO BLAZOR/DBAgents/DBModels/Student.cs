using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;
/// <summary>
/// Инофрмация о студентах
/// </summary>
[Table("Студент")]
public partial class Student
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Column("Пользователь")]
    public decimal User { get; set; }
    /// <summary>
    /// Группа
    /// </summary>
    [Column("Группа")]
    public string Group { get; set; } = null!;
    /// <summary>
    /// Год поступления
    /// </summary>
    [Column("ГодПоступления")]
    public string AdmissionYear { get; set; } = null!;

    public virtual Groups GroupsNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
