using ConsoleChatGPT.OpenIA;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Reflection;

AnsiConsole.MarkupLine("Starting commandline for [underline bold green]Chat GPT[/] World!");
AnsiConsole.Write(new FigletText("Console GPT").Color(Color.Blue));

var config = BuildConfig();

//-----------------------------------------------------------------------
//Inicia o chat OpenIA
//com as configuracoes de acesso recuperadas de appsettings.json
//-----------------------------------------------------------------------
IOpenIAProxy chatOpenAI = new OpenIAProxy(
    config["OpenAI:ApiKey"],
    config["OpenAI:OrganizationId"]);

chatOpenAI.SetSystemMessage("You are a helpful assistant called Felix AI");

var msg = AnsiConsole.Ask<string>("[bold blue]Type your first Prompt[/]:");

//-----------------------------------------------------------------------
//Entra em um modo de prompt continuo até que 'q' seja digitado
//-----------------------------------------------------------------------
do
{
    var results = await chatOpenAI.SendChatMessage(msg);

    foreach (var item in results)
    {
        AnsiConsole.MarkupLineInterpolated($"[red]{item.Role.Humanize(LetterCasing.Title)}: [/] {item.Content}");
    }

    msg = AnsiConsole.Ask<string>("[bold blue]Next Prompt[/]:");
} while (msg != "q");

//------------------------------------------------------------
// BuildConfig: Função responsável por construir
// e retornar uma instância de IConfiguration.
// É usada para carregar as configurações do aplicativo
// a partir do arquivo appsettings.json
// e informações secretas do usuário. (Ver secrets)
//------------------------------------------------------------
static IConfiguration BuildConfig()
{

    // Obtém o diretório atual em que o aplicativo está sendo executado.
    var dir = Directory.GetCurrentDirectory();

    // 1- Cria um objeto ConfigurationBuilder para construir a configuração do aplicativo.
    // 2- Adiciona o arquivo appsettings.json como uma fonte de configuração obrigatória.
    // 3- Adiciona as informações secretas do usuário ao ConfigurationBuilder.
    var configBuilder = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false)
        .AddUserSecrets(Assembly.GetExecutingAssembly());

    // Constrói e retorna a instância
    // de IConfiguration com as configurações carregadas.
    return configBuilder.Build();
}