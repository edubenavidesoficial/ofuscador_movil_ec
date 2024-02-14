using Microsoft.CodeAnalysis;

namespace OfuscadorMovilEc.Services
{
    public class OfuscadorService
    {
        private int contador = 0;
        private SemanticModel semanticModel;
        private Compilation compilation;

        public OfuscadorService(SemanticModel semanticModel, Compilation compilation)
        {
            this.semanticModel = semanticModel;
            this.compilation = compilation;
        }

        public SyntaxNode OfuscarCodigo(SyntaxNode root)
        {
            var reescritor = new Rewriter(contador, semanticModel, compilation);
            SyntaxNode nuevoArbol = reescritor.Visit(root);

            return nuevoArbol;
        }
    }
}
