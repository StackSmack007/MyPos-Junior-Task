using System;

namespace Infrastructure.Models
{
    public abstract class BaseStats
    {
        private bool isDeleted;
        public BaseStats()
        {
            IsDeleted = false;
            var currentTime = DateTime.UtcNow;
            CreatedOn = currentTime;
            LastModified = currentTime;
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                if (value)
                {
                    this.DeletedOn = DateTime.UtcNow;
                }
                else
                {
                    this.DeletedOn = null;
                }

                isDeleted = value;
            }
        }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime? DeletedOn { get; private set; }
    }
}