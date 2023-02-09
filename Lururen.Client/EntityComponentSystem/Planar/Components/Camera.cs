﻿using Lururen.Client.EntityComponentSystem.Planar;
using Lururen.Client.EntityComponentSystem.Planar.Systems;
using Lururen.Client.Graphics.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace Lururen.Client.EntityComponentSystem.Planar.Components
{
    public class Camera : Component2D
    {
        public Camera()
        {
            Camera2DSystem.GetInstance().Register(this);
        }

        public Vector2i ViewportSize { get; private set; }

        public void SetViewportSize(int width, int height)
        {
            ViewportSize = new Vector2i(width, height);
        }

        public void ResetViewportSize()
        {
            ViewportSize = Renderer2D.WindowSize;
        }

        public override void Init(Entity ent)
        {
            base.Init(ent);
            ViewportSize = Renderer2D.WindowSize;
        }

        public override void Update(double deltaTime)
        {
            ViewportSize = Renderer2D.WindowSize;
            GL.Viewport(0, 0, ViewportSize.X, ViewportSize.Y);
        }

        public static Camera? GetActiveCamera()
        {
            // Check for active instead
            return Camera2DSystem.GetInstance().Cameras.Find(x => x.GetType() == typeof(Camera));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Vector2 GetPositionCorrector()
        {
            // Do not change. Works twice as fast this way
            return new Vector2(-Transform.Position.X + ViewportSize.X / 2, -Transform.Position.Y + ViewportSize.Y / 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal float GetRotationCorrector()
        {
            return -Transform.Rotation;
        }
    }
}