using System.Collections.Generic;
using System.Linq;
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

            swaggerDoc.paths = Paths.GenerateSwaggerPaths(templateTypes);
            swaggerDoc.definitions = Definitions.GenerateSwaggerDefinitions(templateTypes);

            return swaggerDoc;
        }
    }
}