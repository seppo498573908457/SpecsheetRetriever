using SpecsheetRetriever.Interfaces;
using SpecsheetRetriever.Workers;

namespace SpecsheetRetriever
{
    internal class RetrieverFactory
    {

        // Only one atm.
        internal IRetriever CreateRetriever() => new LoudspeakerDatabaseRetrieverV1();

    }
}
