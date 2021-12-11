using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Reinforced.Typings;
using Reinforced.Typings.Ast;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Generators;

namespace ReinforcedTypings.FluentConfigurations.Generator
{
    public class AngularActionCallGenerator : MethodCodeGenerator
    {
        public override RtFunction GenerateNode(MethodInfo element, RtFunction result, TypeResolver resolver)
        {
            result = base.GenerateNode(element, result, resolver);
            if (result == null)
            {
                return null;
            }

            var returnType = result.ReturnType;
            if (returnType is RtSimpleTypeName && ((RtSimpleTypeName)returnType).TypeName == "void")
            {
                returnType = resolver.ResolveTypeName(typeof(object));
            }

            result.ReturnType = new RtSimpleTypeName("Observable", new[] { returnType });

            var parameters = element.GetParameters().Select(c => c.Name).ToList();
            var parameterTypes = element.GetParameters().Select(c => c.ParameterType).ToList();

            var controller = element.DeclaringType.Name.Replace("Controller", string.Empty);
            var path = $"{controller}/{element.Name}";

            var angularAttribute = (AngularMethodAttribute)Attribute.GetCustomAttribute(element, typeof(AngularMethodAttribute));

            var httpPostAttribute = Attribute.GetCustomAttribute(element, typeof(Microsoft.AspNetCore.Mvc.HttpPostAttribute));

            var allowAnonymousAttribute = Attribute.GetCustomAttribute(element, typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute));

            var code = new StringBuilder();

            var joinedParameters = string.Join(",", parameters.Select(p => $"'{p}': {p}"));
            code.AppendLine($"const body = <any>{{ {joinedParameters} }};");
            

            if (httpPostAttribute == null)
            {
                code.AppendLine($"return this.httpClient.get<{returnType}>(");
                code.AppendLine($"this.settingsService.createApiUrl('{path}'),");
                code.AppendLine("{");
                code.AppendLine($"\tresponseType: '{(angularAttribute?.IsArrayBuffer == true ? "arraybuffer" : "json")}',");
                code.AppendLine("\tobserve: 'response',");
                code.AppendLine($"\twithCredentials: {(allowAnonymousAttribute == null).ToString().ToLower()},");
                code.AppendLine("\tparams: new HttpParams({ fromObject: body })");
                code.AppendLine("})");
                code.AppendLine(".pipe(map(response => response.body));");

            }
            else
            {
                code.AppendLine($"return this.httpClient.post<{returnType}>(");
                code.AppendLine($"this.settingsService.createApiUrl('{path}'),");
                if (parameters.Any())
                {
                    code.AppendLine("body,");
                }
                else
                {
                    code.AppendLine("null,");
                }
                code.AppendLine("{");
                code.AppendLine($"\tresponseType: '{(angularAttribute?.IsArrayBuffer == true ? "arraybuffer" : "json")}',");
                code.AppendLine("\tobserve: 'response',");
                code.AppendLine($"\twithCredentials: {(allowAnonymousAttribute == null).ToString().ToLower()}");
                code.AppendLine("})");
                code.AppendLine(".pipe(map(response => response.body));");
            }

            result.Body = new RtRaw(code.ToString());

            return result;
        }
    }
}
