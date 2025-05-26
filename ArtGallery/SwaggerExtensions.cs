using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Art Gallery API",
                Description = "New backend service that provides resources for Art Gallery.",
                Contact = new OpenApiContact
                {
                    Name = "Dong Quan Le",
                    Email = "s225051626@deakin.edu.au"
                },
            });
            //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            
            //--Oauth2 Authentication 
            // Configure OAuth2 security scheme
            var oauth2Scheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                BearerFormat = "JWT",
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://dev-2h03oeftseq6mqrr.us.auth0.com/authorize"), 
                        TokenUrl = new Uri("https://dev-2h03oeftseq6mqrr.us.auth0.com/oauth/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "read", "Read access to Art Gallery" },
                            { "write", "Create, Update, Delete access to Art Gallery" }
                        }
                    }
                },
                Scheme = "oauth2",
                Name = "OAuth2",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "OAuth2"
                }
            };

            options.AddSecurityDefinition("OAuth2", oauth2Scheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    new[] { "read", "write" }
                }
            });
            
            /*
            //-------Bearer Authentication
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Using the Authorization header with the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securitySchema);
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                  {
                  { securitySchema, new[] { "Bearer" } }
                  });
            */
        });
        return services;
    }
}