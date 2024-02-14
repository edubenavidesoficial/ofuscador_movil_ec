using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OfuscadorMovilEc.Services;

namespace OfuscadorMovilEc
{
    class Program
    {
        static void Main()
        {
            string filePath = "C:/PROYECTOS DESARROLLO/.NET/ofuscador/Prueba.cs";
            string codigoFuente = File.ReadAllText(filePath);

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codigoFuente);
            CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

            var compilation = CreateCompilation(syntaxTree);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Pasa 'compilation' al constructor de OfuscadorService
            var ofuscadorService = new OfuscadorService(0, semanticModel, compilation);
            SyntaxNode nuevoArbol = ofuscadorService.OfuscarCodigo(root);

            GuardarCodigoOfuscado(nuevoArbol);

            Console.WriteLine("Proceso de ofuscación completado.");
        }

        private static CSharpCompilation CreateCompilation(SyntaxTree syntaxTree)
        {
            return CSharpCompilation.Create("MyCompilation")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);
        }

        private static void GuardarCodigoOfuscado(SyntaxNode nuevoArbol)
        {
            string nuevoCodigoFuente = nuevoArbol.ToFullString();
            File.WriteAllText("C:/PROYECTOS DESARROLLO/.NET/ofuscador/ArchivoOfuscado.cs", nuevoCodigoFuente);
        }
    }
}
