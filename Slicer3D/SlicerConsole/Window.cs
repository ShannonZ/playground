using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlicerConsole
{
    class Window:GameWindow
    {
        private FovBox box;

        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.Default)
        {
            CanRot = false;
            rotation = Matrix3.Identity;
            cameraPos = 2f;
            rotSpeed = 1;
        }

        float FovX;
        float FovY;
        float FovZ;
        protected override void OnLoad(EventArgs e)
        {
            FovX = 100;
            FovY = 100;
            FovZ = 100;
            cameraPos = -FovZ;
            box = new FovBox(FovX, FovY, FovZ);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            base.OnLoad(e);
        }

        // Rotation
        private bool CanRot;
        private Vector3 LastPos;
        private float rotSpeed;
        private Matrix3 rotation;
        float cameraPos;
        private Matrix4 MVP;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            CanRot = true;
            LastPos = MapToSphere(e.Position);

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            CanRot = false;
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (!CanRot)
            {
                return;
            }

            Quaternion result = Quaternion.Identity;
            var CurPos = MapToSphere(e.Position);

            //Compute the vector perpendicular to the begin and end vectors
            Vector3 Perp = Vector3.Cross(LastPos, CurPos);

            //Compute the length of the perpendicular vector
            if (Perp.Length > double.Epsilon)
            //if its non-zero
            {
                //We're ok, so return the perpendicular vector as the transform after all
                result.X = Perp.X;
                result.Y = Perp.Y;
                result.Z = Perp.Z;
                //In the quaternion values, w is cosine (theta / 2), where theta is the rotation angle
                result.W = Vector3.Dot(LastPos, CurPos) * rotSpeed;
            }

            Matrix3 rot = Matrix3.CreateFromQuaternion(result);
            rotation *= rot;

            var P = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), (float)Width / Height, FovZ/4, FovZ*3);
            var position = new Vector3(0.0f, 0.0f, cameraPos);
            position = rotation * position;

            var V = Matrix4.LookAt(position,
                                   new Vector3(0.0f, 0.0f, 0.0f),
                                   rotation * (new Vector3(0.0f, 1.0f, 0.0f)));

            var M = Matrix4.Identity;
            //M *= Matrix4.CreateTranslation(1, 0.5f, 0.5f);

            MVP = M*V*P;
            box.Bind();
            box.SetMVP(MVP);
            base.OnMouseMove(e);
        }

        private Vector3 MapToSphere(Point point)
        {
            Vector3 result = new Vector3();

            Point tempPoint = new Point(point.X, point.Y);

            //Adjust point coords and scale down to range of [-1 ... 1]
            result.X = (float)((2.0 * point.X - Width) / Width);
            result.Y = (float)((Height - 2.0 * point.Y) / Height);

            //Compute square of the length of the vector from this point to the center
            var length = (result.X * result.X) + (result.Y * result.Y);

            //If the point is mapped outside the sphere... (length > radius squared)
            if (length > 1)
            {
                length = 1;
            }
            result.Z = (float)Math.Sqrt(1 - length);
            result = result.Normalized();

            return result;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            box.Render();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            base.OnUpdateFrame(e); 
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
    }
}
