# TechChallenge

## Introdução
- Projeto criado para o Tech Challenge dotnet da Fase 1 da PósTech em Arquitetura de Sistemas .Net com Azure da FIAP.
- Objetivo: Desenvolver uma aplicação Web com funcionalidades de upload e exibição de imagens, utilizando Serviços de Storage e banco de dados Azure.
- Vídeo da entrega: https://youtu.be/MicNhRRvxTA

## Funcionalidades

### Recurso 1: Aplicação Front-end em Angular 8 e TypeScript 3.5
- Responsável por apresentar as seguintes funcionalidades ao usuário:
  - Login e cadastro de usuário
  - Upload de imagens
  - Visualização e busca de imagens

Detalhes de setup do Angular 8: [Documentação oficial](https://v8.angular.io/guide/setup-local)  
Detalhes da implementação e execução: [Readme do projeto Front-end](https://github.com/JairJr/TechChallenge/tree/master/TechChallenge.WebApp#readme)

### Recurso 2: API AspNetCore em .NET 7 com Identity 7.05
- Responsável pelos seguintes itens:
  - Autenticação e autorização do sistema
  - Validação de token

Detalhes da implementação e execução: [Readme do projeto API com Identity](https://github.com/JairJr/TechChallenge/blob/master/TechChallenge.Identity/README.md)

### Recurso 3: API AspNetCore em .NET 7 com as implementações das regras de negócio e validações (Business Core)
- Implementa a conexão com banco de dados SQL Server no Azure e oferece as seguintes funcionalidades:
  - Criação e gerenciamento dos objetos Usuário, Foto e suas propriedades, como título e extensão.
  - Relacionamento entre as imagens e o usuário
  - Persistência de dados através do Entity Framework Core 7.05

Detalhes da implementação e execução: [Readme do projeto API Business Core](https://github.com/JairJr/TechChallenge/blob/master/TechChallange.Api/README.md)

### Recurso 4: API Mínima em .NET 7
- Responsável por salvar arquivos no Azure Storage e recuperar um SASToken com validade de 1 hora para acesso ao arquivo.

Detalhes da implementação e execução: [Readme da API Minima para genrenciamento de arquivos no Azure Storage Acount](https://github.com/JairJr/TechChallenge/blob/master/TechChallenge.MinimalApi/README.md)

A imagem abaixo representa a arquitetura definida como referência no início do projeto:

![Arquitetura](https://github.com/JairJr/TechChallenge/assets/29376086/c1825dc0-cf40-4290-84ac-b134eff7bcbd)
