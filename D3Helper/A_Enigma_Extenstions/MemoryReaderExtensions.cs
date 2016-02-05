using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.Memory;

namespace D3Helper.A_Enigma_Extenstions
{
    public static class MemoryReaderExtensions
    {
        public static T ReadChain<T>(this MemoryReader reader, MemoryAddress address, params int[] offsets)
        {
            for (int i = 0; i < offsets.Length; i++)
            {
                address = reader.Read<MemoryAddress>(address);
                address += offsets[i];
            }
            return reader.Read<T>(address);
        }
    }
}
