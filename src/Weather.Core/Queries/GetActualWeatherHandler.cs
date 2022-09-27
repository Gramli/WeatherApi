using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Payloads;

namespace Weather.Core.Queries
{
    internal class GetActualWeatherHandler : IGetActualWeatherHandler
    {
        public Task<IReadOnlyCollection<ActualWeatherDto>> HandleAsync(LocationPayload request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
