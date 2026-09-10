# ContractFlow

Sistema web para gestão de contratos empresariais, desenvolvido como projeto de portfólio com foco em arquitetura moderna, segurança, organização de código e boas práticas de desenvolvimento.

## Objetivo

O ContractFlow centraliza contratos, fornecedores, aprovações e auditoria em uma única plataforma, ajudando empresas a reduzir riscos, evitar perda de prazos e manter maior controle sobre seus documentos e processos.

## Funcionalidades

- Autenticação com JWT
- Controle de acesso por perfis
- Cadastro de contratos
- Cadastro de fornecedores
- Aprovação e rejeição de contratos
- Dashboard com indicadores
- Contratos próximos do vencimento
- Auditoria de operações
- Dados fictícios para demonstração
- Validação de regras de negócio
- Logs estruturados da API
- Testes automatizados
- Containerização com Docker
- CI com GitHub Actions

## Tecnologias

### Backend

- C#
- .NET 10
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication
- xUnit

### Frontend

- Angular
- TypeScript
- HTML
- CSS
- RxJS

### DevOps

- Docker
- Docker Compose
- Git
- GitHub Actions

## Arquitetura

O projeto está organizado em camadas:

```text
ContractFlow
├── backend
│   ├── ContractFlow.Api
│   ├── ContractFlow.Application
│   ├── ContractFlow.Domain
│   ├── ContractFlow.Infrastructure
│   └── ContractFlow.Tests
├── frontend
│   └── contractflow-web
├── .github
│   └── workflows
├── docker-compose.yml
└── README.md