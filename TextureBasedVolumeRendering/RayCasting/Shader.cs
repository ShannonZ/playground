using System;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL4;


namespace GLCommon
{
    public static class Utils
    {
        public static int CreateShader(string Path,ShaderType shaderType)
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


    }
}