using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;

using GLCommon;

namespace TextureBasedVolumeRendering
{
    class RenderWindow: GameWindow
    {
        string filename = "head256.raw";

        float[] vertices;

        uint[] indices;

        int ElementBufferObject;
        int VertexBufferObject;
        int VertexArrayObject;

        Shader shader;
        Texture texture;

        //A simple constructor to let us set the width/height/title of the window.
        public RenderWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.0f,0.0f,0.0f,1f);

            generateIndices(109);
            generateVertices(109);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();

            texture = new Texture(filename);
            texture.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexArrayObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            //Because there's now 5 floats between the start of the first vertex and the start of the second,
            //we modify this from 3 * sizeof(float) to 5 * sizeof(float).
            //This will now pass the new vertex array to the buffer.
            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Matrix4 transform = Matrix4.Identity;
            transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(5));
            transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(5));
            transform *= Matrix4.CreateTranslation(0.1f, 0.1f, 0.1f);

            //Now that the matrix is finished, pass it to the vertex shader.
            //Go over to shader.vert to see how we finally apply this to the vertices
            shader.SetMatrix4("transform", transform);
            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(VertexArrayObject);

            texture.Use();
            shader.Use();

            GL.DrawElements(PrimitiveType.Quads, indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        //This function runs on every update frame.
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //Get the current state of the keyboard on this frame.
            KeyboardState input = Keyboard.GetState();

            //Check if the Escape button is currently being pressed.
            if (input.IsKeyDown(Key.Escape))
            {
                //If it is, exit the window.
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }


        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            //GL.DeleteBuffer(VertexBufferObject);
            //GL.DeleteVertexArray(VertexArrayObject);

            shader.Dispose();
            texture.Dispose();
            base.OnUnload(e);
        }

        private void generateVertices(int n)
        {
            vertices = new float[n * 3 * 4];
            int cur;
            for (int i = 0; i < n; i++)
            {
                cur = 3 * 4 * i;

                vertices[cur] = -.5f;
                vertices[cur + 1] = -.5f;
                vertices[cur + 2] = -0.5f + i / n;

                vertices[cur+3] = -.5f;
                vertices[cur + 4] = .5f;
                vertices[cur + 5] = -0.5f + i / n;

                vertices[cur+6] = .5f;
                vertices[cur + 7] = .5f;
                vertices[cur + 8] = -0.5f + i / n;

                vertices[cur+9] = .5f;
                vertices[cur + 10] = -.5f;
                vertices[cur + 11] = -0.5f + i / n;
            }
        }

        private void generateIndices(int n)
        {
            indices = new uint[n * 4];
            for (uint i = 0; i < n*4; i++)
            {
                indices[i] = i;
            }
        }
    }
}
