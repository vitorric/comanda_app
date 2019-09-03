
# Comanda Gameficada

## Introdução
Aplicativo de Comanda Gameficada desenvolvido em Unity3D. Este é um aplicativo que exibe a comanda de um cliente e disponibiliza desafios a serem cumpridos pelos mesmos, proporcionando prêmios ao serem finalizados.

## Preparação
Antes de iniciar, é necessário iniciar a API, pois sem ela, o aplicativo não irá funcionar. Acesse  [aqui](https://github.com/vitorric/comanda_api) e siga os passos.

Após isso, instale o  [Unity3D]([https://unity3d.com/pt/get-unity/download] localmente.

 * **Versão do Unity3D**: 2019.1.12f1

## Estrutura de pasta

A estrutura de pasta abaixo é referente a pasta **Assets**.  As outras pasta são geradas automaticamente, logo, o projeto desenvolvido fica totalmente dentro da Assets.

``` bash
├───Animation
├───Audio
│   └───...
├───FacebookSDK
│   ├───...
├───Firebase
│   ├───...
├───Layout
│   ├───...
├───Parse
│   └───...
├───PlayServicesResolver
│   └───..
├───Plugins
│   ├───...
├───Prefabs
├───Resources
│   └───...
├───Scenes
├───Scripts
│   ├───API
│   ├───APIModel
│   ├───Audio
│   ├───FirebaseModel
│   ├───Game
│   ├───Manager
│   ├───QRCode
│   ├───Util
│   └───View
│       ├───...
├───StreamingAssets
├───TextMesh Pro
│   ├───...
└───Third
    ├───...
```

## Executando o Aplicativo

* Faça o clone deste projeto (será criado uma pasta chamada comanda_app)
* Abra o Unity3D e importe a pasta. Aguarde a compilação da mesma.
* Inicie a API
* Inicie o aplicativo no Unity3D

## Conexões

Dentro da pasta **Assets/Scripts/API** existe um arquivo chamado API.cs, com as seguintes variáveis:

```csharp
private  const  string urlBase =  "http://localhost:3000/api/";
private  const  string urlBaseDownloadIcon =  "http://localhost:3000/";
```

Altere o caminho de conexão para o caminho da API que foi iniciado anteriormente.

## Tutorias

* Como [configurar o SDK do facebook](https://developers.facebook.com/docs/unity/).
* Como [configurar o Firebase](https://firebase.google.com/docs/unity/setup?hl=pt-br).

## Exemplos de Tela

<p align="center">
  <img src="https://uploaddeimagens.com.br/imagens/login-png-bf757101-45d2-49b8-bf6c-c8afa930ca3a" width="200">
</p>
