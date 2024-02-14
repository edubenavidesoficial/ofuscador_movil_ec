using Microsoft.CodeAnalysis;

namespace OfuscadorMovilEc.Services
{
    public class OfuscadorService
    {
        private int contador = 0;

        public SyntaxNode OfuscarCodigo(SyntaxNode root)
        {
            var reescritor = new NombreVariableRewriter(contador);
            SyntaxNode nuevoArbol = reescritor.Visit(root);

            return nuevoArbol;
        }
    }
}
