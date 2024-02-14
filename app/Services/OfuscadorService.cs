using Microsoft.CodeAnalysis;

namespace OfuscadorMovilEc.Services
{
   public class OfuscadorService
{
    // Contador utilizado para lógica en la ofuscación
    private readonly int contador;

    // Constructor que recibe un contador, SemanticModel y Compilation
    public OfuscadorService(int contador)
    {
        this.contador = contador;
    }

    // Método para ofuscar el código en un SyntaxNode dado
    public SyntaxNode OfuscarCodigo(SyntaxNode root, SemanticModel semanticModel, Compilation compilation)
    {
        // Crea una nueva instancia de Rewriter y aplica la operación de ofuscación al SyntaxNode
        return new Rewriter(contador, semanticModel, compilation).Visit(root);
    }
}

}
