using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    class Camera
    {
        private Vector3 position;
        private Vector3 target;
        public Matrix viewMatrix, projectionMatrix;

        public Camera()
        {
            ResetCamera();
        }

        public void ResetCamera()
        {
            position = new Vector3(0, -3, 3);
            target = new Vector3(0,3,0);

            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 16 / 9, .5f, 500f);
        }

        public void Update()
        {
            UpdateViewMatrix();
        }
        private void UpdateViewMatrix()
        {
            viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up);
        }
    }
}
