using Library.Core.Abstractions;
using Library.Core.Abstractions.IInfrastructure;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Statistics
{
    public class GetUserStat 
    {
        private readonly IUserStatistics _statistics;
        public GetUserStat(IUserStatistics statistics)
        {

            _statistics = statistics;

        }
        public async Task Execute()
        {
            await _statistics.Execute();
        }


    }
}
