using System;
using System.Linq;
using System.Reflection;
using RimWorld;
using Verse;

namespace GraphicApparelDetour
{
	[StaticConstructorOnStartup]
	internal static class InjectorThingy
	{
		static InjectorThingy()
		{
			LongEventHandler.QueueLongEvent(new Action(InjectorThingy.InjectStuff), "Initializing", true, null);
		}

		private static Assembly Assembly
		{
			get
			{
				return Assembly.GetAssembly(typeof(InjectorThingy));
			}
		}

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
