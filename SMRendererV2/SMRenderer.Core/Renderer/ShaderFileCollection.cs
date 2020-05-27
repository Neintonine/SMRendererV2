﻿using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using SMRenderer.Core.Enums;
using SMRenderer.Core.Exceptions;

namespace SMRenderer.Core.Renderer
{
    /// <include file='renderer.docu' path='Documentation/ShaderFileCollection/Class'/>
    public class ShaderFileCollection : List<ShaderFile>
    {
        #region Fields
        /// <include file='renderer.docu' path='Documentation/ShaderFileCollection/Fields/Field[@name="InDictionary"]'/>
        public List<string> InDictionary { get; private set; } = new List<string>();
        /// <include file='renderer.docu' path='Documentation/ShaderFileCollection/Fields/Field[@name="OutDictionary"]'/>
        public List<string> OutDictionary { get; private set; } = new List<string>();

        public ShaderType Type;

        private Action<Dictionary<string, Uniform>> SetUniforms;

        public string Name = "Shaders";
        #endregion

        #region Constructors

        public ShaderFileCollection(ShaderType type) : this(type, a => { })
        { }
        public ShaderFileCollection(ShaderType type, Action<Dictionary<string, Uniform>> setUniformAction)
        {
            Type = type;
            SetUniforms = setUniformAction;
        }

        #endregion

        #region Public Instance Methods

        public void Add(string source, bool individual = true)
        {
            base.Add(new ShaderFile(Type, source, individual));
        }
        public void Add(ShaderFileCollection fileCollection)
        {
            if (fileCollection.Type != Type)
                throw new ShaderLoadingException($"The file collection-type, that you tried to add, doesn't match the shader type. \n\nInstance type: {Type}\nCollectionType: {fileCollection.Type}");
            foreach (ShaderFile shaderFile in fileCollection) Add(shaderFile);
        }
        /// <include file='renderer.docu' path='Documentation/ShaderFileCollection/Methods/Method[@name="Load"]'/>
        public void Load(int programId)
        {
            foreach (ShaderFile file in this)
            {
                file.Load(programId);

                InDictionary = InDictionary.Concat(file.InDictionary).ToList();
                OutDictionary = OutDictionary.Concat(file.OutDictionary).ToList();
            }
        }

        public void SetUniform(Dictionary<string, Uniform> Uniforms)
        {
            SetUniforms(Uniforms);
        }
        #endregion
    }
}