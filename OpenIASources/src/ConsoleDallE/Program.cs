using ConsoleDallE.HttpServices;
using ConsoleDallE.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

Console.WriteLine("Starting commandline for DALL-E [Open AI]");

// Carrega a configuração do aplicativo
var config = BuildConfig();

// Cria uma instância do cliente
// OpenIAHttpService usando as configuracoes recuperadas
IOpenIAProxy aiClient = new OpenIAHttpService(config);

// Solicita ao usuário que digite o primeiro prompt
Console.WriteLine("Type your first Prompt");
var msg = Console.ReadLine();

// Loop principal do programa
do
{
    // Obtém o número de imagens a serem geradas e o tamanho da imagem da configuração
    var nImages = int.Parse(config["OpenAi:DALL-E:N"]);
    var imageSize = config["OpenAi:DALL-E:Size"];

    // Cria uma instância de GenerateImageRequest
    // com o prompt, número de imagens e tamanho da imagem
    var prompt = new GenerateImageRequest(msg, nImages, imageSize);

    // Gera as imagens usando o cliente OpenAI e o prompt fornecido
    var result = await aiClient.GenerateImages(prompt);

    // Itera sobre os dados das imagens geradas
    foreach (var item in result.Data)
    {
        Console.WriteLine(item.Url);

        // Cria um caminho completo para salvar
        // a imagem no diretório atual com um nome aleatório
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), $"{Guid.NewGuid()}.png");

        // Faz o download da imagem usando a URL fornecida
        var img = await aiClient.DownloadImage(item.Url);

        // Salva a imagem como um arquivo
        // no caminho completo especificado
        await File.WriteAllBytesAsync(fullPath, img);
    }

    // Solicita ao usuário o próximo prompt
    Console.WriteLine("Next Prompt:");
    msg = Console.ReadLine();
} while (msg != "q");

//------------------------------------------------------------
// Função para construir a configuração do aplicativo
//------------------------------------------------------------
static IConfiguration BuildConfig()
{
    var dir = Directory.GetCurrentDirectory();
    var configBuilder = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false)
        .AddUserSecrets(Assembly.GetExecutingAssembly());

    return configBuilder.Build();
}