using System;

namespace Dapper.Fluent.Domain
{
    public class SchemaWithCategory : PublicSchemaEntity
    {
        public SchemaWithCategory(PublicSchemaEntity entity, Category category)
        {
            Id = entity.Id;
            IntProperty = entity.IntProperty;
            TextProperty = entity.TextProperty;
            LimitedTextProperty = entity.LimitedTextProperty;
            BooleanProperty = entity.BooleanProperty;
            DateProperty = entity.DateProperty;
            DecimalProperty = entity.DecimalProperty;
            CategoryId = entity.CategoryId;
            Category = category;
        }

        public Category Category { get; set; }
    }
}
