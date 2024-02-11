using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class Program
{
    static void Main()
    {
        string filePath = "C:/PROYECTOS DESARROLLO/.NET/ofusca/prueba.cs"; // Ruta del archivo C# que deseas ofuscar
        string codigoFuente = File.ReadAllText(filePath);

        // Crear un árbol de sintaxis desde el código fuente
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codigoFuente);

        // Obtener la raíz del árbol de sintaxis
        CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

        // Realizar manipulaciones en el árbol de sintaxis (ejemplo: cambiar todos los nombres de variables)
        SyntaxNode nuevoArbol = OfuscarNombresDeVariables(root);

        // Guardar el nuevo código fuente en un archivo (o realizar acciones adicionales según tus necesidades)
        string nuevoCodigoFuente = nuevoArbol.ToFullString();
        File.WriteAllText("C:/PROYECTOS DESARROLLO/.NET/ofusca/ArchivoOfuscado.cs", nuevoCodigoFuente);

        Console.WriteLine("Proceso de ofuscación completado.");
    }

    static SyntaxNode OfuscarNombresDeVariables(SyntaxNode root)
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
        nuevoNombre = SyntaxFactory.Identifier(nuevoNombre).EnsureUniqueName(this);

        return nodo.WithIdentifier(nuevoNombre);
    }

    public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax nodo)
    {
        // Cambiar el nombre de los parámetros del método
        var parametrosOfuscados = nodo.ParameterList.Parameters.Select(OfuscarNombreDeParametro).ToList();
        var nuevaListaDeParametros = SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parametrosOfuscados));
        return nodo.WithParameterList(nuevaListaDeParametros);
    }

    private ParameterSyntax OfuscarNombreDeParametro(ParameterSyntax parametro)
    {
        var nuevoNombre = "cnb" + contador++;
        nuevoNombre = SyntaxFactory.Identifier(nuevoNombre).EnsureUniqueName(this);
        return parametro.WithIdentifier(SyntaxFactory.Identifier(nuevoNombre));
    }
}

public static class SyntaxNodeExtensions
{
    public static SyntaxToken EnsureUniqueName(this SyntaxToken token, CSharpSyntaxRewriter reescritor)
    {
        while (reescritor.VisitToken(token).Text != token.Text)
        {
            token = SyntaxFactory.Identifier(token.Text + "_");
        }
        return token;
    }
}
