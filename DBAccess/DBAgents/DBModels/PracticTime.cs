using DBAgent.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess.DBAgents.DBModels
{
    [Table("Даты практики")]
    public partial class PracticTime
    {
        public int ID { get; set; }
        /// <summary>
        /// Дата начала практики
        /// </summary>
        [Column("Дата начала")]
        public DateOnly DateStart { get; set; }

        /// <summary>
        /// Дата конца практики
        /// </summary>
        [Column("Дата окончания")]
        public DateOnly DateEnd { get; set; }
        /// <summary>
        /// Год
        /// </summary>
        [Column("Год")]
        public int Year { get; set; }
        /// <summary>
        /// Направление
        /// </summary>
        [Column("Направление")]
        public int Direction { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        public virtual Direction DirectionNavigation { get; set; } = null!;

    }
}
