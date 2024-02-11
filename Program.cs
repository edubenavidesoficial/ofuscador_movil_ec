using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class Program
{
    static void Main()
    {
        string filePath = "C:/PROYECTOS DESARROLLO/.NET/ofuscador_movil_ec/ofuscador/Prueba.cs"; // Ruta del archivo C# que deseas ofuscar
        string codigoFuente = File.ReadAllText(filePath);

        // Crear un árbol de sintaxis desde el código fuente
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codigoFuente);

        // Obtener la raíz del árbol de sintaxis
        CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

        // Realizar manipulaciones en el árbol de sintaxis (ejemplo: cambiar todos los nombres de variables)
        SyntaxNode nuevoArbol = OfuscarCodigo(root);

        // Guardar el nuevo código fuente en un archivo (o realizar acciones adicionales según tus necesidades)
        string nuevoCodigoFuente = nuevoArbol.ToFullString();
        File.WriteAllText("C:/PROYECTOS DESARROLLO/.NET/ofuscador_movil_ec/ofuscador/ArchivoOfuscado.cs", nuevoCodigoFuente);

        Console.WriteLine("Proceso de ofuscación completado.");
    }

    static SyntaxNode OfuscarCodigo(SyntaxNode root)
    {
        // Utilizar el reescritor de Roslyn para cambiar todos los nombres de las variables
        var reescritor = new NombreVariableRewriter();
        SyntaxNode nuevoArbol = reescritor.Visit(root);

        return nuevoArbol;
    }
}

class NombreVariableRewriter : CSharpSyntaxRewriter
{
    private static int contador = 0;

    public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax nodo)
    {
        // Cambiar el nombre de la variable evitando conflictos de nombres
        var nuevoNombre = "variable" + contador++;

        // Asegurar que el nuevo nombre no sea una palabra clave de C# o un identificador existente
        nuevoNombre = EnsureUniqueName(nuevoNombre, nodo.Identifier);

        return nodo.WithIdentifier(SyntaxFactory.Identifier(nuevoNombre));
    }

    public override SyntaxNode VisitParameter(ParameterSyntax parametro)
    {
        // Cambiar el nombre de los parámetros del método
        var nuevoNombre = "parametro" + contador++;
        nuevoNombre = EnsureUniqueName(nuevoNombre, parametro.Identifier);

        return parametro.WithIdentifier(SyntaxFactory.Identifier(nuevoNombre));
    }

    private static string EnsureUniqueName(string nuevoNombre, SyntaxToken originalToken)
    {
        // Asegurar que el nuevo nombre no sea una palabra clave de C# o un identificador existente
        while (SyntaxFactory.ParseToken(nuevoNombre).IsKind(SyntaxKind.None) ||
               !originalToken.GetLocation().SourceTree.IsEquivalentTo(SyntaxFactory.ParseSyntaxTree("class C{}")))
        {
            nuevoNombre = nuevoNombre + "_";
        }
        return nuevoNombre;
    }
}
