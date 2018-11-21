using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace GraphicApparelDetour
{
	// Token: 0x02000002 RID: 2
	public static class Detours
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public unsafe static bool TryDetourFromTo(MethodInfo source, MethodInfo destination)
		{
			bool flag = source == null;
			bool result;
			if (flag)
			{
				Log.Message("Apparel Stuff Color source is null", false);
				result = false;
			}
			else
			{
				bool flag2 = destination == null;
				if (flag2)
				{
					Log.Message("Apparel Stuff Color target is null", false);
					result = false;
				}
				else
				{
					Log.Message("Apparel Stuff Color detours loaded, hopefully correct", false);
					string item = string.Concat(new string[]
					{
						source.DeclaringType.FullName,
						".",
						source.Name,
						" @ 0x",
						source.MethodHandle.GetFunctionPointer().ToString("X" + (IntPtr.Size * 2).ToString())
					});
					string item2 = string.Concat(new string[]
					{
						destination.DeclaringType.FullName,
						".",
						destination.Name,
						" @ 0x",
						destination.MethodHandle.GetFunctionPointer().ToString("X" + (IntPtr.Size * 2).ToString())
					});
					bool flag3 = Detours.detoured.Contains(item);
					if (flag3)
					{
					}
					Detours.detoured.Add(item);
					Detours.destinations.Add(item2);
					bool flag4 = IntPtr.Size == 8;
					if (flag4)
					{
						long num = source.MethodHandle.GetFunctionPointer().ToInt64();
						long num2 = destination.MethodHandle.GetFunctionPointer().ToInt64();
						byte* ptr = num;
						long* ptr2 = (long*)(ptr + 2);
						*ptr = 72;
						ptr[1] = 184;
						*ptr2 = num2;
						ptr[10] = byte.MaxValue;
						ptr[11] = 224;
					}
					else
					{
						int num3 = source.MethodHandle.GetFunctionPointer().ToInt32();
						int num4 = destination.MethodHandle.GetFunctionPointer().ToInt32();
						byte* ptr3 = num3;
						int* ptr4 = (int*)(ptr3 + 1);
						int num5 = num4 - num3 - 5;
						*ptr3 = 233;
						*ptr4 = num5;
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		private static List<string> detoured = new List<string>();

		// Token: 0x04000002 RID: 2
		private static List<string> destinations = new List<string>();
	}
}
