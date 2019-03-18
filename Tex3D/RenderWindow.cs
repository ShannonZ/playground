using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL; // OpenGL3
using OpenTK.Input;
using System;
using System.IO;
using System.Text;

namespace Tex3D
{
    class RenderWindow:GameWindow
    {
        int VAO;
        int program;

        int volTex;

        float ydeg = 0.0f;
        float xdeg = 0.0f;

        public RenderWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title){}

        protected override void OnLoad(EventArgs e)
        {
            InitVBO();
            InitVol3DTex("head256.raw");
            InitShaderProgram();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
            base.OnRenderFrame(e);
        }

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

            if (input.IsKeyDown(Key.Left))
            {
                ydeg += 5;
            }

            if (input.IsKeyDown(Key.Right))
            {
                ydeg -= 5;
            }

            if (input.IsKeyDown(Key.Up))
            {
                xdeg += 5;
            }

            if (input.IsKeyDown(Key.Down))
            {
                xdeg -= 5;
            }

            base.OnUpdateFrame(e);
        }

        private void InitVBO()
        {
            float[] vertices =
             {          0f, 0f, 0f,
                        0f, 0f, 1.0f,
                        0f, 1.0f, 0f,
                        0f, 1.0f, 1.0f,
                        1.0f, 0f, 0f,
                        1.0f, 0f, 1.0f,
                        1.0f, 1.0f, 0f,
                        1.0f, 1.0f, 1.0f
            };

            uint[] indices =
                    {
                            1,5,7,
                            7,3,1,
                            0,2,6,
                            6,4,0,
                            0,1,3,
                            3,2,0,
                            7,5,4,
                            4,6,7,
                            2,3,7,
                            7,6,2,
                            1,0,4,
                            4,5,1
            };

            int VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        }

        #region Texture

        private void InitVol3DTex(string path)
        {
            volTex = GL.GenTexture();

            int r = 256;
            int c = 256;
            int p = 109;

            int count = r * c * p;

            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            using (var bw = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                byte[] raw = bw.ReadBytes(count);

                GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Intensity, r, c, p, 0, PixelFormat.Luminance, PixelType.UnsignedByte, raw);
            }

            //using (var bw = new BinaryReader(new FileStream(path, FileMode.Open)))
            //{
            //    byte[] raw = bw.ReadBytes(count);
            //    byte[] rgbabuf = new byte[count * 4];

            //    for (int i = 0; i < count; i++)
            //    {
            //        rgbabuf[4 * i] =
            //        rgbabuf[4 * i + 1] =
            //        rgbabuf[4 * i + 2] =
            //        rgbabuf[4 * i + 3] = raw[i];
            //    }

            //    GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba, r, c, p, 0, PixelFormat.Rgba, PixelType.UnsignedByte, rgbabuf);
        }
        #endregion

        #region Shader
        private void InitShaderProgram()
        {
            int raycastingVertShader = CreateShader("frontface.vert", ShaderType.VertexShader);
            int raycastingFragShader = CreateShader("frontface.frag", ShaderType.FragmentShader);
            program = GL.CreateProgram();
            attachShader(program, raycastingVertShader, raycastingFragShader);
        }

        private void attachShader(int program,int vertShader, int fragShader)
        {
            GL.AttachShader(program, vertShader);
            GL.AttachShader(program, fragShader);

            GL.LinkProgram(program);

            string infoLogLink = GL.GetShaderInfoLog(vertShader);
            if (infoLogLink != string.Empty)
                Console.WriteLine(infoLogLink);
        }

        public static int CreateShader(string Path, ShaderType shaderType)
        {
            string ShaderSource;

            using (StreamReader streamReader = new StreamReader(Path, Encoding.UTF8))
            {
                ShaderSource = streamReader.ReadToEnd();
                ShaderSource += "\0";
            }

            int shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, ShaderSource);
            GL.CompileShader(shader);

            string infoLog = GL.GetShaderInfoLog(shader);
            if (infoLog != string.Empty)
                Console.WriteLine(infoLog);

            return shader;
        }
        #endregion

        private void DrawBox(CullFaceMode mode)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(mode);
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            GL.Disable(EnableCap.CullFace);
        }

        private void Render(CullFaceMode cullFace)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), (float)Width / Height, 0.1f, 400f);
            var view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 2.0f),
                                      new Vector3(0.0f, 0.0f, 0.0f),
                                      new Vector3(0.0f, 1.0f, 0.0f));
            var model = Matrix4.Identity;
            
            model *= Matrix4.CreateTranslation(-0.5f, -0.5f, -0.5f);
            model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(ydeg));
            model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(xdeg));

            var mvp = model * view * projection; // API specific order, not the same as C++ libs

            int loc = GL.GetUniformLocation(program, "MVP");
            if (loc>=0)
            {
                GL.UniformMatrix4(loc,false, ref mvp);
            }
            else
            {
                Console.WriteLine("Uniform MVP not found");
            }

            DrawBox(cullFace);
        }

        private void Display()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, Width, Height);
            GL.Enable(EnableCap.Texture3DExt);
            GL.UseProgram(program);
            SetUniforms();  // Transfer Texture in OfflineBuf
            Render(CullFaceMode.Back);
            GL.UseProgram(0);

            Context.SwapBuffers();
        }

        private void SetUniforms()
        {

            int volloc = GL.GetUniformLocation(program, "VolumeTex");
            if (volloc >= 0)
            {
                GL.ActiveTexture(TextureUnit.Texture2);
                GL.BindTexture(TextureTarget.Texture3D, volTex);
                GL.Uniform1(volloc, 2);
            }
            else
            {
                Console.WriteLine("Uniform VolumeTex not found");
            }
        }
    }
}
