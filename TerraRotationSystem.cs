using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace TerraRotation
{
    public class TerraRotationSystem : ModSystem
    {
        private BasicEffect basicEffect;
        private VertexPositionColorTexture[] vertices;
        private int[] indices;
        private Matrix worldMatrix;
        private Vector3 cameraPosition;
        private Vector3 cameraTarget;
        private Vector3 cameraUpVector;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;
        private float rad = 0;

        private Texture2D texture;

        private float rotVec = 0.03f;
        private float scale = 1f;
        private float scaleNote = 1f;
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            var config = ModContent.GetInstance<TerraRotationConfig>();

            rotVec = config.转速;
            scale = config.旋转画面尺寸;
            //释放内存
            if (texture != null)
            {
                texture.Dispose();
                texture = null;
            }

            texture = new Texture2D(Main.screenTarget.GraphicsDevice, Main.screenTarget.Width, Main.screenTarget.Height);
            Color[] data = new Color[Main.screenTarget.Width * Main.screenTarget.Height];
            Main.screenTarget.GetData(data);
            texture.SetData(data);

            if (rad == 0 || scale != scaleNote)
            {
                basicEffect = new BasicEffect(Main.graphics.GraphicsDevice);
                vertices = new VertexPositionColorTexture[]
                {
                
                // 顶点
                new VertexPositionColorTexture(new Vector3(-28.8f*scale, -16.2f*scale, 0f), Color.Red, new Vector2(1, 1)),
                new VertexPositionColorTexture(new Vector3(28.8f*scale, -16.2f * scale, 0f), Color.Green, new Vector2(0, 1)),
                new VertexPositionColorTexture(new Vector3(28.8f * scale, 16.2f * scale, 0f), Color.Blue, new Vector2(0, 0)),
                new VertexPositionColorTexture(new Vector3(-28.8f * scale, 16.2f * scale, 0f), Color.Yellow, new Vector2(1, 0)),

                };
                indices = new int[]
                {

                0, 1, 2,
                2, 3, 0,
                };
            }
            if (!Main.gamePaused)
            {
                rad += rotVec * 0.01f;
            }

            basicEffect.Texture = texture;
            basicEffect.TextureEnabled = true;

            worldMatrix = Matrix.Identity;
            cameraPosition = new Vector3((float)Math.Cos(rad) * 50, 0f, (float)Math.Sin(rad) * 50);
            cameraTarget = new Vector3(-(float)Math.Sin(rad), 0f, (float)Math.Cos(rad));
            cameraUpVector = Vector3.Up;
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), (float)Main.screenWidth / (float)Main.screenHeight, 1f, 1000f);
            //添加新的绘制层
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "MyMod: Cube",
                    delegate
                    {
                        DrawCube();
                        return true;
                    },
                    InterfaceScaleType.Game
                ));
            }
            scaleNote = scale;


        }
        //绘制三维平面
        private void DrawCube()
        {
            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };
            Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Main.graphics.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
            }
        }
    }
}