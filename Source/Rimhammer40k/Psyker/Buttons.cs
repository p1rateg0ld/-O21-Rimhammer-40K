using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Psyker
{
    [StaticConstructorOnStartup]
    public static class TexButton
    {
        public static readonly Texture2D O21Tex_SkillBox = ContentFinder<Texture2D>.Get("UI/SkillsBox", true);
        public static readonly Texture2D O21Tex_SkillBoxAdd = ContentFinder<Texture2D>.Get("UI/SkillsBoxAdd", true);
        public static readonly Texture2D O21Tex_SkillBoxFull = ContentFinder<Texture2D>.Get("UI/SkillsBoxFull", true);
    }
}
