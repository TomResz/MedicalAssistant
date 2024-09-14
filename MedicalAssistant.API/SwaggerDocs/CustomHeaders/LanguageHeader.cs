using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MedicalAssistant.API.SwaggerDocs.CustomHeaders;

public class LanguageHeader : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		operation.Parameters ??= new List<OpenApiParameter>();

		operation.Parameters.Add(new OpenApiParameter()
		{
			Name = "X-Current-Language",
			In = ParameterLocation.Header,
			Required = true,
			Schema = new OpenApiSchema()
			{
				Type = "string",
				Enum = new List<IOpenApiAny>
				{
					new OpenApiString("pl-PL"),
					new OpenApiString("en-US")
				}
			},
		});
	}
}
