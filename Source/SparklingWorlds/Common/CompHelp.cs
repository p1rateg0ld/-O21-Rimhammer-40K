using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Rimhammer40k.Common
{
    [StaticConstructorOnStartup]
    public class CompHelp : ThingComp
    {
        public static readonly Texture2D LaunchReportTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true);
        public string HelpText
        {
            get
            {
                if (Translator.TryTranslate($"{parent.def.defName}_HelpText", out string text))
                {
                    return text;
                }
                return null;
            }
        }
        public string OrdoText
        {
            get
            {
                if (Translator.TryTranslate($"{parent.def.defName}_OrdoText", out string text))
                {
                    return text;
                }
                return null;
            }
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo g in base.CompGetGizmosExtra()) yield return g;
            string helpText = HelpText;
            if (!string.IsNullOrEmpty(helpText))
            {
                yield return new Command_Action
                {
                    defaultLabel = "CommonHelp".Translate(),
                    defaultDesc = "CommonHelpDesc".Translate(),
                    icon = LaunchReportTex,
                    action = () =>
                    {
                        if (Find.WindowStack.WindowOfType<Dialog_MessageBox>() == null)
                        {
                            Find.WindowStack.Add(new Dialog_MessageBox(helpText));
                        }
                    }
                };
            }
        }
    }
}
