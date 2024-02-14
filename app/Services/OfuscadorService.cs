// OfuscadorService.cs
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class OfuscadorService
{
    private static int contador = 0;

    public SyntaxNode OfuscarCodigo(SyntaxNode root)
    {
        var reescritor = new NombreVariableRewriter();
        SyntaxNode nuevoArbol = reescritor.Visit(root);

        return nuevoArbol;
    }

    private string EnsureUniqueName(string nuevoNombre, SyntaxToken? originalToken)
    {
        while (!IsNameUnique(nuevoNombre, originalToken))
        {
            nuevoNombre = nuevoNombre + "_";
        }
        return nuevoNombre;
    }

    private bool IsNameUnique(string nombre, SyntaxToken? originalToken)
    {
        return SyntaxFactory.ParseToken(nombre).IsKind(SyntaxKind.None) &&
               (originalToken?.GetLocation().SourceTree?.IsEquivalentTo(SyntaxFactory.ParseSyntaxTree("class C{}")) ?? true);
    }
}
