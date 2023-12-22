using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Repository.Contracts;

public interface IErrorMessageRepository
{
    public Task<DataTable> SelectErrorMessagesAsync();
}
