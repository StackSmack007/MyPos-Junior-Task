using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public abstract class BaseId<T> 
    where T : IComparable
    {
        [Key]
        public T Id { get; set; }
    }
}
