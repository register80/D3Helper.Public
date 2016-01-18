using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;

namespace D3Helper.A_Tools
{
    public class Vector2D
    {
        public Vector2D(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public float X { get; set; }
        public float Y { get; set; }
    }
    class T_World
    {
        public static void ToScreenCoordinate(float X, float Y, float Z, out float RX, out float RY)
        {
            try
            {
                ActorCommonData localACD;
                lock(A_Collection.Me.HeroGlobals.LocalACD ) localACD = A_Collection.Me.HeroGlobals.LocalACD;

                float xd = X - localACD.x0D0_WorldPosX;
                float yd = Y - localACD.x0D4_WorldPosY;
                float zd = Z - localACD.x0D8_WorldPosZ;

                float w = -0.515f * xd - 0.514f * yd - 0.686f * zd + 97.985f;
                if (w < 1.0f) w = 1.0f;
                float rX = (-1.682f * xd + 1.683f * yd + 0.007045f) / w;
                float rY = (-1.540f * xd - 1.539f * yd + 2.307f * zd + 6.161f) / w;

                float a = (float)Engine.Current.VideoPreferences.x0C_DisplayMode.x20_Width / (float)Engine.Current.VideoPreferences.x0C_DisplayMode.x24_Height;
                float D3ClientWindowApect = a * 600.0f / 800.0f;

                rX /= D3ClientWindowApect;

                RX = (rX + 1.0f) / 2.0f * Engine.Current.VideoPreferences.x0C_DisplayMode.x20_Width;
                RY = (1.0f - rY) / 2.0f * Engine.Current.VideoPreferences.x0C_DisplayMode.x24_Height;
            }
            catch { RX = 0; RY = 0; }

        }
    }
}
