using FileTypeChecker.Extensions; // Inclui métodos de extensão para verificar tipos de arquivos
using FileTypeChecker.Types; // Inclui tipos específicos de arquivos suportados pela biblioteca

namespace MRB.Application.Extensions
{
    public static class StreamImageExtensions
    {
        /// <summary>
        /// Valida se o fluxo (stream) contém uma imagem válida e retorna sua extensão.
        /// </summary>
        /// <param name="stream">O fluxo de entrada contendo o arquivo a ser verificado.</param>
        /// <returns>
        /// Uma tupla contendo um booleano indicando se é uma imagem válida e uma string com a extensão do arquivo.
        /// </returns>
        public static (bool isValidImage, string extension) ValidateAndGetImageExtension(this Stream stream)
        {
            // Inicializa o resultado com valores padrão (não válido e extensão vazia)
            var result = (false, string.Empty);

            // Verifica se o stream é um arquivo PNG
            if (stream.Is<PortableNetworkGraphic>())
                result = (true, NormalizeExtension(PortableNetworkGraphic.TypeExtension));
            // Verifica se o stream é um arquivo JPEG
            else if (stream.Is<JointPhotographicExpertsGroup>())
                result = (true, NormalizeExtension(JointPhotographicExpertsGroup.TypeExtension));

            // Reseta a posição do stream para o início, garantindo que ele pode ser lido novamente
            stream.Position = 0;

            // Retorna o resultado da verificação
            return result;
        }

        /// <summary>
        /// Normaliza a extensão do arquivo para garantir que comece com um ponto (.)
        /// </summary>
        /// <param name="extension">A extensão do arquivo a ser normalizada.</param>
        /// <returns>A extensão do arquivo, garantindo que comece com um ponto.</returns>
        private static string NormalizeExtension(string extension)
        {
            // Verifica se a extensão começa com um ponto (.)
            // Se não começar, adiciona um ponto no início
            return extension.StartsWith('.') ? extension : $".{extension}";
        }
    }
}