using OpenTK.Graphics.OpenGL4;
using System;
using System.Windows;

namespace Slicer3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int VAO_Box;
        private int VBO_Box;
        private int EBO_Box;
        private Shader shader_Box;
        public MainWindow()
        {
            InitializeComponent();

            var settings = new OpenTK.Wpf.GLWpfControlSettings();
            settings.MajorVersion = 4;
            settings.MinorVersion = 5;
            OpenTkControl.Start(settings);
        }

        private void OpenTkControl_Render(TimeSpan obj)
        {
            GL.ClearColor(0.2f, 0.3f, 0.2f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            DrawBox();
        }

        private void DrawBox()
        {
            shader_Box.Use();
            GL.BindVertexArray(VAO_Box);
            GL.DrawArrays(PrimitiveType.Lines, 0, 12);
        }

        private float[] InitVertex(float x, float y, float z)
        {
            return new float[]
            {
                0f, 0f, 0f,
                0f, 0f, z,
                0f, y, 0f,
                0f, y, z,
                x, 0f, 0f,
                x, 0f, z,
                x, y, 0f,
                x, y, z
            };
        }

        private void InitBoxVAO(float R, float C, float S)
        {
            float MAX = Math.Max(Math.Max(R, C), S);

            var Vertx = R / MAX;
            var Verty = C / MAX;
            var Vertz = S / MAX;
            float[] vertices = InitVertex(Vertx, Verty, Vertz);

            uint[] indices = { 0, 1, 1, 5, 5, 4, 4, 0, 2, 3, 3, 7, 7, 6, 6, 2, 2, 0, 6, 4, 7, 5, 3, 1 };

            if (VBO_Box > 0)
            {
                GL.DeleteBuffer(VBO_Box);
            }
            VBO_Box = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_Box);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            if (EBO_Box > 0)
            {
                GL.DeleteBuffer(EBO_Box);
            }
            EBO_Box = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO_Box);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader_Box = new Shader(@"Shaders\box_shader.vert", @"Shaders\box_shader.frag");
            shader_Box.Use();
            if (VAO_Box > 0)
            {
                GL.DeleteVertexArray(VAO_Box);
            }
            VAO_Box = GL.GenVertexArray();
            GL.BindVertexArray(VAO_Box);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VAO_Box);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO_Box);
            int loc = shader_Box.GetAttribLocation("pos");
            GL.EnableVertexAttribArray(loc);
            GL.VertexAttribPointer(loc, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        private void OpenTkControl_Ready()
        {
            InitBoxVAO(1f, 1f, 1f);
        }
    }
}
