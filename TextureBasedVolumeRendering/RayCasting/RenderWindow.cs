using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;

using GLCommon;

namespace RayCasting
{
    class RenderWindow: GameWindow
    {
        string filename = "head256.raw";

        float[] vertices = 
        {
                    0.0f, 0.0f, 0.0f,
                    0.0f, 0.0f, 1.0f,
                    0.0f, 1.0f, 0.0f,
                    0.0f, 1.0f, 1.0f,
                    1.0f, 0.0f, 0.0f,
                    1.0f, 0.0f, 1.0f,
                    1.0f, 1.0f, 0.0f,
                    1.0f, 1.0f, 1.0f};

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
                    4,5,1};

        float ydeg = 0;
        float zdeg = 0;
        float xdeg = 0;

        float stepSize = 0.01f;

        int ElementBufferObject;
        int VertexBufferObject;
        int VertexArrayObject;

        int frameBuffer;

        int backfaceVertShader,backfaceFragShader;
        int raycastingVertShader,raycastingFragShader;
        int program;

        Texture2D exitpointTexture;
        Texture3D volumeTexture;

        //A simple constructor to let us set the width/height/title of the window.
        public RenderWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) {

        }

        private void initShaderObj()
        {
            backfaceVertShader = Utils.CreateShader("backface.vert", ShaderType.VertexShader);
            backfaceFragShader = Utils.CreateShader("backface.frag", ShaderType.FragmentShader);

            raycastingVertShader = Utils.CreateShader("raycasting.vert", ShaderType.VertexShader);
            raycastingFragShader = Utils.CreateShader("raycasting.frag", ShaderType.FragmentShader);

            program = GL.CreateProgram();
        }

        private void attachShader(int vertShader,int fragShader)
        {
            GL.AttachShader(program, vertShader);
            GL.AttachShader(program, fragShader);

            GL.LinkProgram(program);

            string infoLogLink = GL.GetShaderInfoLog(vertShader);
            if (infoLogLink != string.Empty)
                Console.WriteLine(infoLogLink);

            GL.UseProgram(program);

            GL.DetachShader(program, vertShader);
            GL.DetachShader(program, fragShader);
        }

        protected override void OnLoad(EventArgs e)
        {

            GL.ClearColor(0.0f,0.0f,0.0f,1f);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            initShaderObj();

            exitpointTexture = new Texture2D(Width, Height);
            volumeTexture = new Texture3D("head256.raw");

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexArrayObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            int depthBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, Width, Height);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            frameBuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, exitpointTexture.Handle, 0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            ydeg += 1;
            GL.Enable(EnableCap.DepthTest);

            // Pass #1
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.Viewport(0, 0, Width, Height);
            attachShader(backfaceVertShader, backfaceFragShader);
            Render(CullFaceMode.Front);

            // Pass #2
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, Width, Height);
            attachShader(raycastingVertShader, raycastingFragShader);

            rcSetUniforms();
            Render(CullFaceMode.Back);
            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        private void Render(CullFaceMode cullface)
        {

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), (float)Width / Height, 0.1f, 100f);

            Matrix4 view = Matrix4.LookAt(new Vector3(0f,0f,50f),new Vector3(0f,0f,0f), new Vector3(0f,1f,0f));

            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateRotationY(ydeg);
            model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90));
            model *= Matrix4.CreateTranslation(-0.5f, -0.5f, -0.5f);

            Matrix4 transform = projection * view * model;

            int location = getUniformLocation("MVP");

            GL.UniformMatrix4(location, true, ref transform);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(cullface);

            GL.BindVertexArray(VertexArrayObject);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.Disable(EnableCap.CullFace);
        }

        private int getUniformLocation(string uniformName)
        {
            int location = GL.GetUniformLocation(program, uniformName);

            if (location == -1)
            {
                throw new ArgumentException("uniform "+uniformName+"  not found");
            }

            return location;
        }

        private void rcSetUniforms()
        {
            GL.Uniform2(getUniformLocation("ScreenSize"), Width, Height);
            //GL.Uniform1(getUniformLocation("StepSize"), stepSize);

            int exitTexLoc = getUniformLocation("ExitPoints");
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, exitpointTexture.Handle);
            GL.Uniform1(exitTexLoc, 1);

            //int volTexLoc = getUniformLocation("VolumeTex");
            //GL.ActiveTexture(TextureUnit.Texture2);
            //GL.BindTexture(TextureTarget.Texture3D, volumeTexture.Handle);
            //GL.Uniform1(volTexLoc, 2);

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
                zdeg += 5;
            }

            if (input.IsKeyDown(Key.Down))
            {
                zdeg -= 5;
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

            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);

            exitpointTexture.Dispose();
            volumeTexture.Dispose();

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
                vertices[cur + 2] = -0.5f + 1.0f*i / n;

                vertices[cur+3] = -.5f;
                vertices[cur + 4] = .5f;
                vertices[cur + 5] = -0.5f + 1.0f * i / n;

                vertices[cur+6] = .5f;
                vertices[cur + 7] = .5f;
                vertices[cur + 8] = -0.5f + 1.0f * i / n;

                vertices[cur+9] = .5f;
                vertices[cur + 10] = -.5f;
                vertices[cur + 11] = -0.5f + 1.0f * i / n;
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
