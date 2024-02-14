using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OfuscadorMovilEc.Services;

class Program
{
    static void Main()
    {
        string filePath = "C:/PROYECTOS DESARROLLO/.NET/ofuscador/Prueba.cs";
        string codigoFuente = File.ReadAllText(filePath);

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codigoFuente);
        CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

        var ofuscadorService = new OfuscadorService();
        SyntaxNode nuevoArbol = ofuscadorService.OfuscarCodigo(root);

        string nuevoCodigoFuente = nuevoArbol.ToFullString();
        File.WriteAllText("C:/PROYECTOS DESARROLLO/.NET/ofuscador/ArchivoOfuscado.cs", nuevoCodigoFuente);

        Console.WriteLine("Proceso de ofuscación completado.");
    }
}
