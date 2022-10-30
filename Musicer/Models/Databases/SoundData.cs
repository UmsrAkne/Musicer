using System.ComponentModel.DataAnnotations;

namespace Musicer.Models.Databases
{
    public class SoundData
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public long PlaybackTimeTicks { get; set; } = 0;
    }
}