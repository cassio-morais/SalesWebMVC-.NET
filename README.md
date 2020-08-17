## Sistema de vendas baseado no projeto do curso de .NET do Professor [Nélio Alves](https://www.udemy.com/course/programacao-orientada-a-objetos-csharp/) ##

#### Tecnologias usadas: ASP.NET CORE MVC, Bootstrap e Mysql

* [Diagrama do projeto](https://github.com/cassio-morais/SalesWebMVC-.NET/blob/master/img/diagrama.JPG)

  * Criação das classes e enums a partir de um diagrama UML e depois de um DbContext usando Entity Framework (CODE-FIRST). 
  * Configuração do DbContext para uso do Mysql.
  * Criação de um serviço de Seed para popular o banco de dados em mode de desenvolvimento.
  * Criação de um CRUD automático para os Departments a partir de uma ferramenta de Scaffolding do Visual Studio 2019.
  * Criação de Controllers, Views e Services (injeção de dependência) para Sellers.
  * Criação de ViewModels para composição de objetos a se mostrar na tela.
  * Criação de Exceptions personalizadas da camada de dados, passando pelo Service até a camada de controller.
  * Configuração de busca simples e agrupada por datas para as SalesRecord.
  * Configuração de locale para EUA (projeto em inglês).

## O que foi refatorado no projeto após o término do curso:

* O scaffolding feito com a classe Departments foi refeita para separação de responsabilidades em camadas.
  * DbContext > Service > Controller > View

* Criação de um cadastro de SalesRecord para cadastrar vendas para vendedores.
