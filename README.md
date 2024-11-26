- [Aplicação Quickfood (backend)](#aplicação-quickfood-backend)
  - [Funcionalidades Principais](#funcionalidades-principais)
  - [Estrutura do Projeto](#estrutura-do-projeto)
  - [Tecnologias Utilizadas](#tecnologias-utilizadas)
  - [Serviços Utilizados](#serviços-utilizados)
  - [Motivações para Utilizar o SQL Server como Banco de Dados da Aplicação](#motivações-para-utilizar-o-sql-server-como-banco-de-dados-da-aplicação)
  - [Como Executar o Projeto](#como-executar-o-projeto)
    - [Clonar o repositório](#clonar-o-repositório)
    - [Executar com docker-compose](#executar-com-docker-compose)
    - [Executar com Kubernetes](#executar-com-kubernetes)
  - [Collection com todas as APIs com exemplos de requisição](#collection-com-todas-as-apis-com-exemplos-de-requisição)
  - [Desenho da arquitetura](#desenho-da-arquitetura)
  - [Demonstração em vídeo](#demonstração-em-vídeo)
  - [Autores](#autores)
  - [Documentação Adicional](#documentação-adicional)

---

# Aplicação Quickfood (backend)

Este projeto visa o desenvolvimento do backend para um software que simula um totem de auto-atendimento de uma lanchonete.<br>
Utilizando a arquitetura limpa, .NET 8, SQL Server, Cognito, Docker e Kubernetes, o objetivo é criar uma base sólida e escalável para suportar as funcionalidades necessárias para um sistema de autoatendimento. <br>
O foco principal é a criação de uma aplicação robusta, modular e de fácil manutenção.<br>

## Funcionalidades Principais

- **Gerenciamento de Pedidos**: Criação, atualização e consulta de pedidos feitos pelos clientes. <br>
- **Gerenciamento de Cardápio**: Manutenção dos itens do cardápio, incluindo adição, remoção e atualização de produtos. <br>
- **Gerenciamento de Cliente**: Manutenção dos cliente, incluindo adição, remoção e atualização de clientes. <br>
- **Pagamento dos Pedidos**: Integração (fake) com gateway de pagamentos. <br>
- **Armazenamento de Dados**: Persistência de dados utilizando um banco de dados adequado SQL Server. <br>
- **Gerenciamento de Usuários**: Gestão de usuários (funcionários ou clientes) integrados com o Cognito, permitindo o cadastro, confirmação do e-mail e recuperação de senha. <br>

## Estrutura do Projeto

A arquitetura limpa será utilizada para garantir que a aplicação seja modular e de fácil manutenção, o projeto foi estruturado da seguinte forma: API, Controllers, Gateways, Gateways.Cognito, Presenters, Domain, Infra (implementação das interfaces de repositório, configurações de banco de dados) e Building Blocks (componentes e serviços reutilizáveis)<br>

## Tecnologias Utilizadas

- **.NET 8**: Framework principal para desenvolvimento do backend. <br>
- **Arquitetura Limpa**: Estruturação do projeto para promover a separação de preocupações e facilitar a manutenção e escalabilidade. <br>
- **Docker**: Containerização da aplicação para garantir portabilidade e facilitar o deploy. <br>
- **Kubernetes**: Orquestração dos container visando resiliência da aplicação <br>
- **Banco de Dados**: Utilização do SQL Server para armazenamento de informações. <br>

## Serviços Utilizados
- **Cognito**: Gestão e autenticação de usuários. <br>
- **Github Actions**: Todo o CI/CD é automatizado através de pipelines. <br>
- **SonarQube**: Análise estática do código para promover qualidade. <br>

## Motivações para Utilizar o SQL Server como Banco de Dados da Aplicação
Para um sistema de gerenciamento de pedidos em uma lanchonete, a escolha do SQL Server como banco de dados relacional se destaca como a mais adequada por diversas razões. Primeiramente, ele oferece uma estrutura de dados bem definida, assegurando a consistência das informações e garantindo que todas as operações sigam as propriedades ACID (Atomicidade, Consistência, Isolamento e Durabilidade), essenciais para a integridade das transações.
Além disso, o SQL Server proporciona segurança robusta, com mecanismos avançados de controle de acesso e permissões. A plataforma também facilita a geração de relatórios complexos e a manutenção dos dados, atributos cruciais para a gestão eficiente de pedidos, clientes e produtos.
Como a aplicação não demanda um alto volume de leituras e escritas nem requer alta escalabilidade, e ainda utiliza um modelo de dados estruturado, a necessidade de um banco NoSQL se torna dispensável. Dessa forma, o SQL Server atende plenamente às exigências do sistema, oferecendo um equilíbrio entre desempenho, confiabilidade e facilidade de gerenciamento.

## Como Executar o Projeto

### Clonar o repositório
```
git clone https://github.com/SofArc6Soat/quickfood-backend.git
```

### Executar com docker-compose
1. Docker (docker-compose)
1.1. Navegue até o diretório do projeto:
```
cd quickfood-backend\src\DevOps
```
2. Configure o ambiente Docker:
```
docker-compose up --build
```
2.1. A aplicação estará disponível em http://localhost:5000 ou https://localhost:5001
2.2. URL do Swagger: http://localhost:5000/swagger ou https://localhost:5001/swagger
2.3. URL do Healthcheck da API: http://localhost:5000/health ou https://localhost:5001/health

### Executar com Kubernetes
2. Kubernetes
2.1. Navegue até o diretório do projeto:
```
cd quickfood-backend\src\DevOps\kubernetes
```
2.2. Configure o ambiente Docker:
```
kubectl apply -f 01-sql-data-pvc.yaml
kubectl apply -f 02-sql-log-pvc.yaml
kubectl apply -f 03-sql-secrets-pvc.yaml
kubectl apply -f 04-quickfood-sqlserver-deployment.yaml
kubectl apply -f 05-quickfood-sqlserver-service.yaml
kubectl apply -f 06-quickfood-backend-deployment.yaml
kubectl apply -f 06-quickfood-backend-deployment.yaml
kubectl apply -f 07-quickfood-backend-service.yaml
kubectl apply -f 08-quickfood-backend-hpa.yaml
kubectl port-forward svc/quickfood-backend 8080:80
```
ou executar todos scripts via PowerShell
```
Get-ExecutionPolicy
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
.\delete-k8s-resources.ps1
.\apply-k8s-resources.ps1
```
2.3. A aplicação estará disponível em http://localhost:8080
2.4. URL do Swagger: http://localhost:8080/swagger
2.5. URL do Healthcheck da API: http://localhost:8080/health

## Collection com todas as APIs com exemplos de requisição
1. Caso deseje testar via postman com dados fake importe o arquivo "API QuickFood.postman_collection" do diretorio "src/DevOps/postman" na aplicação postman local.

2. Também é possível utilizar o Swagger disponibilizado pela aplicação (URL do Swagger: http://localhost:8080/swagger).

Para testar localmente com o Postman, a ordem de excução deverá ser a seguinte:

1. Caso deseje se autenticar pela API ao invés da função Lambda também é possível. Usuários para testes:
- Funcionário Mario (usuário com perfil admin):
E-mail: sof.arc.6soat@gmail.com
Senha: A@cdE1460
<br>
- Cliente João (usuário com perfil cliente):
E-mail: joao-teste@gmail.com
CPF: 08062759016
Senha: B@cdE0943
<br>
- Cliente Maria (usuário com perfil cliente):
E-mail: maria-teste@gmail.com
CPF: 05827307084
Senha: C@cdE0943

2. POST - {{baseUrl}}/pedidos (criar um pedido)
3. POST - {{baseUrl}}/pagamentos/checkout/:pedidoId (efetar checkout do pedido com integração fake para gerar o QRCODE do PIX)
4. POST - {{baseUrl}}/pagamentos/notificacoes/:pedidoId (simulação do WebHook que recebe que o PIX foi pago)
5. PATCH - {{baseUrl}}/pedidos/status/:pedidoId (altera status do pedido para "EmPreparacao" -> "Pronto" -> "Finalizado")

Obs.:
- Cadastrar um novo funcionário (usuário com perfil admin):
POST - {{baseUrl}}/funcionarios

- Efetuar login de funcionarios (será gerado um token JWT, utilizar a função lambda):
POST - {{baseUrl}}/usuarios/funcionario/identifique-se

- Cadastrar um novo cliente (usuário com perfil cliente):
POST - {{baseUrl}}/clientes

- Efetuar login de cliente (será gerado um token JWT, utilizar a função lambda):
POST - {{baseUrl}}/usuarios/cliente/identifique-se

- Sempre após o cadastro de um usuário, será enviado um email com o código de verificação. Para efetar a confirmação, acesse:
POST - {{baseUrl}}/usuarios/email-verificacao:confirmar

- Caso tenha esquecido a senha, acesse para poder prosseguir com a troca da senha:
POST - {{baseUrl}}/usuarios/esquecia-senha:solicitar

- Para redefinir sua senha, acesse:
POST - {{baseUrl}}/usuarios/esquecia-senha:resetar

## Desenho da arquitetura
Para visualizar o desenho da arquitetura abra o arquivo "Arquitetura.drawio.png" no diretório "arquitetura" ou importe o arquivo "Arquitetura.drawio" no Draw.io (https://app.diagrams.net/).

## Demonstração em vídeo
Para visualizar a demonstração da aplicação da Fase 2 acesse o seguinte link do Vimeo: https://vimeo.com/992479829/d8dba62df1

## Autores

- **Anderson Lopez de Andrade RM: 350452** <br>
- **Henrique Alonso Vicente RM: 354583**<br>

## Documentação Adicional

- **Miro - Domain Storytelling, Context Map, Linguagem Ubíqua e Event Storming**: [Link para o Event Storming](https://miro.com/app/board/uXjVKST91sw=/)
- **Github - Domain Storytelling**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-domain-story-telling)
- **Github - Context Map**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-ubiquitous-language)
- **Github - Linguagem Ubíqua**: [Link para o Domain Storytelling](https://github.com/SofArc6Soat/quickfood-ubiquitous-language)
