# 🏥 Saúde Fácil — Marketplace & Comparador de Medicamentos Local

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-512BD4?style=for-the-badge&logo=dotnet)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=for-the-badge&logo=dotnet)
![Status](https://img.shields.io/badge/Status-Conclu%C3%ADdo-brightgreen?style=for-the-badge)

O **Saúde Fácil** é uma plataforma e marketplace web inteligente projetado para aproximar os consumidores de farmácias e drogarias de bairro. A solução permite que usuários pesquisem, comparem preços em tempo real, verifiquem a proximidade geográfica de estabelecimentos físicos e efetuem compras com diferentes opções de entrega e pagamento, ao mesmo tempo em que oferece às farmácias um ecossistema simplificado para gestão e importação em lote de inventário.

---

## 📌 Sumário
- [Visão Geral e Proposta de Valor](#-visão-geral-e-proposta-de-valor)
- [Principais Funcionalidades](#-principais-funcionalidades)
- [Arquitetura e Tecnologias](#-arquitetura-e-tecnologias)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Como Executar o Projeto](#-como-executar-o-projeto)
  - [Pré-requisitos](#pré-requisitos)
  - [Passo a Passo (VS Code / CLI / Visual Studio)](#passo-a-passo-vs-code--cli--visual-studio)
  - [Configuração e Migração do Banco de Dados](#configuração-e-migração-do-banco-de-dados)
- [Importação de Estoque em Lote (Farmácias)](#-importação-de-estoque-em-lote-farmácias)
- [Autor](#-autor)

---

## 🎯 Visão Geral e Proposta de Valor

O comércio farmacêutico de bairro muitas vezes enfrenta desafios de visibilidade digital frente às grandes redes. O **Saúde Fácil** resolve essa assimetria centralizando o catálogo e estoque de drogarias locais em um único portal interativo.

### Objetivos do Sistema:
- **Para o Consumidor:** Encontrar medicamentos pelo menor preço, identificar drogarias mais próximas através de geolocalização por GPS/raio, gerenciar carrinho de compras e realizar checkout de forma simples.
- **Para a Farmácia Parceira:** Painel exclusivo protegido por autenticação com funcionalidade de carregamento massivo de produtos (Excel/`.xlsx`), integrando seu estoque físico diretamente com a vitrine digital.

---

## ⚙️ Principais Funcionalidades

1. **Vitrine & Catálogo Dinâmico:**
   - Exibição de medicamentos mais procurados, preços promocionais e detalhes do produto.
   - Filtragem por categorias como *Analgésicos e Térmicos*, *Anti-inflamatórios*, *Vitaminas e Suplementos*.

2. **Busca Avançada de Medicamentos:**
   - Pesquisa por nome comercial ou princípio ativo com contagem e filtragem de resultados em tempo real.

3. **Geolocalização de Farmácias ("Mais Próximas"):**
   - Mapeamento dinâmico e cálculo de distância (em km) a partir do local atual do usuário, listando endereço completo e botão de acesso direto ao catálogo da loja.

4. **Autenticação Segura & Perfis Segregados:**
   - Login unificado para clientes e estabelecimentos.
   - Cadastro segregado para **Clientes** (dados pessoais e endereço de entrega) e **Farmácias** (dados corporativos e CNPJ).

5. **Carrinho & Checkout Completo:**
   - Gestão de itens na sacola de compras persistida em sessão.
   - Cálculo automático de frete/taxas conforme modalidade (*Entrega Rápida Delivery*).
   - Métodos de pagamento integrados (PIX) com geração de recibo imediato acompanhado de um código `UUID` único de transação.

6. **Módulo do Parceiro — Importação de Estoque em Lote:**
   - Upload de planilhas Excel (`.xlsx`) com validação de colunas (*Nome, Categoria, Descricao, Preco, Estoque, ExigeReceita*).
   - Associação e bloqueio automático do ID do estabelecimento por meio de *Claims* de segurança.

7. **Assistente Virtual (Chatbot FAQ):**
   - Widget flutuante integrado no rodapé de todas as páginas para sanar dúvidas recorrentes dos usuários de forma autônoma.

---

## 🛠 Arquitetura e Tecnologias

A aplicação segue o padrão arquitetural **MVC (Model-View-Controller)** sobre a plataforma .NET.

* **Backend / Framework:** C# com .NET 9 SDK (ASP.NET Core MVC / Web API)
* **Persistência / ORM:** Entity Framework Core
* **Frontend:** Razor Views, HTML5, CSS3 e Bootstrap 5
* **Algoritmos Especiais:** Cálculo de distâncias geográficas via fórmula de Haversine (`GeoUtils.cs`)
* **Processamento de Arquivos:** Biblioteca de manipulação de planilhas `.xlsx` em C#
* **Autenticação:** ASP.NET Core Identity / Claims-Based Authentication

---

## 📁 Estrutura do Projeto

```
SaudeFacil/
├── Controllers/         # Controladores da aplicação (Home, Produtos, Carrinho, Account, etc.)
├── Models/              # Entidades do domínio (Cliente, Farmacia, Produto, Pedido, etc.)
├── Data/                # Contexto do Entity Framework e Mapeamentos (AppDbContext)
├── Services/            # Regras de negócio e utilitários (GeoUtils, ExcelImporter)
├── Views/               # Páginas Razor (Interface do Usuário com Bootstrap)
│   ├── Home/            # Vitrine principal e página Sobre Nós
│   ├── Farmacia/        # Páginas de geolocalização e catálogo por estabelecimento
│   ├── Estoque/         # Área restrita para importação em lote
│   └── Shared/          # Layouts e componentes reutilizáveis (Header, Chatbot FAQ)
├── wwwroot/             # Arquivos estáticos (CSS, JS, Imagens)
├── appsettings.json     # Configurações globais e Connection Strings
└── Program.cs           # Ponto de entrada, injeção de dependências e middlewares
```

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) instalado na máquina.
* IDE de sua preferência: [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/) (com a extensão C# Dev Kit).
* Banco de Dados (SQL Server / LocalDB ou SQLite, conforme configurado na Connection String).

### Passo a Passo (VS Code / CLI / Visual Studio)

1. **Clonar o Repositório:**
   ```bash
   git clone https://github.com/seu-usuario/saude-facil.git
   cd saude-facil
   ```

2. **Restaurar Dependências:**
   ```bash
   dotnet restore
   ```

3. **Compilar a Aplicação:**
   ```bash
   dotnet build
   ```

4. **Executar o Projeto:**
   ```bash
   dotnet run
   ```
   Acesse a aplicação no navegador através do endereço informado no terminal (ex: `https://localhost:7xxx` ou `http://localhost:5xxx`).

---

### Configuração e Migração do Banco de Dados

Caso a aplicação não aplique as migrações automaticamente no momento da inicialização (`Database.Migrate()`), você pode atualizar o banco manualmente executando o comando:

```bash
dotnet ef database update
```
*(Nota: Certifique-se de que a ferramenta `dotnet-ef` esteja instalada globalmente na sua máquina).*

---

## 📊 Importação de Estoque em Lote (Farmácias)

Para que uma farmácia parceira consiga importar seus medicamentos via planilha Excel, a estrutura das colunas na folha de cálculo deve seguir exatamente o padrão abaixo:

| Nome | Categoria | Descricao | Preco | Estoque | ExigeReceita |
| :--- | :--- | :--- | :--- | :--- | :--- |
| Ibuprofeno 600mg | Anti-inflamatórios | Indicado para dores leves a moderadas. | 18.20 | 50 | false |
| Vitamina D3 2.000 UI | Vitaminas e Suplementos | Auxilia no sistema imunológico. | 28.70 | 120 | false |

---

## 👩‍💻 Autores

Desenvolvido por **Gabrielle dos Santos Rodrigues**
Supervisionado por **Jean Barbosa**  
*Versão do Sistema: V.0.1*  
*Ano de Documentação: 2026*