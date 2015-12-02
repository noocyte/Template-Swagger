using System.Collections.Generic;
using Swashbuckle.Swagger;
using UXRisk.Lib.TemplateModel.Models;

namespace TemplateSwagger
{
    internal static class Paths
    {
        internal static IDictionary<string, PathItem> GenerateSwaggerPaths(IEnumerable<ArticleType> templateTypes)
        {
            var paths = new Dictionary<string, PathItem>();

            var articleParameters = new List<Parameter>
            {
                new Parameter
                {
                    name = "id",
                    @in = "query",
                    description = "Specify to get only one object",
                    required = false,
                    type = "string",
                    format = "string"
                },
                new Parameter
                {
                    name = "id",
                    @in = "query",
                    description = "Specify to get only one object",
                    required = false,
                    type = "string",
                    format = "string"
                }
            };

            foreach (var templateType in templateTypes)
            {
                var pathItem = new PathItem();
                var type = templateType.Type;

                paths.Add($"/article/{type}", pathItem);
                var get = new Operation
                {
                    summary = $"Get one or more {type}",
                    parameters = articleParameters,
                    responses = new Dictionary<string, Response>
                    {
                        ["200"] = new Response
                        {
                            description = $"A result array of {type}",
                            schema = new Schema
                            {
                                type = "array",
                                items = new Schema {@ref = $"#/definitions/{type}"}
                            }
                        }
                    }
                };

                pathItem.get = get;
            }

            return paths;
        }
    }
}