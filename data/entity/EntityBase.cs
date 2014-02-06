
using System;
using System.ComponentModel.DataAnnotations;

namespace data.entity
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public EntityBase()
        {
            CreatedDateTime = DateTime.Now;
        }
    }
}
