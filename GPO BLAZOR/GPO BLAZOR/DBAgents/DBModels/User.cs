using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPO_BLAZOR.DBAgents.DBModels;
/// <summary>
/// Пользователь
/// </summary>
[Table("Пользователь")]
public partial class User
{
    /// <summary>
    /// ID
    /// </summary>
    public decimal Id { get; set; }
    /// <summary>
    /// Email - электронная почта
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// Пароль
    /// </summary>
    [Column("Пароль")]
    public string Password { get; set; } = null!;
    /// <summary>
    /// Фамилия
    /// </summary>
    [Column("Фамилия")]
    public string Surname { get; set; } = null!;
    /// <summary>
    /// Имя
    /// </summary>
    [Column("Имя")]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Отчество
    /// </summary>
    [Column("Отчество")]
    public string? MiddleName { get; set; }

    public virtual ICollection<AskForm> AskFormResponsibleForFillingNavigations { get; set; } = new List<AskForm>();

    public virtual ICollection<AskForm> AskFormSupervisorAndConsultantNavigations { get; set; } = new List<AskForm>();

    public virtual ICollection<AskForm> AskFormFactoryPracticeLeaderNavigations { get; set; } = new List<AskForm>();

    public virtual ICollection<Cafedral> Cafedrals { get; set; } = new List<Cafedral>();

    public virtual ICollection<Direction> Directions { get; set; } = new List<Direction>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Organisation> Organisations { get; set; } = new List<Organisation>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
