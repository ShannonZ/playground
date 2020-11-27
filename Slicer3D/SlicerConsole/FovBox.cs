using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Slicer3D.Common;
using System;

namespace SlicerConsole
{
    class FovBox : IDisposable
    {
        private bool _initialized;
        private int VBO;
        private int EBO;
        private int VAO;
        private Matrix4 MVP;
        private Vector4 EdgeColor;
        private Shader shader;
        public FovBox(float x, float y, float z)
        {
            InitBoxVAO(x, y, z);
            MVP = Matrix4.Identity;
            EdgeColor = new Vector4(1, 0, 0, 1);
            _initialized = true;
        }

        public void SetEdgeColor(float R, float G, float B, float A)
        {
            EdgeColor = new Vector4(R, G, B, A);
        }

        public void SetMVP(Matrix4 mat)
        {
            MVP = mat;
        }

        public void Bind()
        {
            shader.Use();
            GL.BindVertexArray(VAO);
        }

        public void Render()
        {
            Bind();
            shader.SetVector4("edgeColor",EdgeColor);
            shader.SetMatrix4("MVP", MVP);
            GL.DrawElements(BeginMode.Lines, 24, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_initialized)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    GL.BindVertexArray(0);
                    GL.UseProgram(0);

                    GL.DeleteBuffer(VBO);
                    GL.DeleteBuffer(EBO);
                    GL.DeleteVertexArray(VAO);
                    GL.DeleteProgram(shader.Handle);

                    _initialized = false;
                }
            }
        }

        private float[] InitVertex(float x, float y, float z)
        {
            return new float[]
            {
                -x/2, -y/2, -z/2,
                -x/2, -y/2, z/2,
                -x/2, y/2, -z/2,
                -x/2, y/2, z/2,
                x/2, -y/2, -z/2,
                x/2, -y/2, z/2,
                x/2, y/2, -z/2,
                x/2, y/2, z/2
            };
        }

        private void InitBoxVAO(float R, float C, float S)
        {
            float[] vertices = InitVertex(R, C, S);

            uint[] indices = { 0, 1, 1, 5, 5, 4, 4, 0, 2, 3, 3, 7, 7, 6, 6, 2, 2, 0, 6, 4, 7, 5, 3, 1 };

            if (VBO> 0)
            {
                GL.DeleteBuffer(VBO);
            }
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            if (EBO > 0)
            {
                GL.DeleteBuffer(EBO);
            }
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader = new Shader(@"Shaders\fovbox_vert.c", @"Shaders\fovbox_frag.c");
            shader.Use();
            if (VAO > 0)
            {
                GL.DeleteVertexArray(VAO);
            }
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            int loc = shader.GetAttribLocation("pos");
            GL.EnableVertexAttribArray(loc);
            GL.VertexAttribPointer(loc, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }
    }
}
