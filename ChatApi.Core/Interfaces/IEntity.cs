using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Core.Interfaces
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
        
    }
}