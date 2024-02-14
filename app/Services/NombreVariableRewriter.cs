using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OfuscadorMovilEc.Services
{
    public class NombreVariableRewriter : CSharpSyntaxRewriter
    {
        private int contador;

        public NombreVariableRewriter(int contador)
        {
            this.contador = contador;
        }

        public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax nodo)
        {
            var nuevoNombre = "variable" + contador++;
            nuevoNombre = EnsureUniqueName(nuevoNombre, nodo.Identifier);

            return nodo.WithIdentifier(SyntaxFactory.Identifier(nuevoNombre));
        }

        public override SyntaxNode VisitParameter(ParameterSyntax parametro)
        {
            var nuevoNombre = "parametro" + contador++;
            nuevoNombre = EnsureUniqueName(nuevoNombre, parametro.Identifier);

            return parametro.WithIdentifier(SyntaxFactory.Identifier(nuevoNombre));
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
}
