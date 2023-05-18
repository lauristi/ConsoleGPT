namespace ConsoleChatGPT.OpenIA
{
    // OpenIAProxy: Classe que atua como um proxy para a biblioteca OpenAI.
    // É responsável por enviar mensagens de chat para o modelo GPT-3.5-turbo
    // e receber as respostas correspondentes.
    public class OpenIAProxy : IOpenIAProxy
    {
        // Cliente OpenAI utilizado para enviar solicitações ao modelo.
        private readonly OpenAIClient openAIClient;

        // Lista de mensagens do chat.
        private readonly List<ChatCompletionMessage> _messages;  
        
        public OpenIAProxy(string apiKey, string organizationId)
        {
            // Configurações do cliente OpenAI
            // Incluindo a chave de API e o ID da organização.
            var openAIConfigurations = new OpenAIConfigurations
            {
                ApiKey = apiKey,
                OrganizationId = organizationId
            };

            // Inicializa o cliente OpenAI com as configurações fornecidas.
            openAIClient = new OpenAIClient(openAIConfigurations);

            // Inicializa a lista de mensagens do chat.
            _messages = new();
        }

        // Define uma mensagem do sistema que
        // será enviada antes das mensagens do usuário.
        public void SetSystemMessage(string systemMessage)
        {
            var sysMsg = new ChatCompletionMessage()
            {
                Content = systemMessage,
                Role = "system"
            };
            _messages.Insert(0, sysMsg);
        }

        // Empilha mensagens na lista de mensagens do chat.
        private void StackMessages(params ChatCompletionMessage[] message)
        {
            _messages.AddRange(message);
        }

        // Converte um array de escolhas de completude do chat
        // em um array de mensagens completas do chat.
        private static ChatCompletionMessage[] ToCompletionMessage(ChatCompletionChoice[] choices)
            => choices.Select(x => x.Message).ToArray();

        // Envia uma mensagem de chat do usuário e retorna as mensagens completas em resposta.
        public Task<ChatCompletionMessage[]> SendChatMessage(string message)
        {
            var chatMsg = new ChatCompletionMessage() { Content = message, Role = "user" };
            return SendChatMessage(chatMsg);
        }

        // Envia uma mensagem de chat específica
        // e retorna as mensagens completas em resposta.
        private async Task<ChatCompletionMessage[]> SendChatMessage(ChatCompletionMessage message)
        {
            StackMessages(message);

            var chatCompletion = new ChatCompletion
            {
                Request = new ChatCompletionRequest
                {
                    Model = "gpt-3.5-turbo",
                    Messages = _messages.ToArray(),
                    Temperature = 0.2,
                    MaxTokens = 800
                }
            };

            // Envia a solicitação de completude
            // do chat para o modelo GPT-3.5-turbo.
            ChatCompletion resultChatCompletion = await openAIClient.ChatCompletions.SendChatCompletionAsync(chatCompletion);

            var choices = resultChatCompletion.Response.Choices;

            var messages = ToCompletionMessage(choices);

            StackMessages(messages);

            return messages;
        }
    }
}