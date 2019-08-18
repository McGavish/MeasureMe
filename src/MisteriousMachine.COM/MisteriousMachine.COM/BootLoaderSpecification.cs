using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriousMachine.COM
{
    public class BootLoaderSpecification
    {
        public byte[] Identificator { get; set; }
        public byte[] MemoryAddress { get; set; }
        public byte[] ExternalMemoryAddress { get; set; }
        public byte[] SystemMemoryAddress { get; set; }
        public byte[] FlashMemoryAddress { get; set; }
    }
}
