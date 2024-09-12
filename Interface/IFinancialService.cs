using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stockapi.Models;

namespace stockapi.Interface
{
    public interface IFinancialService
    {
        Task<Stock?> FindStockByName(string Symbol);
    }
}