using NServiceKit.DataAnnotations;

namespace Core.Entities
{
    public class Oxide : Entity
    {
        [Index(Unique = true)]
        public string Formula { get; set; }

        public bool IsDefault { get; set; }

        public bool IsRequred { get; set; }

        public override string ToString()
        {
            return this.Formula;
        }
    }
}
