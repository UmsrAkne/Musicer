namespace Musicer.Models.Databases
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ListenHistory
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int SoundDataId { get; set; }

        [Required]
        public DateTime ListenDateTime { get; set; }

        [NotMapped]
        public string Name { get; set; } = string.Empty;

        [NotMapped]
        public int ListenCount { get; set; }

        [NotMapped]
        public int Index { get; set; }
    }
}