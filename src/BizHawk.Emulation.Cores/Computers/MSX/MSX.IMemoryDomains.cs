﻿using System.Collections.Generic;
using System.Linq;

using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Computers.MSX
{
	public partial class MSX
	{
		private MemoryDomainList MemoryDomains;
		private readonly Dictionary<string, MemoryDomainByteArray> _byteArrayDomains = new Dictionary<string, MemoryDomainByteArray>();
		private bool _memoryDomainsInit = false;

		private void SetupMemoryDomains()
		{
			var domains = new List<MemoryDomain>
			{
				new MemoryDomainDelegate(
					"System Bus",
					0x10000,
					MemoryDomain.Endian.Little,
					(addr) => LibMSX.MSX_getsysbus(MSX_Pntr, (int)(addr & 0xFFFF)),
					(addr, value) => { },
					1),
				new MemoryDomainDelegate(
					"VRAM",
					0x4000,
					MemoryDomain.Endian.Little,
					(addr) => LibMSX.MSX_getvram(MSX_Pntr, (int)(addr & 0x3FFF)),
					(addr, value) => { },
					1),
				new MemoryDomainDelegate(
					"RAM",
					0x10000,
					MemoryDomain.Endian.Little,
					(addr) => LibMSX.MSX_getram(MSX_Pntr, (int)(addr & 0xFFFF)),
					(addr, value) => { },
					1)
			};

			if (SaveRAM != null)
			{
				var saveRamDomain = new MemoryDomainDelegate("Save RAM", SaveRAM.Length, MemoryDomain.Endian.Little,
					addr => SaveRAM[addr],
					(addr, value) => { SaveRAM[addr] = value; SaveRamModified = true; }, 1);
				domains.Add(saveRamDomain);
			}

			SyncAllByteArrayDomains();

			MemoryDomains = new MemoryDomainList(_byteArrayDomains.Values.Concat(domains).ToList());
			(ServiceProvider as BasicServiceProvider).Register<IMemoryDomains>(MemoryDomains);

			_memoryDomainsInit = true;
		}

		private void SyncAllByteArrayDomains()
		{
			SyncByteArrayDomain("ROM", RomData);
		}

		private void SyncByteArrayDomain(string name, byte[] data)
		{
			if (_memoryDomainsInit)
			{
				var m = _byteArrayDomains[name];
				m.Data = data;
			}
			else
			{
				var m = new MemoryDomainByteArray(name, MemoryDomain.Endian.Little, data, true, 1);
				_byteArrayDomains.Add(name, m);
			}
		}
	}
}
