using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpecsheetRetriever.Interfaces;
using SpecsheetRetriever.Models;

namespace SpecsheetRetriever.Workers
{
    internal class RetrieverWorker
    {
        internal bool WaitBetweenDrivers { get; set; }

        internal int SuccessCount { get; private set; }
        internal int FailedCount { get; private set; }
        internal List<SpecsheetItem> SuccessfulResults => _results ?? throw new Exception("Not run or completed yet.");
        internal List<SpecsheetItem>? _results { get; private set; }

        internal async Task Start(IRetriever retriever, List<SearchItem> searchList, CancellationToken cToken)
        {
            _results = null;
            var list = new List<SpecsheetItem>(searchList.Count);
            bool isFirst = true;

            if (!cToken.IsCancellationRequested)
            {
                foreach (SearchItem item in searchList)
                {
                    // Wait in hopes of not looking like a malicious user.
                    if (!isFirst && WaitBetweenDrivers) { Thread.Sleep(200); }

                    var specsheet = await retriever.SearchItem(item, cToken);

                    if (specsheet != null)
                    {
                        ++SuccessCount;
                        list.Add(specsheet);
                    }
                    else
                    {
                        ++FailedCount;
                    }

                    if (cToken.IsCancellationRequested)
                    {
                        break;
                    }

                    isFirst = false;
                }
            }

            _results = list;
        }

    }
}
