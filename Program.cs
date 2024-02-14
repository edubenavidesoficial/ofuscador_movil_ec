// Program.cs
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

            var compilation = CSharpCompilation.Create("MyCompilation")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Almacena instancias reutilizables de SemanticModel y Compilation
            var ofuscadorService = new OfuscadorService(semanticModel, compilation);
            SyntaxNode nuevoArbol = ofuscadorService.OfuscarCodigo(root);

            string nuevoCodigoFuente = nuevoArbol.ToFullString();
            File.WriteAllText("C:/PROYECTOS DESARROLLO/.NET/ofuscador/ArchivoOfuscado.cs", nuevoCodigoFuente);

            Console.WriteLine("Proceso de ofuscación completado.");
        }
    }
}

