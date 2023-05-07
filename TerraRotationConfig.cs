using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.GameContent.UI;
using Filters = Terraria.Graphics.Effects.Filters;
using System.ComponentModel;

namespace TerraRotation
{
    [Label("ModConfig")]
    public class TerraRotationConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Range(0f, 10f)]
        [DefaultValue(3f)]
        public float 转速;

        [Range(0f, 2f)]
        [DefaultValue(1f)]
        public float 旋转画面尺寸;

    }
}
