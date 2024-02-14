using Microsoft.CodeAnalysis;

namespace OfuscadorMovilEc.Services
{
    public class OfuscadorService
    {
        private readonly int contador;
        private readonly SemanticModel semanticModel;
        private readonly Compilation compilation;

        public OfuscadorService(int contador, SemanticModel semanticModel, Compilation compilation)
            => (this.contador, this.semanticModel, this.compilation) = (contador, semanticModel, compilation);

        public SyntaxNode OfuscarCodigo(SyntaxNode root)
            => new Rewriter(contador, semanticModel, compilation).Visit(root);
    }
}
