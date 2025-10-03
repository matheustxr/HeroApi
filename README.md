# Desafio Full Stack - Gerenciamento de Super-Heróis

Este repositório contém a solução para o desafio de Desenvolvedor Full Stack, focado na criação de uma API REST para gerenciamento de super-heróis.

## Sobre o Projeto

O objetivo do projeto é desenvolver uma aplicação web completa, composta por um backend em .NET e um frontend em React, para realizar operações de CRUD (Create, Read, Update, Delete) em uma base de dados de super-heróis.

## Tecnologias Utilizadas

- **.NET 8**: Plataforma de desenvolvimento.
- **ASP.NET Core Web API**: Framework para a construção da API REST.
- **Entity Framework Core 8**: ORM para acesso e manipulação de dados.
- **Banco de Dados em Memória**: Provedor do EF Core utilizado para agilidade no desenvolvimento e testes, conforme permitido pelo desafio.
- **Swagger (OpenAPI)**: Ferramenta para documentação e teste interativo dos endpoints da API.

## Funcionalidades Implementadas

A API implementa todas as funcionalidades CRUD solicitadas para os super-heróis:

- ✅ **Cadastro (Create)**: Cria um novo super-herói, associando um ou mais superpoderes.
- ✅ **Listagem (Read)**: Retorna a lista de todos os heróis cadastrados.
- ✅ **Consulta por ID (Read)**: Busca e retorna um herói específico pelo seu ID.
- ✅ **Atualização (Update)**: Altera os dados de um herói existente.
- ✅ **Exclusão (Delete)**: Remove um herói da base de dados.Swagger.

## Como Executar

**Pré-requisitos:**
- .NET 8 SDK

**Passos:**
1. Clone o repositório.
2. Navegue até a pasta do projeto da API.
3. Execute a aplicação com o comando:
   ```sh
   dotnet run
4. A API estará em execução e pronta para receber requisições.

## Seed de Dados
Ao iniciar a aplicação, o banco de dados em memória é automaticamente preenchido com 5 superpoderes de exemplo, com IDs de 1 a 5. Isso facilita o uso da API sem precisar cadastrar poderes manualmente.

| Id  | Super poder |
| -------- | ----- | 
| 1        | Super Força     | 
| 2        | Invisibilidade     | 
| 3        | Velocidade Extrema   | 
| 4        | Telepatia     |
| 5       | Controle do Tempo    | 


## Decisões de Arquitetura

A arquitetura escolhida buscou um equilíbrio entre a **simplicidade**, adequada ao escopo do projeto, e a aplicação de **boas práticas** de desenvolvimento.

- **Estrutura em Camadas**: Adotamos uma separação clara de responsabilidades para manter o código organizado e de fácil manutenção.
  - **Controllers**: Camada responsável apenas por gerenciar as requisições e respostas HTTP.
  - **Services**: Camada que centraliza toda a lógica de negócio e validações.
  - **Data (DbContext)**: Camada de acesso aos dados, abstraída pelo Entity Framework.

- **Injeção de Dependência (DI)**: Utilizamos a injeção de dependência nativa do ASP.NET Core para fornecer o `HeroService` ao `HeroController` através de sua interface (`IHeroService`). Essa abordagem desacopla as camadas, facilita os testes e a manutenção do código.

- **DTOs (Data Transfer Objects)**: Criamos as classes `RequestHeroJson` e `ResponseHeroJson` para servir como o "contrato" da API. Isso evita expor diretamente as entidades do banco de dados e nos dá flexibilidade para moldar os dados enviados e recebidos.
