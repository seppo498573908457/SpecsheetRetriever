using System.Threading;
using System.Threading.Tasks;
using SpecsheetRetriever.Models;

namespace SpecsheetRetriever.Interfaces
{
    internal interface IRetriever
    {
        /// <summary>
        /// Null on failure.
        /// </summary>
        Task<SpecsheetItem?> SearchItem(SearchItem searchItem, CancellationToken cToken);

    }
}
