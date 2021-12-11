using System;
using System.Linq;
using System.Reflection;
using Reinforced.Typings.Fluent;
using ReinforcedTypings.Controllers;
using ReinforcedTypings.Dtos;
using ReinforcedTypings.Enumerations;
using ReinforcedTypings.FluentConfigurations.Generator;

namespace ReinforcedTypings.FluentConfigurations
{
    public static class FluentConfiguration
    {
        public const string SOLUTION_NAMESPACE = "ReinforcedTypings";
        
        private static TBuilder BaseConfiguration<TBuilder>(this TBuilder conf)
            where TBuilder : ClassOrInterfaceExportBuilder
        {
            conf
                .WithPublicProperties(i => i.CamelCase())
                .WithProperties(i => i.PropertyType == typeof(DateTime) || i.PropertyType == (typeof(DateTime?)), i => i.Type("Date"))
                .WithProperties(i => i.PropertyType == typeof(Guid) || i.PropertyType == (typeof(Guid?)), i => i.Type("string"));

            return conf;
        }

        private static Type[] GetTypesForExport<T>(string namespaceFilter)
        {
            return Assembly
                .GetAssembly(typeof(T))
                .ExportedTypes
                .Where(i => i.Namespace.StartsWith(namespaceFilter))
                .OrderBy(i => i.Name)
                .OrderBy(i => i.Name != nameof(T))
                .ToArray();
        }

        private static readonly Action<ClassExportBuilder> _classConfiguration = conf => conf
            .BaseConfiguration()
            .ExportTo("models.ts");

        private static readonly Action<InterfaceExportBuilder> _interfacesConfiguration = conf => conf
            .BaseConfiguration()
            .ExportTo("interfaces.ts");


        private static readonly Action<ClassExportBuilder> _serviceConfiguration = conf => conf
            .AddImport("{ Injectable }", "@angular/core")
            .AddImport("{ HttpParams, HttpClient }", "@angular/common/http")
            .AddImport("{ SettingsService }", "@Workspace/services")
            .AddImport("{ Observable }", "rxjs")
            .AddImport("{ map }", "rxjs/operators")
            .ExportTo("services.ts")
            .WithCodeGenerator<AngularControllerGenerator>();


        public static void Configure(ConfigurationBuilder builder)
        {
            builder.Global(i => i.UseModules());
            builder.ConfigureTypes();
        }

        private static void ConfigureTypes(this ConfigurationBuilder builder)
        {
            var dtos = GetTypesForExport<BaseDto>($"{SOLUTION_NAMESPACE}.Dtos");
            var controllers = GetTypesForExport<BaseApiController>($"{SOLUTION_NAMESPACE}.Controllers");

            // Tools > Options > Projects And Solutions > Build And Run
            // Set MSBuild project build output verbosity -> Detailed
            Console.WriteLine("EXPORT-DIR");
            Console.WriteLine(builder.Context.TargetDirectory);
            Console.WriteLine(builder.Context.TargetFile);

            // dto export
            builder.ExportAsInterfaces(dtos, _interfacesConfiguration);

            // enum export
            builder.ExportAsEnums(new Type[]
                {
                     typeof(eEntityStatus)
                      // ...
                },
                conf => conf.ExportTo("enums.ts")
            );
            
            // controller export
            builder.ExportAsClasses(controllers, _serviceConfiguration);
        }
    }
}
