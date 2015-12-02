using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoreLinq;
using Swashbuckle.Swagger;
using UXRisk.Lib.TemplateModel.Models;

namespace TemplateSwagger
{
    public class TemplateSwagger : ISwaggerProvider
    {
        private readonly ISwaggerProvider _swaggerProvider;
        private readonly IEnumerable<Template> _templates;


        public TemplateSwagger(ISwaggerProvider swaggerProvider, IEnumerable<Template> templates)
        {
            _swaggerProvider = swaggerProvider;
            _templates = templates;
        }

        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            var swaggerDoc = _swaggerProvider.GetSwagger(rootUrl, apiVersion);
            var templateTypes = _templates
                .SelectMany(t => t.Types)
                .DistinctBy(type => type.Type)
                .ToList();

            swaggerDoc.paths = swaggerDoc.paths
                .Concat(Paths.GenerateSwaggerPaths(templateTypes))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            swaggerDoc.definitions = swaggerDoc.definitions
                .Concat(Definitions.GenerateSwaggerDefinitions(templateTypes))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            return swaggerDoc;
        }
    }
}