# Multiverse ORM

Welcome to Multiverse, an Object-Relational Mapping (ORM) library for .NET designed specifically for managing multitenancy within your database schemas using Dapper.

## Overview

Multiverse offers a comprehensive solution for developers working with applications that require multitenancy support. 
Whether you're building a Software as a Service (SaaS) platform or a multi-user application, Multiverse simplifies the complexities of managing multiple tenants within a single database.

## Key features

* **Schema Management**: Seamlessly handle multiple schemas within a single database instance. You also can customize how the schema is injected.
* **Dapper Integration**: Leveraging the simplicity and performance of Dapper for database operations. 
* **SQL Generation**: We use a custom implementation of the [Dommel](https://github.com/henkmollema/Dommel) library added inside our source code to generate SQL isolating the schemas.
* **Migration**: Streamline database schema migrations across multiple tenants using [FluentMigrator](https://fluentmigrator.github.io/)
* **JSON Handling**: Work with JSON data types in tables, like `jsonb` in PostgreSQL. You can create a complex object on your domain entity and map it as a json field.
* **Flexible Configuration**: Configure Multiverse to suit your specific multitenancy requirements or overrite any injection of our implementation to customize yours.
* **Connection Handling**: Handle the database connection, with or without transactions.
* **Automatic Entity Validation**: Allows an entity to be validated (nullable fields or field lenght) before being sent to database, according with what was configurated on mapping.

## Dependências

This library aggregates and customize the following libraries to allow multitenancy with dapper:

* [Dapper](https://www.nuget.org/packages/Dapper)
* [FluentMigrator](https://www.nuget.org/packages/FluentMigrator/)
* [FluentMigrator.Runner](https://www.nuget.org/packages/FluentMigrator.Runner)
* [Dapper.FluentMap](https://www.nuget.org/packages/Dapper.FluentMap)

We currently only support PostgreSQL, but new databases can be implemented. For PostgreSQL, the following dependencies are used:

* [Npgsql](https://www.nuget.org/packages/Npgsql)
* [FluentMigrator.Runner.Postgres](https://www.nuget.org/packages/FluentMigrator.Runner.Postgres)

### About Dommel

We imported part of the code from [Dommel](https://github.com/henkmollema/Dommel) because it required some changes in Cache and SQL generation for allow multischema. 

## Getting Started

### Injecting the library

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        // Add postgres migration and referencing the assembly with the migrations (optional)
        .AddPostgresRepositoryWithMigration(Configuration["ConnectionString"], assembliesWithMappers: typeof(Reference).Assembly)
        // Add the entity map configuration
        .AddMapperConfiguration<MapperConfiguration>()
        // Add the ORM and Migration runner
        .AddDapperORM()
        // Add the multischema option. It is the implementation of ISchema.
        .AddHttpMultiSchema();
    ...
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDapperORMRunner dapper)
{
    // Add the migration mappers
    dapper.AddMappers();
    ...
}
```

### Mapping your entities

To configure your entities as tables, just follow the example below: 

```csharp

    // The map must has this inheritance. Only mapped properties are transformed in columns. The others are ignored.
    public class MyEntityMap: DapperFluentEntityMap<MyEntity>
    {
        
        public MyEntityMap()
            : base()
        {
            // Table name and its schema (optional). You can inform an specific schema or use the tenant one.
            ToTable("myentity");

            // Inform that you want to validate your entity before send it to the database
            WithEntityValidation();   

            // Add a primary key column with identity option.
            MapToColumn(x => x.Id).IsKey().IsIdentity();

            // Add default value
            MapToColumn(x => x.IntProperty).Default(5).NotNull();

            // Limit the size of a string property
            MapToColumn(x => x.LimitedTextProperty).WithLenght(255);
            MapToColumn(x => x.TextProperty).NotNull();

            // The column name can be different from the property
            Map(x => x.DateProperty).ToColumn("datepp");
            Map(x => x.DecimalProperty).ToColumn("decimalpp");
            MapToColumn(x => x.BooleanProperty);
            
            // Save a complex object (list or class) as a json field.
            MapToColumn(x => x.Data).AsJson();

            // Defines current date as default value and ignore the column in select queries.
            MapToColumn(x => x.CreationDate).Ignore().Default(SystemMethods.CurrentDateTime).NotNull();

            // Create a foreign key
            MapToColumn(x => x.CategoryId).ForeignKeyFor<Category>("id");
        }
    }

```

After creating all your mappings, it's necessary to implement `IMapperConfiguration` and configurate all maps that are going to be used:

```csharp
    public class MapperConfiguration : IMapperConfiguration
    {
        private readonly IPostgresSettings _settings;

        public MapperConfiguration(IPostgresSettings settings)
        {
            _settings = settings;
        }

        public void ConfigureMappers()
        {
            // We can use the default schema at the map
            FluentMapping.AddMap(new CategoryMap(_settings.DefaultSchema));                        
            FluentMapping.AddMap(new MyEntity());
        }
    }
```

The created class is used on the injection:

```csharp
services.AddMapperConfiguration<MapperConfiguration>();
```

**All the tables are going to be automaticaly created for each schema when the repository was first created using it.**

## Using the repository

```csharp    
    public class MyEntityRepository
    {
        // Injecting the postgres repository
        private readonly IPostgresRepository<MyEntity> _repository;
        
        public PublicSchemaEntityRepository(IPostgresRepository<MyEntity> repository)
        {
            this._repository = repository;
        }
        
        public void Delete(int id) => _repository.Remove(x => x.Id == id);
        public MyEntity Get(int id) => _repository.Find(x => x.Id == id);
        public IEnumerable<MyEntity> GetAll() => _repository.All();
        public int Insert(MyEntity entity) => _repository.Add(entity);
        public bool Update(MyEntity entity) => _repository.Update(entity);
        
        public MyEntity GetWithCategory(int id)
            => _repository.JoinWith<Category>(id, (entity, category) =>
            {
                entity.Category = category;
                return entity;
            });
        
        public IEnumerable<ViewModelClass> GetWithSQL(int categoryId)
            => _repository.GetData<ViewModelClass>("SELECT * FROM SAMPLEENTITY WHERE CATEGORYID = :CATEGORYID",
            new
            {
                CategoryId = categoryId
            });
    }
```

* The method `JoinWith` currently only returns one object per id because of the limitations of [FluentMigrator](https://www.nuget.org/packages/FluentMigrator/). It's necessary to change the library to use it in a different way.

## Using migrations

To create a new migration, like add a column in a table:

```csharp
    [Migration(123456)]
    public class NewMigration : OnlyUpMigration
    {
        private readonly ISchema _schema;    

        public NewMigration(ISchema schema)
        {
            this._schema = schema;
        }

        public override void Up()
        {
            var map = new MyEntityMap();
            var tablename = map.TableName;
            var schemaName = _schema.GetSchema();
            const string columnName = "details";

            if (!Schema.Schema(schemaName).Table(tablename).Column(columnName).Exists())
                this.Alter.Table(tablename)
                    .AddColumn(columnName)
                    .AsString()
                    .Nullable();
        }
    }
```

All migrations are controlled by the table `migrations` in each schema. However, if you do not use the multischema option, the table `VersionInfo` will be created at the default schema.

<mark>The numbers <strong>1</strong> and <strong>2</strong> are already used for the library, so all migration codes have to be bigger then 2.</mark>

## Contributing
We welcome contributions from the community! Whether it's bug fixes, feature enhancements, or documentation improvements, please feel free to open a pull request. 
We have a few things you can already start contributing:

* Join returning lists
* Join with lambda (similar to EF)
* `PostgresJsonPropertyHandler` uses `NpgsqlConnection.GlobalTypeMapper` that is obsolete.
* Unit tests
* Allow group by
* Change Dommel implementation from static to a more threadsafe implementation.
* Creating a GitHub documentation page
* Implementing other databases

