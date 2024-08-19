using System.Collections; // Importa a interface IEnumerable para permitir a enumeração.
using MRB.CommonTest.Requests.Recipes; // Importa o namespace que contém a classe FormFileBuilder.

namespace MRB.UseCaseTest.Recipes.InlineData // Define o namespace para a classe atual.
{
    // Define uma classe que implementa a interface IEnumerable<object[]> para fornecer dados inline.
    public class ImageTypesInlineData : IEnumerable<object[]>
    {
        // Implementa o método GetEnumerator para retornar um enumerador que itera sobre uma coleção de objetos.
        public IEnumerator<object[]> GetEnumerator()
        {
            // Chama um método estático que retorna uma coleção de imagens.
            var images = FormFileBuilder.ImageCollection();
            
            // Itera sobre cada imagem na coleção e retorna como um array de objetos.
            foreach (var image in images)
                yield return new object[] { image };
        }

        // Implementa o método não genérico GetEnumerator para compatibilidade com a interface IEnumerable.
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}