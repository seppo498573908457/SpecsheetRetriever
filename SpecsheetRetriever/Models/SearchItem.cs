using SpecsheetRetriever.Interfaces;

namespace SpecsheetRetriever.Models
{
    internal class SearchItem : IListItem
    {
        internal string Brand { get; set; } = null!;
        internal string Model { get; set; } = null!;
        internal int? OhmVersion { get; set; }

        public virtual string ToListItemString() => $"{Brand} {Model}" + (OhmVersion.HasValue ? $" ({OhmVersion} Ω)" : null);
    }
}
