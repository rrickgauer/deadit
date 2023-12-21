using Deadit.Lib.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Domain.Errors;

public class ApiResponse<T>
{
    public List<ErrorMessage> Errors { get; set; } = new();
    public T? Data { get; set; }
}
