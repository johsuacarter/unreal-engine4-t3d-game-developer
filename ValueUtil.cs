﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace JollySamurai.UnrealEngine4.T3D
{
    public class ValueUtil
    {
        public static readonly Regex ExpressionReferenceRegex = new Regex(@"(?<type>[a-zA-Z0-9]+)'""(?<material>\w+:)?(?<object>.+)""'", RegexOptions.Compiled);
        public static readonly Regex ResourceReferenceRegex = new Regex(@"(\w+)'""([\w\/\.]+)""'", RegexOptions.Compiled);
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
            return float.Parse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
        }

        public static SamplerType TryParseSamplerType(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;
                return ParseSamplerType(value);
            } catch (Exception /* FIXME: replace with named exception */) {
                successOrFailure = false;
            }

            return SamplerType.Unknown;
        }

        public static SamplerType ParseSamplerType(string value)
        {
            if (value == null) {
                return SamplerType.Default;
            } else if (value == "SAMPLERTYPE_Normal") {
                return SamplerType.Normal;
            }

            // FIXME:
            throw new Exception("unhandled sampler type");
        }

        public static UnresolvedExpressionReference TryParseExpressionReference(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;
                return ParseExpressionReference(value);
            } catch (Exception /* FIXME: replace with named exception */) {
                successOrFailure = false;
            }

            return null;
        }

        public static UnresolvedExpressionReference ParseExpressionReference(string value)
        {
            if (value == null) {
                return null;
            }

            ParsedPropertyBag propertyBag;
            string expression;
            
            if (value.StartsWith("(") && value.EndsWith(")")) {
                Parser parser = new Parser(value.Substring(1, value.Length - 2));

                if (parser.PeekToken() == "ExpressionInputId") {
                    parser.ExpectToken("ExpressionInputId");
                    parser.ExpectToken("=");
                    parser.ReadToken();
                    parser.ExpectToken("Input");
                    parser.ExpectToken("=");

                    var inputValue = parser.ReadUntilEndOfLine();
                    
                    parser = new Parser(inputValue.Substring(1, inputValue.Length - 2));
                }

                parser.ExpectToken("Expression");
                parser.ExpectToken("=");

                expression= parser.ReadToken();
                propertyBag = parser.ReadAttributeList();
            } else {
                expression = value;
                propertyBag = ParsedPropertyBag.Empty;
            }
            
            Match match = ExpressionReferenceRegex.Match(expression);

            if (! match.Success) {
                // FIXME: replace with named exception
                throw new Exception("Failed to parse UnresolvedExpressionReference");
            }

            return new UnresolvedExpressionReference(match.Groups["type"].Value, match.Groups["object"].Value, propertyBag);
        }

        public static TextureReference TryParseTextureReference(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;
                return ParseTextureReference(value);
            } catch (Exception /* FIXME: replace with named exception */) {
                successOrFailure = false;
            }

            return null;
        }

        public static TextureReference ParseTextureReference(string value)
        {
            Match match = ResourceReferenceRegex.Match(value);

            if (! match.Success) {
                // FIXME: replace with named exception
                throw new Exception("Failed to parse TextureReference");
            }

            var type = TextureType.Texture2D;

            if (match.Groups[1].Value == "Texture2D") {
                type = TextureType.Texture2D;
            } else {
                // FIXME: replace with named exception
                throw new Exception("Failed to parse TextureReference");
            }

            return new TextureReference(type, match.Groups[2].Value);
        }

        public static FunctionReference TryParseFunctionReference(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;
                return ParseFunctionReference(value);
            } catch (Exception /* FIXME: replace with named exception */) {
                successOrFailure = false;
            }

            return null;
        }

        public static FunctionReference ParseFunctionReference(string value)
        {
            Match match = ResourceReferenceRegex.Match(value);

            if (! match.Success) {
                // FIXME: replace with named exception
                throw new Exception("Failed to parse FunctionReference");
            }

            return new FunctionReference(match.Groups[1].Value, match.Groups[2].Value);
        }

        public static Vector4 TryParseVector4(string value, out bool successOrFailure)
        {
            try {
                successOrFailure = true;
                return ParseVector4(value);
            } catch (Exception /* FIXME: replace with named exception */) {
                successOrFailure = false;
            }

            return default(Vector4);
        }

        public static Vector4 ParseVector4(string value)
        {
            Match match = Vector4Regex.Match(value);

            if (! match.Success) {
                // FIXME: replace with named exception
                throw new Exception("Failed to parse Vector4");
            }

            return new Vector4(ParseFloat(match.Groups[1].Value), ParseFloat(match.Groups[2].Value), ParseFloat(match.Groups[3].Value), ParseFloat(match.Groups[4].Value));
        }

        public static UnresolvedExpressionReference[] ParseExpressionReferenceArray(ParsedProperty[] elements)
        {
            List<UnresolvedExpressionReference> list = new List<UnresolvedExpressionReference>();

            foreach (var parsedProperty in elements) {
                list.Add(ParseExpressionReference(parsedProperty.Value));
            }

            return list.ToArray();
        }
    }
}
