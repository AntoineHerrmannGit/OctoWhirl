using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using OctoWhirl.TechnicalServices.Scripting.Interfaces;
using System.Reflection;
using System.Runtime.Loader;

namespace OctoWhirl.TechnicalServices.Scripting
{
    public class ScriptCompiler
    {
        public IScript? CompileAndInstantiate(string userCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(userCode);

            var requiredAssemblies = new[]
            {
            typeof(object).Assembly,                  // mscorlib
            typeof(Enumerable).Assembly,              // System.Linq
            typeof(IScript).Assembly,                 // Interface de ton domaine
            Assembly.Load("System.Runtime")           // Pour les types de base
            };

            var references = requiredAssemblies
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .ToList();

            var compilation = CSharpCompilation.Create(
                assemblyName: "UserScriptAssembly",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var diagnostics = result.Diagnostics
                    .Where(d => d.Severity != DiagnosticSeverity.Hidden)
                    .Select(d => d.ToString());

                throw new InvalidOperationException("Compilation failed:\n" + string.Join("\n", diagnostics));
            }

            ms.Seek(0, SeekOrigin.Begin);
            var context = new AssemblyLoadContext("UserScriptContext", isCollectible: true);
            Assembly assembly;

            try
            {
                assembly = context.LoadFromStream(ms);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load compiled assembly.", ex);
            }

            var scriptType = assembly.GetTypes()
                .FirstOrDefault(t => typeof(IScript).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            if (scriptType == null)
                throw new InvalidOperationException("No valid IScript implementation found.");

            var instance = Activator.CreateInstance(scriptType) as IScript;
            if (instance == null)
                throw new InvalidOperationException("Failed to instantiate IScript.");

            return instance;
        }
    }
}
