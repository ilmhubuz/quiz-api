using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Quiz.CSharp.Api.Dtos.Question;

namespace Quiz.CSharp.Api.Helper;

public class QuestionMetadataSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(UpdateQuestionMetaDataDto)) return;
        
        schema.Discriminator = new OpenApiDiscriminator
        {
            PropertyName = "type",
            Mapping = new Dictionary<string, string>
            {
                { "mcq", $"#/components/schemas/{nameof(UpdateMcqMetaDataDto)}" },
                { "true_false", $"#/components/schemas/{nameof(UpdateTrueFalseMetaDataDto)}" },
                { "fill", $"#/components/schemas/{nameof(UpdateFillMetaDataDto)}" },
                { "error_spotting", $"#/components/schemas/{nameof(UpdateErrorSpottingMetaDataDto)}" },
                { "output_prediction", $"#/components/schemas/{nameof(UpdateOutputPredictionMetaDataDto)}" },
                { "code_writing", $"#/components/schemas/{nameof(UpdateCodeWritingMetaDataDto)}" }
            }
        };

        schema.OneOf = new List<OpenApiSchema>
        {
            context.SchemaGenerator.GenerateSchema(typeof(UpdateMcqMetaDataDto), context.SchemaRepository),
            context.SchemaGenerator.GenerateSchema(typeof(UpdateTrueFalseMetaDataDto), context.SchemaRepository),
            context.SchemaGenerator.GenerateSchema(typeof(UpdateFillMetaDataDto), context.SchemaRepository),
            context.SchemaGenerator.GenerateSchema(typeof(UpdateErrorSpottingMetaDataDto), context.SchemaRepository),
            context.SchemaGenerator.GenerateSchema(typeof(UpdateOutputPredictionMetaDataDto), context.SchemaRepository),
            context.SchemaGenerator.GenerateSchema(typeof(UpdateCodeWritingMetaDataDto), context.SchemaRepository),
        };
    }
}