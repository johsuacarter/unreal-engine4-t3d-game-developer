﻿using JollySamurai.UnrealEngine4.T3D.Common;
using JollySamurai.UnrealEngine4.T3D.Processor;

namespace JollySamurai.UnrealEngine4.T3D.Material
{
    public abstract class VectorConstantNode : Node
    {
        public Vector4 Constant { get; }

        public VectorConstantNode(string name, Vector4 constant, int editorX, int editorY)
            : base(name, editorX, editorY)
        {
            Constant = constant;
        }
    }

    public abstract class VectorConstantProcessor : ObjectNodeProcessor
    {
        public VectorConstantProcessor()
        {
            AddRequiredAttribute("Name", PropertyDataType.String);

            AddOptionalProperty("Constant", PropertyDataType.Vector4);
            AddOptionalProperty("MaterialExpressionEditorX", PropertyDataType.Integer);
            AddOptionalProperty("MaterialExpressionEditorY", PropertyDataType.Integer);

            AddIgnoredProperty("Material");
            AddIgnoredProperty("MaterialExpressionGuid");
        }
    }
}
