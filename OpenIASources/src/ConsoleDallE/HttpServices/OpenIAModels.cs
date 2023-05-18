namespace ConsoleDallE.HttpServices
{
    // GenerateImageRequest:
    // Classe de registro (record) que representa
    // uma solicitação de geração de imagem.
    // Ela contém propriedades para o prompt da imagem,
    // a quantidade de imagens a serem geradas (N) e o tamanho desejado.
    public record class GenerateImageRequest(string Prompt, int N, string Size);

    // GenerateImageResponse:
    // Classe de registro (record) que representa
    // a resposta da geração de imagem.
    // Ela contém uma propriedade para o carimbo de data/hora
    // de criação e um array de dados de imagem gerados.
    public record class GenerateImageResponse(long Created, GeneratedImageData[] Data);

    // GeneratedImageData: Classe de registro (record) que representa
    // os dados de uma imagem gerada.
    // Ela contém uma propriedade para a URL da imagem.
    public record class GeneratedImageData(string Url);
}
