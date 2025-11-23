using ATMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMProject.Data;
public interface ILogRepository
{
    Task AddLogAsync(TransactionLog log);
    Task<IEnumerable<TransactionLog>> GetAllLogsAsync();
}