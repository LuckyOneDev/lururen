using Lururen.Client.Audio.Generic;
using Lururen.Client.EntityComponentSystem.Generic;
using Lururen.Client.EntityComponentSystem.Planar.Systems;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace Lururen.Client.EntityComponentSystem.Components
{
    /// <summary>
    /// Provides camera abstraction in 2D context.
    /// </summary>
    public class Camera : Component
    {
        public ALSoundDevice SoundDevice;

        public Camera(Entity entity) : base(entity)
        {
            SoundDevice = new ALSoundDevice();
        }

        /// <summary>
        /// Current visible part of the screen dimentions.
        /// </summary>
        public Vector2i ViewportSize { get; set; }

        /// <summary>
        /// Sets viewport size to screen size.
        /// </summary>
        public void ResetViewportSize()
        {
            ViewportSize = SpriteRenderSystem.WindowSize;
        }

        public override void Init<T>(ISystem<T> system)
        {
            base.Init(system);
            ViewportSize = SpriteRenderSystem.WindowSize;
            SoundDevice.SetPosition(new Vector3(Transform.Position));
        }

        public override void Update(double deltaTime)
        {
            ViewportSize = SpriteRenderSystem.WindowSize;
            GL.Viewport(0, 0, ViewportSize.X, ViewportSize.Y);
            SoundDevice.SetPosition(new Vector3(Transform.Position));
        }

        /// <summary>
        /// Gets first active camera. Note that if there are more than one active camera
        /// it may return any of active cameras.
        /// </summary>
        /// <returns></returns>
        public static Camera? GetActiveCamera()
        {
            // Check for active instead
            //return Camera2DSystem.GetInstance().Cameras.Find(x => x.GetType() == typeof(Camera2D));
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Vector3 GetPositionCorrector()
        {
            // Do not change. Works twice as fast this way
            return new Vector3(-Transform.Position.X + ViewportSize.X / 2, -Transform.Position.Y + ViewportSize.Y / 2, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal float GetRotationCorrector()
        {
            return -Transform.Rotation;
        }
    }
}