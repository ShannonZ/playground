using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL; // OpenGL3
using System;
using System.IO;
using System.Text;

namespace RayCasting3
{
    class RenderWindow:GameWindow
    {
        int VAO;
        int passOneProgram, passTwoProgram;
        int OfflineBuf;
        int backfaceTex;
        int volTex;

        public RenderWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title){}

        protected override void OnLoad(EventArgs e)
        {
            InitVBO();
            InitFace2DTex(Width,Height);
            InitVol3DTex("head256.raw");
            InitShaderProgram();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Display();
            base.OnRenderFrame(e);
        }

        private void InitVBO()
        {
            float[] vertices =
                                {  0f, 0f, 0f,
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
        private void InitFace2DTex(int texWidth, int texHeight)
        {
            backfaceTex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, backfaceTex);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f, texWidth, texHeight, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);

        }

        private void InitVol3DTex(string path)
        {
            volTex = GL.GenTexture();

            int r = 256;
            int c = 256;
            int p = 225;

            int count = r*c*p;
            
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);           
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            using (var bw = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                byte[] raw = bw.ReadBytes(count);
                byte[] rgbabuf = new byte[count * 4];

                for (int i = 0; i < count; i++)
                {
                    rgbabuf[4 * i] =
                    rgbabuf[4 * i + 1] =
                    rgbabuf[4 * i + 2] =
                    rgbabuf[4 * i + 3] = raw[i];
                }

                GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba, r, c, p, 0, PixelFormat.Rgba, PixelType.UnsignedByte, rgbabuf);
            }
        }
        #endregion

        #region FrameBuffer
        private void InitFrameBuffer(int backfaceTex,int texWidth, int texHeight)
        {
            int depthBuf = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuf);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, texWidth, texHeight);

            OfflineBuf = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, OfflineBuf);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, backfaceTex, 0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuf);

            if (FramebufferErrorCode.FramebufferComplete!= GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer))
            {
                Console.WriteLine("Frame Buffer Not Complete");
            }
        }

        #endregion

        #region Shader
        private void InitShaderProgram()
        {
            int backfaceVertShader = CreateShader("backface.vert", ShaderType.VertexShader);
            int backfaceFragShader = CreateShader("backface.frag", ShaderType.FragmentShader);
            passOneProgram = GL.CreateProgram();
            attachShader(passOneProgram, backfaceVertShader, backfaceFragShader);

            int raycastingVertShader = CreateShader("raycasting.vert", ShaderType.VertexShader);
            int raycastingFragShader = CreateShader("raycasting.frag", ShaderType.FragmentShader);
            passTwoProgram = GL.CreateProgram();
            attachShader(passTwoProgram, raycastingVertShader, raycastingFragShader);
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
            var view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.1f),
                                      new Vector3(0.0f, 0.0f, 0.0f),
                                      new Vector3(0.0f, 1.0f, 0.0f));
            var model = Matrix4.Identity;
            model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(30));
            model = Matrix4.CreateTranslation(-0.5f, -0.5f, -0.5f);

            var mvp = projection * view * model;


            int loc = GL.GetUniformLocation(cullFace == CullFaceMode.Back ? passTwoProgram : passOneProgram, "MVP");
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
            GL.Enable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, OfflineBuf);
            GL.Viewport(0,0,Width, Height);
            GL.UseProgram(passOneProgram);

            Render(CullFaceMode.Front);
            GL.UseProgram(0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, Width, Height);
            GL.UseProgram(passTwoProgram);
            SetUniforms();
            Render(CullFaceMode.Back);
            GL.UseProgram(0);

            Context.SwapBuffers();
        }

        private void SetUniforms()
        {
            int loc = GL.GetUniformLocation(passTwoProgram, "ScreenSize");
            if (loc >= 0)
            {
                GL.Uniform2(loc, Width,Height);
            }
            else
            {
                Console.WriteLine("Uniform ScreenSize not found");
            }

            int backfaceloc = GL.GetUniformLocation(passTwoProgram, "ExitPoints");
            if (backfaceloc>=0)
            {
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, backfaceTex);
                GL.Uniform1(backfaceloc, 1);
            }
            else
            {
                Console.WriteLine("Uniform ExitPoints not found");
            }
        }
    }
}
