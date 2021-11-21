using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoGuru_Project.Models
{
    public class Images
    {
        [Key]
        public int Id { get; set; }

        public string PicName { get; set; }

        public byte[] Picture { get; set; }
    }
}
