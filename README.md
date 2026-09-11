# ContractFlow

Sistema Full Stack para **gestão de contratos empresariais e fornecedores**, desenvolvido como projeto de portfólio com foco em arquitetura, segurança, qualidade de código, testes automatizados e integração contínua.

O ContractFlow centraliza contratos, fornecedores, aprovações, vencimentos e auditoria em uma única aplicação, reduzindo a dependência de planilhas, e-mails e controles manuais.

---

## Sobre o projeto

Empresas precisam acompanhar contratos, fornecedores, valores, responsáveis, aprovações e principalmente datas de vencimento.

O ContractFlow foi desenvolvido para centralizar essas informações e facilitar o acompanhamento do ciclo de vida dos contratos.

A aplicação permite:

- cadastrar e gerenciar contratos;
- cadastrar e gerenciar fornecedores;
- acompanhar contratos ativos e vencidos;
- identificar contratos próximos do vencimento;
- controlar contratos aguardando aprovação;
- aprovar ou rejeitar contratos;
- controlar permissões por perfil de usuário;
- acompanhar indicadores através de dashboard;
- registrar ações importantes através de auditoria.

---

## Arquitetura

O projeto utiliza uma arquitetura em camadas, separando responsabilidades entre domínio, aplicação, infraestrutura, API e interface web.

```text
                         USUÁRIO
                            │
                            ▼
                  ┌───────────────────┐
                  │      Angular      │
                  │    TypeScript     │
                  └─────────┬─────────┘
                            │
                      HTTP / JSON
                       REST API
                            │
                            ▼
                  ┌───────────────────┐
                  │ ASP.NET Core API  │
                  │      .NET 10      │
                  └─────────┬─────────┘
                            │
                  JWT / Authorization
                            │
                            ▼
                  ┌───────────────────┐
                  │    Application    │
                  │     Services      │
                  └─────────┬─────────┘
                            │
                            ▼
                  ┌───────────────────┐
                  │  Infrastructure   │
                  │ Entity Framework  │
                  │       Core        │
                  └─────────┬─────────┘
                            │
                            ▼
                  ┌───────────────────┐
                  │    SQL Server     │
                  │  ContractFlowDb   │
                  └───────────────────┘
```

Além do fluxo principal, a aplicação possui autenticação JWT, autorização baseada em perfis, logs estruturados, auditoria, testes automatizados, Docker e pipeline CI/CD.

---

## Tecnologias utilizadas

### Backend

- C#
- .NET 10
- ASP.NET Core
- REST API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Role-based Authorization

### Frontend

- Angular
- TypeScript
- HTML
- CSS
- Angular Router
- HTTP Interceptors
- Route Guards

### Qualidade e DevOps

- xUnit
- Git
- GitHub
- GitHub Actions
- Docker
- Docker Compose
- Logs estruturados

---

## Estrutura do projeto

```text
ContractFlow
│
├── backend
│   ├── ContractFlow.Api
│   ├── ContractFlow.Application
│   ├── ContractFlow.Domain
│   ├── ContractFlow.Infrastructure
│   └── ContractFlow.Tests
│
├── frontend
│   └── contractflow-web
│
├── docs
│
├── .github
│   └── workflows
│       └── ci.yml
│
├── docker-compose.yml
├── .dockerignore
├── .gitignore
└── README.md
```

---

## Backend

### ContractFlow.Domain

Camada responsável pelas entidades e regras centrais do domínio.

Principais entidades:

- Contract
- Supplier
- User
- AuditLog

Também contém os status utilizados durante o ciclo de vida dos contratos.

### ContractFlow.Application

Responsável pelos contratos da aplicação através de DTOs e interfaces de serviços.

Inclui recursos relacionados a:

- contratos;
- fornecedores;
- autenticação;
- dashboard;
- auditoria;
- usuário autenticado.

### ContractFlow.Infrastructure

Implementa acesso a dados e regras de infraestrutura.

Inclui:

- Entity Framework Core;
- DbContext;
- migrations;
- serviços;
- autenticação;
- geração de JWT;
- auditoria;
- dados demonstrativos.

### ContractFlow.Api

Expõe os recursos através de uma REST API desenvolvida com ASP.NET Core.

Principais áreas:

- Auth
- Contracts
- Suppliers
- Dashboard
- Audit

---

## Funcionalidades

### Dashboard

Apresenta uma visão consolidada dos contratos da empresa, incluindo:

- total de contratos;
- contratos ativos;
- contratos vencidos;
- contratos aguardando aprovação;
- valor total dos contratos;
- contratos com vencimento próximo.

### Gestão de contratos

Permite:

- cadastrar contratos;
- editar contratos;
- excluir contratos;
- consultar contratos;
- associar fornecedores;
- definir período de vigência;
- controlar valores;
- acompanhar status;
- identificar contratos próximos do vencimento.

### Gestão de fornecedores

Permite:

- cadastrar fornecedores;
- editar fornecedores;
- consultar fornecedores;
- excluir fornecedores;
- relacionar fornecedores aos contratos.

### Fluxo de aprovação

Contratos podem passar por um processo de aprovação.

Usuários autorizados podem:

- visualizar contratos pendentes;
- aprovar contratos;
- rejeitar contratos.

### Auditoria

A aplicação registra ações relevantes para permitir maior rastreabilidade das operações realizadas no sistema.

---

## Status dos contratos

O ContractFlow trabalha com diferentes etapas do ciclo de vida:

```text
Rascunho
   │
   ▼
Aguardando Aprovação
   │
   ├──────────────► Rejeitado
   │
   ▼
Ativo
   │
   ▼
Vencido

Também é possível:
Ativo / Rascunho ──► Cancelado
```

---

## Segurança

A aplicação utiliza **JWT (JSON Web Token)** para autenticação.

O fluxo básico é:

```text
Login
  │
  ▼
Validação das credenciais
  │
  ▼
Geração do JWT
  │
  ▼
Token enviado ao frontend
  │
  ▼
Angular envia o token nas requisições
  │
  ▼
API valida autenticação e permissões
```

As chaves sensíveis não são armazenadas diretamente no código-fonte.

Durante o desenvolvimento, configurações sensíveis podem ser fornecidas através de:

- .NET User Secrets;
- variáveis de ambiente;
- arquivo `.env` local não versionado.

---

## Controle de acesso

A aplicação possui autorização baseada em roles.

### User

Possui acesso às funcionalidades permitidas para usuários comuns.

### Manager

Possui permissões administrativas adicionais, incluindo operações relacionadas à aprovação e gerenciamento de registros.

A autorização também é validada no backend, evitando depender apenas das restrições da interface.

---

## Banco de dados

Banco utilizado:

**SQL Server**

Database:

```text
ContractFlowDb
```

O acesso aos dados é realizado utilizando **Entity Framework Core**.

As alterações da estrutura do banco são controladas através de **Migrations**.

---

## Dados demonstrativos

O projeto possui um `DemoDataSeeder` responsável por disponibilizar dados demonstrativos para facilitar testes e apresentação do sistema.

Isso permite visualizar dashboards, contratos, fornecedores e diferentes situações do fluxo da aplicação.

---

## Testes automatizados

Os testes foram desenvolvidos utilizando **xUnit**.

Última execução local:

```text
Total: 8
Bem-sucedidos: 8
Falharam: 0
Ignorados: 0
```

Resultado:

**8/8 testes aprovados.**

Os testes cobrem regras importantes da camada de serviços e do fluxo de contratos.

Para executar:

```bash
dotnet test backend/ContractFlow.Tests/ContractFlow.Tests.csproj --configuration Release
```

---

## CI/CD

O projeto utiliza **GitHub Actions** para integração contínua.

A cada push ou pull request para a branch `main`, o pipeline realiza validações automáticas do projeto.

### Backend

```text
Checkout
   ↓
Setup .NET
   ↓
Restore
   ↓
Build
   ↓
Tests
```

### Frontend

```text
Checkout
   ↓
Setup Node.js
   ↓
npm ci
   ↓
Angular Build
```

O pipeline permite detectar problemas de compilação ou testes antes que alterações sejam consideradas válidas.

---

## Docker

O projeto possui configuração para containerização.

Estão disponíveis:

- Dockerfile da API;
- Dockerfile do frontend;
- Docker Compose;
- configuração para SQL Server.

Estrutura conceitual:

```text
┌──────────────────┐
│ Angular / Nginx  │
│     :4200        │
└────────┬─────────┘
         │
         ▼
┌──────────────────┐
│ ASP.NET Core API │
│     :5149        │
└────────┬─────────┘
         │
         ▼
┌──────────────────┐
│    SQL Server    │
│      :1433       │
└──────────────────┘
```

As credenciais utilizadas pelo Docker Compose devem ser fornecidas através de variáveis de ambiente.

---

## Observabilidade

O backend possui logging estruturado em JSON.

Também existe registro do tempo de processamento das requisições HTTP.

Exemplo conceitual:

```text
HTTP GET /api/contracts respondeu 200 em 42ms
```

Isso facilita:

- diagnóstico de problemas;
- acompanhamento das requisições;
- rastreabilidade;
- futura integração com plataformas de monitoramento.

---

## Executando o projeto localmente

### Pré-requisitos

Para executar o projeto localmente:

- .NET 10 SDK
- Node.js
- npm
- Angular CLI
- SQL Server
- Git

---

### 1. Clonar o projeto

```bash
git clone https://github.com/igorkaynan/ContractFlow.git
cd ContractFlow
```

---

### 2. Configurar o backend

A aplicação utiliza uma connection string semelhante a:

```text
Server=localhost;
Database=ContractFlowDb;
Trusted_Connection=True;
TrustServerCertificate=True;
```

A chave JWT deve ser configurada localmente através de User Secrets ou variável de ambiente.

Exemplo com User Secrets:

```bash
dotnet user-secrets init --project backend/ContractFlow.Api
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_LOCAL" --project backend/ContractFlow.Api
```

Nunca utilize uma chave real de produção diretamente no repositório.

---

### 3. Executar migrations

```bash
dotnet ef database update --project backend/ContractFlow.Infrastructure --startup-project backend/ContractFlow.Api
```

---

### 4. Executar backend

```bash
dotnet run --project backend/ContractFlow.Api
```

API local:

```text
http://localhost:5149
```

---

### 5. Executar frontend

Abra outro terminal:

```bash
cd frontend/contractflow-web
npm install
npm start
```

Frontend:

```text
http://localhost:4200
```

---

## Screenshots

Crie a pasta:

```text
docs/screenshots
```

E adicione as principais telas do sistema.

Sugestão:

```text
docs/screenshots/dashboard.png
docs/screenshots/contracts.png
docs/screenshots/suppliers.png
docs/screenshots/approvals.png
docs/screenshots/audit.png
```

Depois você pode exibi-las aqui:

### Login
<img width="1366" height="768" alt="0 - login" src="https://github.com/user-attachments/assets/2ac4e7ef-5a50-438d-b68a-2583bce8a2d1" />

### Dashboard
<img width="1366" height="768" alt="1-dashboard" src="https://github.com/user-attachments/assets/1c088df8-2b4b-4aeb-bc24-7400ab2afa1d" />

### Contratos
<img width="1366" height="768" alt="2-contratos" src="https://github.com/user-attachments/assets/16ec9f8d-e132-457f-980e-fe4cafd99c13" />

### Fornecedores
<img width="1364" height="768" alt="3-fornecedores" src="https://github.com/user-attachments/assets/8de54d4d-ae2e-45ba-a685-2ebc1d75c6b1" />

### Aprovações
<img width="1366" height="768" alt="4-aprovacoes" src="https://github.com/user-attachments/assets/fba90094-e374-48cd-8c90-f4b3b094eb0f" />

### Auditoria
<img width="1366" height="768" alt="5-auditoria" src="https://github.com/user-attachments/assets/2152666d-af33-4b8a-8e0b-576efa0e512e" />

---

## Fluxo geral

```text
                    ┌─────────────┐
                    │   Usuário   │
                    └──────┬──────┘
                           │
                           ▼
                 ┌──────────────────┐
                 │ Angular Frontend │
                 └────────┬─────────┘
                          │
                     REST / JSON
                          │
                          ▼
                 ┌──────────────────┐
                 │ ASP.NET Core API │
                 └────────┬─────────┘
                          │
                ┌─────────┴─────────┐
                │                   │
                ▼                   ▼
          Autenticação          Autorização
              JWT                 Roles
                │                   │
                └─────────┬─────────┘
                          ▼
                 ┌──────────────────┐
                 │ Application /    │
                 │ Services         │
                 └────────┬─────────┘
                          │
                          ▼
                 ┌──────────────────┐
                 │ Entity Framework │
                 │ Core             │
                 └────────┬─────────┘
                          │
                          ▼
                 ┌──────────────────┐
                 │    SQL Server    │
                 └──────────────────┘
```

---

## Diferenciais técnicos

O projeto demonstra aplicação prática de conceitos utilizados em desenvolvimento Full Stack moderno:

- arquitetura em camadas;
- separação de responsabilidades;
- REST API;
- autenticação JWT;
- autorização baseada em roles;
- Entity Framework Core;
- migrations;
- SQL Server;
- Angular;
- TypeScript;
- route guards;
- interceptors HTTP;
- testes automatizados;
- Docker;
- Docker Compose;
- CI/CD;
- GitHub Actions;
- logging estruturado;
- auditoria;
- controle de regras de negócio.

---

## Objetivo

O ContractFlow foi desenvolvido como um projeto de portfólio para demonstrar a construção de uma aplicação empresarial Full Stack completa, passando por:

**arquitetura → backend → banco de dados → segurança → frontend → testes → containerização → CI/CD → observabilidade.**

Mais do que uma demonstração visual, o objetivo é apresentar uma solução funcional estruturada com práticas utilizadas em projetos reais.

---

## Autor

**Igor Kaynan**

Desenvolvedor Full Stack

GitHub: `github.com/igorkaynan`

---

## Status do projeto

**Concluído**

- Backend funcionando
- Frontend funcionando
- Banco de dados integrado
- Autenticação JWT
- Controle por roles
- Dashboard
- Gestão de contratos
- Gestão de fornecedores
- Aprovações
- Auditoria
- 8/8 testes automatizados aprovados
- Docker configurado
- GitHub Actions configurado
- Pipeline CI executado com sucesso
