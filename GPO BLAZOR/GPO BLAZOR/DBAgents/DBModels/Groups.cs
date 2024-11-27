using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;
/// <summary>
/// Таблица групп
/// </summary>
[Table("Группы")]
public partial class Groups
{
    /// <summary>
    /// Группа
    /// </summary>
    [Column("Группа")]
    public string Group { get; set; } = null!;
    /// <summary>
    /// Направление
    /// </summary>
    [Column("Направление")]
    public decimal Direction { get; set; }
    /// <summary>
    /// Кафедра
    /// </summary>
    [Column("Кафедра")]
    public decimal Cafedral { get; set; }
    /// <summary>
    /// Курс
    /// </summary>
    [Column("Курс")]
    public string Course { get; set; } = null!;
    /// <summary>
    /// Год поступления
    /// </summary>
    [Column("ГодПоступления")]
    public string Year { get; set; } = null!;

    public virtual Cafedral CafedralNavigation { get; set; } = null!;

    public virtual Direction DirectionNavigation { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
