﻿﻿using System.Linq;
 using JollySamurai.UnrealEngine4.T3D.Parser;
 using JollySamurai.UnrealEngine4.T3D.Processor;

 namespace JollySamurai.UnrealEngine4.T3D.Material
{
    public class Material : Node
    {
        public ShadingModel ShadingModel { get; }
        public ExpressionReference BaseColor { get; }
        public ExpressionReference Metallic { get; }
        public ExpressionReference Normal { get; }
        public ExpressionReference Roughness { get; }
        public ExpressionReference Specular { get; }
        public ExpressionReference EmissiveColor { get; }
        public ExpressionReference[] Expressions { get; }

        public Material(Node[] children, string name, ShadingModel shadingModel, ExpressionReference baseColor, ExpressionReference metallic, ExpressionReference normal, ExpressionReference roughness, ExpressionReference specular, ExpressionReference emissiveColor, ExpressionReference[] expressionReferences, int editorX, int editorY)
            : base(name, editorX, editorY, children)
        {
            ShadingModel = shadingModel;
            BaseColor = baseColor;
            Metallic = metallic;
            Normal = normal;
            Roughness = roughness;
            Specular = specular;
            EmissiveColor = emissiveColor;
            Expressions = expressionReferences;
        }

        public Node ResolveExpressionReference(ExpressionReference reference)
        {
            if (null == reference) {
                return null;
            }

            return Children.SingleOrDefault(node => node.Name == reference.NodeName && node.IsClassOf(reference.ClassName));
        }
    }

    public class MaterialProcessor : NodeProcessor
    {
        public override string Class {
            get { return "/Script/Engine.Material"; }
        }

        public MaterialProcessor() : base()
        {
            AddRequiredAttribute("Name", PropertyDataType.String);

            AddRequiredProperty("EditorX", PropertyDataType.Integer);
            AddRequiredProperty("EditorY", PropertyDataType.Integer);
            AddRequiredProperty("Expressions", PropertyDataType.ExpressionReference | PropertyDataType.Array);

            AddOptionalProperty("BaseColor", PropertyDataType.ExpressionReference);
            AddOptionalProperty("EmissiveColor", PropertyDataType.ExpressionReference);
            AddOptionalProperty("Metallic", PropertyDataType.ExpressionReference);
            AddOptionalProperty("Normal", PropertyDataType.ExpressionReference);
            AddOptionalProperty("Roughness", PropertyDataType.ExpressionReference);
            AddOptionalProperty("ShadingModel", PropertyDataType.ShadingModel);
            AddOptionalProperty("Specular", PropertyDataType.ExpressionReference);

            AddIgnoredProperty("LightingGuid");
            AddIgnoredProperty("ReferencedTextureGuids");
            AddIgnoredProperty("StateId");
            AddIgnoredProperty("ThumbnailInfo");
        }

        public override Node Convert(ParsedNode node, Node[] children)
        {
            ParsedProperty expressionList = node.FindProperty("Expressions");

            return new Material(
                children,
                node.FindAttributeValue("Name"),
                ValueUtil.ParseShadingModel(node.FindPropertyValue("ShadingModel")),
                ValueUtil.ParseExpressionReference(node.FindPropertyValue("BaseColor")),
                ValueUtil.ParseExpressionReference(node.FindPropertyValue("Metallic")),
                ValueUtil.ParseExpressionReference(node.FindPropertyValue("Normal")),
                ValueUtil.ParseExpressionReference(node.FindPropertyValue("Roughness")),
                ValueUtil.ParseExpressionReference(node.FindPropertyValue("Specular")),
                ValueUtil.ParseExpressionReference(node.FindPropertyValue("EmissiveColor")),
                ValueUtil.ParseExpressionReferenceArray(node.FindProperty("Expressions").Elements),
                ValueUtil.ParseInteger(node.FindPropertyValue("EditorX")),
                ValueUtil.ParseInteger(node.FindPropertyValue("EditorX"))
            );
        }
    }
}
