using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLProjectDW.Core.Application.Interfaces
{
    public interface ITransformer<TInput, TOutput>
    {
        IEnumerable<TOutput> Transform(IEnumerable<TInput> source);
    }
}
