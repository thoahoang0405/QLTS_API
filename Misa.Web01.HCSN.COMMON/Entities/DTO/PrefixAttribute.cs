using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB01.HCSN.COMMON.Entities
{
    public class PrefixAttribute : Attribute
    {
        public string Name { get; set; }

        public PrefixAttribute(string name)
        {
            Name = name;
        }
    }
}
