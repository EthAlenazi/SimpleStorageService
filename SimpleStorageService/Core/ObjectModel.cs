using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ObjectModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Data { get; set; }
    }
}
