using Misa.Web01.HCSN.COMMON.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.COMMON
{
    public static class MISAResource
    {
        public static string GetResource(string key)
        {
            return Misa.Web01.HCSN.COMMON.Resource.Resource.ResourceManager.GetString(key);
        }
    }
}
