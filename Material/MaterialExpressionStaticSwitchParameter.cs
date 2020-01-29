﻿using JollySamurai.UnrealEngine4.T3D.Parser;
using JollySamurai.UnrealEngine4.T3D.Processor;

namespace JollySamurai.UnrealEngine4.T3D.Material
{
    public class MaterialExpressionStaticSwitchParameter : ParameterNode<bool>
    {
        public ParsedPropertyBag A { get; }
        public ParsedPropertyBag B { get; }

        public MaterialExpressionStaticSwitchParameter(string name, string parameterName, bool defaultValue, ParsedPropertyBag a, ParsedPropertyBag b, int editorX, int editorY)
            : base(name, parameterName, defaultValue, editorX, editorY)
        {
            A = a;
            B = b;
        }
    }

    public class MaterialExpressionStaticSwitchParameterProcessor : NodeProcessor
    {
        public override string Class => "/Script/Engine.MaterialExpressionStaticSwitchParameter";

        public MaterialExpressionStaticSwitchParameterProcessor()
        {
            AddRequiredAttribute("Name", PropertyDataType.String);

            AddRequiredProperty("MaterialExpressionEditorX", PropertyDataType.Integer);
            AddRequiredProperty("MaterialExpressionEditorY", PropertyDataType.Integer);
            AddRequiredProperty("ParameterName", PropertyDataType.String);

            AddOptionalProperty("DefaultValue", PropertyDataType.Boolean);
            AddOptionalProperty("A", PropertyDataType.AttributeList);
            AddOptionalProperty("B", PropertyDataType.AttributeList);

            AddIgnoredProperty("ExpressionGUID");
            AddIgnoredProperty("Material");
            AddIgnoredProperty("MaterialExpressionGuid");
        }

        public override Node Convert(ParsedNode node, Node[] children)
        {
            return new MaterialExpressionStaticSwitchParameter(
                node.FindAttributeValue("Name"),
                node.FindPropertyValue("ParameterName"),
                ValueUtil.ParseBoolean(node.FindPropertyValue("DefaultValue") ?? "False"),
                ValueUtil.ParseAttributeList(node.FindPropertyValue("A")),
                ValueUtil.ParseAttributeList(node.FindPropertyValue("B")),
                ValueUtil.ParseInteger(node.FindPropertyValue("MaterialExpressionEditorX")),
                ValueUtil.ParseInteger(node.FindPropertyValue("MaterialExpressionEditorY"))
            );
        }
    }
}
