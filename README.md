# API de produtos e microsserviço de Exportação de Dados

## 1. Objetivos

- Criar uma Web API para gerenciamento e exportação de dados de produtos de mercado.
- Criar um micro serviço que funcionará com uma mensageria que terá como propósito exportar os dados de um produto num arquivo JSON.

## 2. Escopo e Funcionalidades

### a. Contextos

- Essa aplicação terá 2 contextos de dados: **PRODUTO** e **CATEGORIA**.

### b. Estrutura de dados da categoria

- ID  
- Nome  
- Ativo  
- Deletado  
- Data de inclusão  
- Data de atualização  

### c. Estrutura de dados do produto

- ID  
- Nome  
- Ativo  
- Deletado  
- Data de inclusão  
- Data de atualização  
- ID da Categoria  

### d. Web API

- A API deverá conter pelo menos 4 endpoints, um para cada operação de CRUD: salvar, editar, listar e excluir.
- Endpoints extras podem ser criados se necessário. Esses grupos de endpoints devem ser criados para cada um dos contextos.
- Especificamente endpoints de listagem devem considerar a existência de filtros, permitindo filtrar por qualquer um dos campos da tabela, com paginação padrão de 10 registros por página.

### e. Microsserviço

- O microsserviço terá unicamente como responsabilidade receber algum identificador do produto para que seja possível resgatá-lo do banco de dados e exportá-lo.

## 3. Regras e Definições

- A API deve ser desenvolvida na linguagem **Java** com **Spring Boot**.
- Incluir **Swagger** para execução dos endpoints.
- O micro serviço pode ser desenvolvido em qualquer outra linguagem de sua preferência.
- O banco de dados obrigatoriamente deve ser o **MongoDB**.
- Serviço de mensageria: utilizar o **RabbitMQ**, e o nome da fila deve ser `"product/export.data"`.
  - Se não houver conteúdos de estudos nas trilhas sobre mensagerias, pode buscar na internet/youtube mesmo e tentar aplicar os conceitos básicos.
- Todos os endpoints devem seguir a padronização de **kebab-case**.
- Categorias e produtos devem ter um **delete lógico**, ou seja, devem permanecer no banco marcados como deletados. Uma vez deletado, o registro deve se tornar inválido para uso em qualquer operação CRUD. Considerar validação para esse cenário.
- Os campos de **data de inclusão** e **data de atualização** devem ser preenchidos dinamicamente durante as operações e **não devem ser providos via payload** nos requests.
- Os demais campos de ambos os contextos devem ser considerados como **obrigatórios**. Considerar validação para isso também.
- Não deve ser permitido **duplicatas**, ou seja, produtos com o mesmo nome e vinculados à mesma categoria. É permitido repetir a categoria entre produtos, desde que tenham nomes diferentes.
- Todo e qualquer erro deve ser tratado e retornado no response com **HTTP BadRequest** e com um corpo de mensagem padronizado em JSON, no seguinte modelo:

```json
{
  "errorMessage": "conteúdo da mensagem aqui"
}
```

## 4. Comunicação API x Microsserviço

- A integração entre as duas aplicações deverá ser via mensageria.
- O gatilho para disparar essa comunicação será um **endpoint exclusivo**, que terá como parâmetro apenas o ID do produto.
- A exportação do produto deverá ser no formato de arquivo JSON, contendo apenas os campos:
  - ID  
  - Nome do produto  
  - Nome da categoria  
- O arquivo exportado deverá ser nomeado com o ID do produto concatenado à data e hora da exportação, no seguinte formato:

```
<ID_YYYYMMDD_HHMMSS>.json
```

Exemplo:  
```
67608e8fad5aab24917e83f0_20250417_155752.json
```

- Uma vez que o produto foi exportado:
  - Marcar na tabela do banco de dados:
    - Um campo boolean informando que foi exportado
    - Um campo com a data em que foi exportado

- Em caso de falha na exportação:
  - Armazenar em outra tabela no banco de dados:
    - ID do produto  
    - Mensagem do erro  
    - Data e hora do ocorrido  
  - **Não** fazer as marcações do produto como exportado

- Deve haver uma validação para **prevenir múltiplas exportações do mesmo produto**, ou seja, uma vez que foi exportado, o mesmo produto **não poderá ser exportado novamente**.

## 5. Requisitos Adicionais

- Ambas as aplicações devem estar com pelo menos **90% de cobertura com testes unitários**.

## 6. Requisitos Opcionais

- Implementação de **autenticação JWT** em todas as rotas.
