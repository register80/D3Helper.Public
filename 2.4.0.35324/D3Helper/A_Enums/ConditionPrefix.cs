using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

namespace D3Helper.A_Enums
{
    [ProtoContract]
    public enum ConditionPrefix
    {
        NONE = -1,
        AND = 0,
        OR = 1

    }
}
