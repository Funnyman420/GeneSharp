using System;
using System.Collections.Generic;
using System.Text;

namespace GeneSharp.Interfaces
{


    public interface IShallowClonable<T>
    {
        T ShallowClone();
    }
}
