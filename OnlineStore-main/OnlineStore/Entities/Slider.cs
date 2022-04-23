using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Entities
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
    }
}
