using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Contract
{
    public interface IMessage
    {
        String Id { get; }

        String CorrelationId { get; }
    }
}
