using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public abstract class BaseStatsId<T> : BaseStats
        where T : IComparable
    {
        [Key]
        public T Id { get; set; }
    }
}
