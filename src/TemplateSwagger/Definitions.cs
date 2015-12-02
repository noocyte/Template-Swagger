using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Swashbuckle.Swagger;
using UXRisk.Lib.TemplateModel.Models;

namespace TemplateSwagger
{
    internal static class Definitions
    {
        internal static IDictionary<string, Schema> GenerateSwaggerDefinitions(IEnumerable<ArticleType> templateTypes)
        {
            var definitions = new Dictionary<string, Schema>();
            var articleProperties = new Dictionary<string, Schema>
            {
                ["id"] = new Schema
                {
                    type = "string",
                    description = "unique identifier for the object",
                },
                ["parentid"] = new Schema
                {
                    type = "string",
                    description = "unique identifier for the parent object",
                },
                ["parenttype"] = new Schema
                {
                    type = "string",
                    description = "type of the parent object",
                }
            };

            var articleRequired = new List<string>
            {
                "id",
                "parentid",
                "parenttype"
            };

            foreach (var templateType in templateTypes)
            {
                var combinedProperties = GenerateTypeProperties(templateType)
                    .Concat(articleProperties)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                var schema = new Schema
                {
                    type = "object",
                    required = articleRequired,
                    properties = combinedProperties
                };
                var type = templateType.Type;

                definitions.Add($"#/definitions/{type}", schema);
            }

            var multiReferenceSchema = new Schema
            {
                type = "object",
                properties = new Dictionary<string, Schema>
                {
                    ["type"] = new Schema
                    {
                        type = "string",
                        description = "type of the reference"
                    },
                    ["values"] = new Schema
                    {
                        type = "array",
                        items = new Schema { type = "string", description = "The unique id of referenced objects" }
                    }
                }
            };

            var singleReferenceSchema = new Schema
            {
                type = "object",
                properties = new Dictionary<string, Schema>
                {
                    ["type"] = new Schema
                    {
                        type = "string",
                        description = "type of the reference"
                    },
                    ["values"] = new Schema
                    {
                        type = "array",
                        maxItems = 1,
                        items = new Schema { type = "string", description = "The unique id of referenced objects" }
                    }
                }
            };

            definitions.Add("#/definitions/MultiReference", multiReferenceSchema);
            definitions.Add("#/definitions/SingleReference", singleReferenceSchema);

            return definitions;
        }

        private static IDictionary<string, Schema> GenerateTypeProperties(ArticleType templateType)
        {
            var typeProperties = new Dictionary<string, Schema>();
            foreach (var field in templateType.Fields)
            {
                typeProperties[field.Name] = new Schema
                {
                    type = ConvertType(field),
                    format = ConvertFormat(field),
                    description = ""
                };

                if (field.Type == DataType.SingleValue )
                    typeProperties[field.Name].@ref = "#/definitions/SingleReference";
                if (field.Type == DataType.MultiValue )
                    typeProperties[field.Name].@ref = "#/definitions/MultiReference";

            }

            return typeProperties;
        }

        private static string ConvertFormat(ArticleField field)
        {
            switch (field.Type)
            {
                case DataType.Text:
                    return "string";
                case DataType.Number:
                    return "double";
                case DataType.Bool:
                    return "boolean";
                case DataType.Date:
                    return "date";
                case DataType.DateTime:
                    return "date-time";
                default:
                    return null;
            }
        }

        private static string ConvertType(ArticleField field)
        {
            switch (field.Type)
            {
                case DataType.Number:
                    return "number";
                case DataType.Bool:
                    return "boolean";
                case DataType.SingleValue:
                case DataType.MultiValue:
                    return "object";
                default:
                    return "string";
            }
        }
    }
}
