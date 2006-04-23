/*
general flingy   at 0x0000.  BYTE   - index into flingy.dat
overlay          at 0x00e3.  WORD
      ?          at 0x02ab.
construct sprite at 0x0534.  DWORD  - index into images.dat
shields          at 0x0a8c.  WORD 
hitpoints        at 0x0c54.  DWORD
animation level  at 0x0fe4.  BYTE
movement flags   at 0x10c8.  BYTE
      ?          at 0x11ac.

subunit range    at 0x1d40.  BYTE
sight range      at 0x1e24.  BYTE
      ?          at 0x1f08

mineral cost at 0x3844. WORD
gas cost at 0x3a0c. WORD
build time at 0x3bd4.  WORD

Restricts at 0x3d9c. WORD
? at 0x3f64.  BYTE

unit width at 0x2f5c.  WORD
unit height at 0x30ac.  WORD

space required at 0x4120.  BYTE


score for producing at 0x43d8. WORD
score for destroying at 0x45a0. WORD

*/

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SCSharp
{
	public class UnitsDat : MpqResource
	{
		public UnitsDat ()
		{
		}

		byte[] buf;

		public void ReadFromStream (Stream stream)
		{
			buf = new byte[(int)stream.Length];

			stream.Read (buf, 0, buf.Length);
		}

		/* offsets from the stardat.mpq version */

		const int flingy_offset = 0x0000;
		const int overlay_offset = 0x00e3;
		const int construct_sprite_offset = 0x0534;

		public byte GetFlingyId (int unit_id)
		{
			return buf[flingy_offset + unit_id];
		}
	}
}
