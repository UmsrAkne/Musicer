using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musicer.Models.Databases
{
    public class SoundData
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // EntityFramework で使用するため、get, set の両方が必要。
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public long PlaybackTimeTicks { get; set; }

        [Required]
        public bool IsSkipped { get; set; }

        [NotMapped]
        public long Index { get; set; } = 0;

        [NotMapped]
        public DateTime LastListenDateTime { get; set; }

        [NotMapped]
        public int ListenCount { get; set; }
    }
}