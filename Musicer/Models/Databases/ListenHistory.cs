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
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ListenCount { get; set; }

        [Required]
        public DateTime LastListenDateTime { get; set; }
    }
}
