using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OfuscadorMovilEc.Services
{
    public class Rewriter : CSharpSyntaxRewriter
{
    private int contador;
    private SemanticModel semanticModel;
    private Compilation compilation;

    // Constructor que recibe un contador, SemanticModel y Compilation
    public Rewriter(int contador, SemanticModel semanticModel, Compilation compilation)
    {
        this.contador = contador;
        this.semanticModel = semanticModel;
        this.compilation = compilation;
    }

    // Sobrescribe el método para visitar nodos de declaraciones de variables
    public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax nodo)
    {
        // Genera un nuevo nombre para la variable usando el contador
        var nuevoNombre = "variable" + contador++;
        
        // Asegura que el nuevo nombre sea único y no cause conflictos
        nuevoNombre = EnsureUniqueName(nuevoNombre, nodo.Identifier);

        // Modifica el identificador de la variable con el nuevo nombre
        return nodo.WithIdentifier(SyntaxFactory.Identifier(nuevoNombre));
    }

    // Sobrescribe el método para visitar nodos de parámetros de método
    public override SyntaxNode VisitParameter(ParameterSyntax parametro)
    {
        // Genera un nuevo nombre para el parámetro usando el contador
        var nuevoNombre = "parametro" + contador++;
        
        // Asegura que el nuevo nombre sea único y no cause conflictos
        nuevoNombre = EnsureUniqueName(nuevoNombre, parametro.Identifier);

        // Modifica el identificador del parámetro con el nuevo nombre
        return parametro.WithIdentifier(SyntaxFactory.Identifier(nuevoNombre));
    }

    // Método privado para garantizar que un nombre sea único
    private string EnsureUniqueName(string nuevoNombre, SyntaxToken? originalToken)
    {
        // Agrega sufijos hasta que el nombre sea único
        while (!IsNameUnique(nuevoNombre, originalToken))
        {
            nuevoNombre = nuevoNombre + "_";
        }
        return nuevoNombre;
    }

    // Método privado para verificar si un nombre es único
    private bool IsNameUnique(string nombre, SyntaxToken? originalToken)
    {
        // Verifica que el nombre no cause conflictos y que no sea equivalente a "class C{}"
        return SyntaxFactory.ParseToken(nombre).IsKind(SyntaxKind.None) &&
               (originalToken?.GetLocation().SourceTree?.IsEquivalentTo(SyntaxFactory.ParseSyntaxTree("class C{}")) ?? true);
    }
}

}
