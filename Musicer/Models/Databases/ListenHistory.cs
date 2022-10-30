namespace Musicer.Models.Databases
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ListenHistory
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int SoundDataId { get; set; }

        [Required]
        public DateTime ListenDateTime { get; set; }
    }
}