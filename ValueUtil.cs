﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using JollySamurai.UnrealEngine4.T3D.Exception;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace JollySamurai.UnrealEngine4.T3D
{
    public static class ValueUtil
    {
        public static readonly Regex ExpressionReferenceRegex = new Regex(@"(?<type>[a-zA-Z0-9]+)'""(?<material>\w+:)?(?<object>.+)""'", RegexOptions.Compiled);
        public static readonly Regex ResourceReferenceRegex = new Regex(@"(\w+)'""(.+)""'", RegexOptions.Compiled);
        public static readonly Regex Vector4Regex = new Regex(@"\(R=([0-9]+\.[0-9]+),G=([0-9]+\.[0-9]+),B=([0-9]+\.[0-9]+),A=([0-9]+\.[0-9]+)\)", RegexOptions.Compiled);

        public static bool ParseBoolean(string value)
        {
            return Boolean.Parse(value);
        }

        public static bool TryParseBoolean(string value, out bool successOrFailure)
        {
            successOrFailure = Boolean.TryParse(value, out var result);

            return result;
        }

        public static int TryParseInteger(string value, out bool successOrFailure)
        {
            successOrFailure = int.TryParse(value, out var result);

            return result;
        }

        public static int ParseInteger(string value)
        {
            return int.Parse(value);
        }

        public static float TryParseFloat(string value, out bool successOrFailure)
        {
            successOrFailure = float.TryParse(value, out var result);

            return result;
        }

        public static float ParseFloat(string value)
        {
            return float.Parse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign);
        }

        public static SamplerType TryParseSamplerType(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;

                return ParseSamplerType(value);
            } catch (ValueException) {
                successOrFailure = false;
            }

            return SamplerType.Unknown;
        }

        public static SamplerType ParseSamplerType(string value)
        {
            if (value == null) {
                return SamplerType.Default;
            }

            switch (value) {
                case "SAMPLERTYPE_Color":
                    return SamplerType.Color;
                case "SAMPLERTYPE_Grayscale":
                    return SamplerType.Grayscale;
                case "SAMPLERTYPE_Alpha":
                    return SamplerType.Alpha;
                case "SAMPLERTYPE_Normal":
                    return SamplerType.Normal;
                case "SAMPLERTYPE_Masks":
                    return SamplerType.Masks;
                case "SAMPLERTYPE_DistanceFieldFont":
                    return SamplerType.DistanceFieldFont;
                case "SAMPLERTYPE_LinearColor":
                    return SamplerType.LinearColor;
                case "SAMPLERTYPE_LinearGrayscale":
                    return SamplerType.LinearGrayscale;
                case "SAMPLERTYPE_Data":
                    return SamplerType.Data;
                case "SAMPLERTYPE_External":
                    return SamplerType.External;
                case "SAMPLERTYPE_VirtualColor":
                    return SamplerType.VirtualColor;
                case "SAMPLERTYPE_VirtualGrayscale":
                    return SamplerType.VirtualGrayscale;
                case "SAMPLERTYPE_VirtualAlpha":
                    return SamplerType.VirtualAlpha;
                case "SAMPLERTYPE_VirtualNormal":
                    return SamplerType.VirtualNormal;
                case "SAMPLERTYPE_VirtualMasks":
                    return SamplerType.VirtualMasks;
                case "SAMPLERTYPE_VirtualLinearColor":
                    return SamplerType.VirtualLinearColor;
                case "SAMPLERTYPE_VirtualLinearGrayscale":
                    return SamplerType.VirtualLinearGrayscale;
            }

            throw new ValueException("Unexpected sampler type: " + value);
        }

        public static BlendMode TryParseBlendMode(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;

                return ParseBlendMode(value);
            } catch (ValueException) {
                successOrFailure = false;
            }

            return BlendMode.Unknown;
        }

        public static BlendMode ParseBlendMode(string value)
        {
            if (value == null || value == "BLEND_Opaque") {
                return BlendMode.Opaque;
            } else if (value == "BLEND_Additive") {
                return BlendMode.Additive;
            } else if (value == "BLEND_Masked") {
                return BlendMode.Masked;
            } else if (value == "BLEND_Modulate") {
                return BlendMode.Modulate;
            } else if (value == "BLEND_Translucent") {
                return BlendMode.Translucent;
            }

            throw new ValueException("Unexpected blend mode: " + value);
        }

        public static ShadingModel TryParseShadingModel(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;

                return ParseShadingModel(value);
            } catch (ValueException) {
                successOrFailure = false;
            }

            return ShadingModel.Unknown;
        }

        public static ShadingModel ParseShadingModel(string value)
        {
            if (value == null) {
                return ShadingModel.DefaultLit;
            } else if (value == "MSM_Unlit") {
                return ShadingModel.Unlit;
            }

            throw new ValueException("Unexpected shading model: " + value);
        }

        public static ExpressionReference TryParseExpressionReference(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;

                return ParseExpressionReference(value);
            } catch (ValueException) {
                successOrFailure = false;
            }

            return null;
        }

        public static ExpressionReference ParseExpressionReference(string value)
        {
            if (value == null) {
                return null;
            }

            Match match = ExpressionReferenceRegex.Match(value);

            if (! match.Success) {
                throw new ValueException("Failed to parse ExpressionReference");
            }

            return new ExpressionReference(match.Groups["type"].Value, match.Groups["object"].Value);
        }

        public static ParsedPropertyBag ParseAttributeList(string value)
        {
            if (value == null) {
                return null;
            }

            if (! value.StartsWith("(") || ! value.EndsWith(")")) {
                throw new ValueException("Attribute list should begin and end with parenthesis");
            }

            var parser = new DocumentParser(value.Substring(1, value.Length - 2));

            return parser.ReadAttributeList();
        }

        public static ParsedPropertyBag TryParseAttributeList(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;

                return ParseAttributeList(value);
            } catch (ValueException) {
                successOrFailure = false;
            }

            return null;
        }

        public static TextureReference TryParseTextureReference(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;

                return ParseTextureReference(value);
            } catch (ValueException) {
                successOrFailure = false;
            }

            return null;
        }

        public static TextureReference ParseTextureReference(string value)
        {
            Match match = ResourceReferenceRegex.Match(value);

            if (! match.Success) {
                throw new ValueException("Failed to parse TextureReference");
            }

            var type = TextureType.Texture2D;

            if (match.Groups[1].Value == "Texture2D") {
                type = TextureType.Texture2D;
            } else {
                throw new ValueException("Failed to parse TextureReference");
            }

            return new TextureReference(type, match.Groups[2].Value);
        }

        public static FunctionReference TryParseFunctionReference(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;

                return ParseFunctionReference(value);
            } catch (ValueException) {
                successOrFailure = false;
            }

            return null;
        }

        public static FunctionReference ParseFunctionReference(string value)
        {
            Match match = ResourceReferenceRegex.Match(value);

            if (! match.Success) {
                throw new ValueException("Failed to parse FunctionReference");
            }

            return new FunctionReference(match.Groups[1].Value, match.Groups[2].Value);
        }

        public static Vector4 TryParseVector4(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;

                return ParseVector4(value);
            } catch (ValueException) {
                successOrFailure = false;
            }

            return default(Vector4);
        }

        public static Vector4 ParseVector4(string value)
        {
            Match match = Vector4Regex.Match(value);

            if (! match.Success) {
                throw new ValueException("Failed to parse Vector4");
            }

            return new Vector4(ParseFloat(match.Groups[1].Value), ParseFloat(match.Groups[2].Value), ParseFloat(match.Groups[3].Value), ParseFloat(match.Groups[4].Value));
        }

        public static ExpressionReference[] ParseExpressionReferenceArray(ParsedProperty[] elements)
        {
            if (null == elements) {
                return new ExpressionReference[] {
                };
            }

            List<ExpressionReference> list = new List<ExpressionReference>();

            foreach (var parsedProperty in elements) {
                list.Add(ParseExpressionReference(parsedProperty.Value));
            }

            return list.ToArray();
        }

        public static ParsedPropertyBag[] ParseAttributeListArray(ParsedProperty[] elements)
        {
            if (null == elements) {
                return new ParsedPropertyBag[] {
                };
            }

            List<ParsedPropertyBag> list = new List<ParsedPropertyBag>();

            foreach (var parsedProperty in elements) {
                list.Add(ParseAttributeList(parsedProperty.Value));
            }

            return list.ToArray();
        }
    }
}
