
namespace SpecsheetRetriever.Models
{
    internal class SpecsheetItem : SearchItem
    {
        public string DataSource { get; set; } = null!;
        public string Payload { get; set; } = null!;

        public override string ToListItemString()
        {
            return base.ToListItemString() + $", source: {DataSource}, payload: {Payload.Length}";
        }
    }
}
