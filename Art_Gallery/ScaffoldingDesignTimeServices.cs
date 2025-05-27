using System;
using System.Collections.Generic;
using System.IO;
using EntityFrameworkCore.Scaffolding.Handlebars;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using EntityFrameworkCore.Scaffolding.Handlebars;
using System.Reflection.Emit;

public class ScaffoldingDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        // Register Handlebars scaffolding
        services.AddHandlebarsScaffolding(options =>
        {
            // Generate both DbContext and entity classes
            options.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;

            // Optional: Supply custom template data accessed within .hbs
            options.TemplateData = new Dictionary<string, object>
            {
                ["models-namespace"] = "ArtGallery.Models"
            };
        });

        // Fix: Correct the method signature to match the expected delegate type
        services.AddHandlebarsHelpers( ("check", (writer, context, parameters) =>
        {
            for(int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i]?.ToString() == "is_square")
                {
                    writer.Write("isSquare***");
                }
                else
                {
                    writer.Write(parameters[i]);
                }
            }
            
        }));

        services.AddHandlebarsTransformers(propertyTransformer: e =>
            e.PropertyName == "Id"
            ? new EntityPropertyInfo("int?", e.PropertyName)
            : new EntityPropertyInfo(e.PropertyType, e.PropertyName)
        );

        // Register Handlebars helper
        var myHelper = (helperName: "camelCase", helperFunction: (Action<EncodedTextWriter, Context, Arguments>)CamelCaseHelper);
        var pascalCaseHelper = (helperName: "pascalCase", helperFunction: (Action<EncodedTextWriter, Context, Arguments>)PascalCaseHelper);
        services.AddHandlebarsHelpers(myHelper, pascalCaseHelper);
    }
    private void CamelCaseHelper(EncodedTextWriter writer, Context context, Arguments parameters)
    {
        // If no parameter or not a string, write empty
        if (parameters.Length == 0 || parameters[0] == null)
        {
            writer.Write(string.Empty);
            return;
        }

        var input = parameters[0].ToString();
        if (input.Length == 0)
        {
            writer.Write(string.Empty);
            return;
        }

        // Lowercase first char, append the rest
        var camel = char.ToLowerInvariant(input[0]) + input.Substring(1);
        writer.Write(camel);
    }
    private void PascalCaseHelper(EncodedTextWriter writer, Context context, Arguments parameters)
    {
        if (parameters.Length == 0 || parameters[0] == null)
        {
            writer.Write(string.Empty);
            return;
        }

        var input = parameters[0].ToString();
        if (string.IsNullOrEmpty(input))
        {
            writer.Write(string.Empty);
            return;
        }

        // Convert to PascalCase: "artifact_name" → "ArtifactName"
        var words = input.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var pascal = string.Concat(words.Select(w =>
            char.ToUpperInvariant(w[0]) + w.Substring(1).ToLowerInvariant()));

        writer.Write(pascal);
    }
}
