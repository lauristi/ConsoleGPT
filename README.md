# OpenIA Sources

Esta é uma solution de estudo, só para entender a utilização do dos motores de IA
da OpenIA com .Net

### O código original foi tirado de:
https://rmauro.dev/getting-started-with-chat-gpt-integration-with-csharp-console-application/


### A Saber:

Para urtilizar a API da OpenIA é preciso ter uma conta com um ID e uma apiKey
que deve ser informada no arquivo `appsettings.json` de ambos os projetos.

```
{
  "OpenAi": {
    "OrganizationId": "",
    "ApiKey": ""
  }
}
```

### Atenção!!!
Diferente do Site do ChatGPT o uso da API é cobrado.
Então ter apenas a ApiKey cadastada pelo site do ChatGPT não vai funcionar.

