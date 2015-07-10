using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloShare.BBCode
{
    public interface IBBCode
    {
        string Parse(string token, string parameter, string value);
    }
}
