# Dapper Fluent ORM

## Introdução

Pacote que permite o uso do Dapper como um ORM contendo as seguintes funcionalidades:

* **Migrations**: Controle via tabela e permitindo ser extendido.
* **Pesquisas com _lambda_**
* **Geração automática das queries**: Utilizando o fluent já é compatível com vários bancos, como `Postgres`, `SQL Server`, `MySQL` e `SQLite`
* **Gestão das conexões**: todas as conexões são abertas e fechadas para cada execução no repositório.
* **Validação automática das entidades**: Permite que uma entidade seja validada automaticamente antes de ser enviada ao banco de acordo com as configurações do mapeamento.

## Dependências

Para tal são usadas os seguintes pacotes:

* [Dapper](https://www.nuget.org/packages/Dapper)
* [FluentMigrator](https://www.nuget.org/packages/FluentMigrator/)
* [FluentMigrator.Runner](https://www.nuget.org/packages/FluentMigrator.Runner)
* [Dapper.FluentMap](https://www.nuget.org/packages/Dapper.FluentMap)
* [Dapper.FluentMap.Dommel](https://www.nuget.org/packages/Dapper.FluentMap.Dommel)

Além disso, para a conexão com **Postgres** - única implementada até então - são utilizadas as seguintes dependências:

* [Npgsql](https://www.nuget.org/packages/Npgsql)
* [FluentMigrator.Runner.Postgres](https://www.nuget.org/packages/FluentMigrator.Runner.Postgres)

OBS: _Nenhuma dessas referências precisa ser importada para o projeto que consome esse pacote, apenas o pacote em si._

## Utilizando o pacote

### Injetando as dependências

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Inclui o migration para postgres. Para outros bancos o nome seria equivalente.
    services.AddPostgresRepositoryWithMigration(Configuration["ConnectionString"]);
    // Adiciona a configuração do mapeamento das entidades
    services.AddMapperConfiguration<MapperConfiguration>();
    // Adiciona o IDapperORMRunner
    services.AddDapperORM();

    ...
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDapperORMRunner dapper)
{
    // Cria os mapeamentos e, baseado neles, o migration
    dapper.AddMapsAndRunMigrations();
    ...
}
```

### Criando os mapeamentos

Para implementar um mapeamento, basta fazer de forma semelhante ao exemplo abaixo:

```csharp

    // É necessário herdar de DapperFluentEntity para definir o maepamento
    public class PublicSchemaEntityMap : DapperFluentEntityMap<PublicSchemaEntity>
    {
        // Apenas as propriedades mapeadas serão transformadas em colunas. As outras serão ignoradas.
        public PublicSchemaEntityMap(string schema)
            : base(schema)
        {
            // Nome da tabela e seu respectivo schema
            ToTable("sampleentity", schema);

            // Informa que a entidade deverá ser validada em cada Insert ou Update dentro do repositório de acordo com as informações do banco antes de enviar para o database.
            WithEntityValidation();   

            // Identifica uma coluna como chave e identity         
            MapToColumn(x => x.Id).IsKey().IsIdentity();

            // Atribui valor default e que não pode ser nulo.
            MapToColumn(x => x.IntProperty).Default(5).NotNull();

            // Define tamanho máximo para a coluna (aplicável apenas a strings)
            MapToColumn(x => x.LimitedTextProperty).WithLenght(255);
            MapToColumn(x => x.TextProperty).NotNull();

            // Utiliza nome da coluna diferente da propriedade
            Map(x => x.DateProperty).ToColumn("datepp");
            Map(x => x.DecimalProperty).ToColumn("decimalpp");
            MapToColumn(x => x.BooleanProperty);

            // Cria chave estrangeira com outra tabela.
            MapToColumn(x => x.CategoryId).ForeignKeyFor<Category>("id");
        }
    }

```

##### Observações

* O método `MapToColumn` cria a coluna com o mesmo nome da propriedade, mas em minúsculo e sem ser _case sensitive_.
* Para que o `ForeignKeyFor` funcione corretamente é necessário adicionar os mappers na ordem correta: primeiro o mapper referenciado e depois a tabela que o referencia.


### Declarando os mapeamentos

Após criar os mapeamentos é necessário implementar a interface `IMapperConfiguration` e configurar os mapeamentos. Essa implementação deve ser conforme abaixo:

```csharp
    public class MapperConfiguration : IMapperConfiguration
    {
        public void ConfigureMappers()
        {
            var defaultSchema = "schema";
            // Note que o mapeamento para Category vem antes do PublicSchemaEntity
            FluentMapping.AddMap(new CategoryMap(defaultSchema));
            FluentMapping.AddMap(new PublicSchemaEntityMap(defaultSchema));
            FluentMapping.AddMap(new LogEntityMap(defaultSchema));
        }
    }
```

Após criar a configuração, basta injetar:

```csharp
services.AddMapperConfiguration<MapperConfiguration>();
```

**As tabela serão criadas automaticamente a partir dos mapeamentos caso não existam na base, seguindo as regras do Migration.**

## Consumindo o repositório

```csharp
    public class PublicSchemaEntityRepository : IPublicSchemaEntityRepository
    {
        private readonly IPostgresRepository<PublicSchemaEntity> _repository;

        // A injeção de IPostgresRepository<TEntity> já foi configurada no startup para qualquer valor de TEntity
        public PublicSchemaEntityRepository(IPostgresRepository<PublicSchemaEntity> repository)
        {
            this._repository = repository;
        }

        // Utiliza lambda com linq para realizar as queries.
        public void Delete(int id) => _repository.Remove(x => x.Id == id);
        public PublicSchemaEntity Get(int id) => _repository.Find(x => x.Id == id);
        public IEnumerable<PublicSchemaEntity> GetAll() => _repository.All();
        public int Insert(PublicSchemaEntity entity) => _repository.Add(entity);
        public bool Update(PublicSchemaEntity entity) => _repository.Update(entity);

        // Exemplo de realização de join entre duas tabelas
        public PublicSchemaEntity GetWithCategory(int id)
            => _repository.JoinWith<Category>(id, (entity, category) =>
            {
                entity.Category = category;
                return entity;
            });
    }
```

##### Observações
* O método `JoinWith` atualmente retorna apenas um objeto por id. Isso se dá devido a limitação do [FluentMigrator](https://www.nuget.org/packages/FluentMigrator/). Com isso, só funciona em FKs simples e não retorna uma lista com eles. **No código do pacote do migrator já existe a função para tal, mas ela não está exposta. Um próximo passo seria uma contribuição no pacote**.

## Extendendo o Migrations

Para extender o Migrations, criando os próprios sem perder os recursos deste pacote, basta fazer o seguinte: 

```csharp
    [Migration(123456)]
    public class NewMigration : OnlyUpMigration
    {
        public override void Up()
        {
            ...
        }
    }
```

Todos os migrations criados são controlados pela tabela `VersionInfo` no schema default.

<mark>Os números <strong>1</strong> e <strong>2</strong> são utilizados para o pacote para serem executados antes dos outros. Portanto todos os Migrations criados devem conter valores maiores que esses.</mark>

## Melhorias futuras:

* Atualizar versão do Dapper (o pacote do Fluent está com versão desatualizada e conflitando com a mais recente).
* Join retornando lista
* Join com Lambda
* Testes unitários
* Pacote nuget
* Permitir GroupBy

