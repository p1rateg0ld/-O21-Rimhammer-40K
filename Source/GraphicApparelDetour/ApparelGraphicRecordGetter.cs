using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace GraphicApparelDetour
{
	internal static class ApparelGraphicRecordGetter
	{
		internal static bool TryGetGraphicApparel(Apparel apparel, BodyTypeDef bodyType, out ApparelGraphicRecord rec)
		{
			Color drawColor = apparel.DrawColor;
			Color white = Color.white;
			bool flag = bodyType == null;
			if (flag)
			{
				Log.Error("Getting apparel graphic with undefined body type.", false);
				bodyType = BodyTypeDefOf.Male;
			}
			bool flag2 = GenText.NullOrEmpty(apparel.def.apparel.wornGraphicPath);
			bool result;
			if (flag2)
			{
				rec = new ApparelGraphicRecord(null, null);
				result = false;
			}
			else
			{
				Shader shader = ShaderDatabase.Cutout;
				bool flag3 = apparel.def.graphicData.shaderType == ShaderTypeDefOf.CutoutComplex;
				if (flag3)
				{
					shader = ShaderDatabase.CutoutComplex;
				}
				bool flag4 = apparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead;
				string text;
				if (flag4)
				{
					text = apparel.def.apparel.wornGraphicPath;
				}
				else
				{
					text = apparel.def.apparel.wornGraphicPath + "_" + bodyType.ToString();
				}
				Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(text, shader, apparel.def.graphicData.drawSize, drawColor, white);
				rec = new ApparelGraphicRecord(graphic, apparel);
				result = true;
			}
			return result;
		}
	}
}
