using System;
using System.Linq;
using System.Reflection;
using RimWorld;
using Verse;

namespace GraphicApparelDetour
{
	// Token: 0x02000004 RID: 4
	[StaticConstructorOnStartup]
	internal static class InjectorThingy
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000023B7 File Offset: 0x000005B7
		static InjectorThingy()
		{
			LongEventHandler.QueueLongEvent(new Action(InjectorThingy.InjectStuff), "Initializing", true, null);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000023D3 File Offset: 0x000005D3
		private static Assembly Assembly
		{
			get
			{
				return Assembly.GetAssembly(typeof(InjectorThingy));
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023E4 File Offset: 0x000005E4
		public static void InjectStuff()
		{
			MethodInfo method = typeof(ApparelGraphicRecordGetter).GetMethod("TryGetGraphicApparel", BindingFlags.Static | BindingFlags.Public);
			MethodInfo method2 = typeof(ApparelGraphicRecordGetter).GetMethod("TryGetGraphicApparel", BindingFlags.Static | BindingFlags.NonPublic);
			bool flag = !Detours.TryDetourFromTo(method, method2);
			if (flag)
			{
				Log.Error(InjectorThingy.AssemblyName + " failed to get injected properly.", false);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002448 File Offset: 0x00000648
		private static string AssemblyName
		{
			get
			{
				return InjectorThingy.Assembly.FullName.Split(new char[]
				{
					','
				}).First<string>();
			}
		}
	}
}
