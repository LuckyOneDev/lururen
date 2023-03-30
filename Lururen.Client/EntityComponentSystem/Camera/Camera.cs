using Lururen.Client.Audio.Generic;
using Lururen.Client.EntityComponentSystem.Base;
using Lururen.Client.EntityComponentSystem.Sound;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace Lururen.Client.EntityComponentSystem.Camera
{
    /// <summary>
    /// Provides camera abstraction in 2D context.
    /// </summary>
    public sealed class Camera : Component
    {
        public SoundListener SoundListener;

        public Camera(Entity entity) : base(entity)
        {
            Register(this);
            SoundListener = new SoundListener(entity);
        }

        /// <summary>
        /// Current visible part of the screen dimentions.
        /// </summary>
        public Vector2i ViewportSize { get; set; }

        public override void Init()
        {
        }

        public override void Update(double deltaTime)
        {
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

        public override void Dispose()
        {
            Unregister(this);
            base.Dispose();
            SoundListener.Dispose();
        }
    }
}