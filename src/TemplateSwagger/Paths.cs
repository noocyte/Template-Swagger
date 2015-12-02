using System.Collections.Generic;
using System.Linq;
using Swashbuckle.Swagger;
using UXRisk.Lib.TemplateModel.Models;

namespace TemplateSwagger
{
    internal static class Paths
    {
        internal static IDictionary<string, PathItem> GenerateSwaggerPathsForEntities(IEnumerable<ArticleType> templateTypes)
        {
            var knownEntities = templateTypes
                .SelectMany(t => t.Fields)
                .Where(f => !f.Name.StartsWith("a_"))
                .Where(f => !string.IsNullOrWhiteSpace(f.Entity?.Type))
                .Select(f => f.Entity.Type)
                .Distinct()
                .ToList();

            var paths = new Dictionary<string, PathItem>();

            var articleParameters = new List<Parameter>
            {
                new Parameter
                {
                    name = "id",
                    @in = "query",
                    description = "Unique id of object",
                    required = false,
                    type = "string",
                    format = "string"
                }
            };

            foreach (var entity in knownEntities)
            {
                var type = entity;
                paths.Add($"/entity/{type}/{{id}}", new PathItem
                {
                    get = CreateGetSingleOperation(type, articleParameters),
                    put = CreatePutSingleOperation(type, articleParameters)
                });

                paths.Add($"/entity/{type}", new PathItem
                {
                    get = CreateGetMultiOperation(type, articleParameters)
                });
            }

            return paths;
        }


        internal static IDictionary<string, PathItem> GenerateSwaggerPathsForArticles(IEnumerable<ArticleType> templateTypes)
        {
            var paths = new Dictionary<string, PathItem>();

            var articleParameters = new List<Parameter>
            {
                new Parameter
                {
                    name = "id",
                    @in = "query",
                    description = "Unique id of object",
                    required = false,
                    type = "string",
                    format = "string"
                }
            };

            foreach (var templateType in templateTypes)
            {
                var type = templateType.Type;
                paths.Add($"/article/{type}/{{id}}", new PathItem
                {
                    get = CreateGetSingleOperation(type, articleParameters),
                    put = CreatePutSingleOperation(type, articleParameters)
                });

                paths.Add($"/article/{type}", new PathItem
                {
                    get = CreateGetMultiOperation(type, articleParameters)
                });
            }

            return paths;
        }

        private static Operation CreatePutSingleOperation(string type, List<Parameter> articleParameters)
        {
            var put = new Operation
            {
                summary = $"Create or update {type}",
                parameters = articleParameters,
                responses = new Dictionary<string, Response>
                {
                    ["200"] = new Response
                    {
                        description = $"A result array of {type}",
                        schema = new Schema {@ref = $"{type}"}
                    }
                }
            };
            return put;
        }

        private static Operation CreateGetMultiOperation(string type, List<Parameter> articleParameters)
        {
            var get = new Operation
            {
                summary = $"Get all {type}(s)",
                parameters = articleParameters,
                responses = new Dictionary<string, Response>
                {
                    ["200"] = new Response
                    {
                        description = $"A result array of {type}",
                        schema = new Schema
                        {
                            type = "object",
                            @ref = $"ResultOf{type}"
                        }
                    }
                }
            };
            return get;
        }

        private static Operation CreateGetSingleOperation(string type, List<Parameter> articleParameters)
        {
            var get = new Operation
            {
                summary = $"Get one {type}",
                parameters = articleParameters,
                responses = new Dictionary<string, Response>
                {
                    ["200"] = new Response
                    {
                        description = $"A result array of {type}",
                        schema = new Schema
                        {
                            type = "object",
                            @ref = $"ResultOf{type}"
                        }
                    }
                }
            };
            return get;
        }
    }
}