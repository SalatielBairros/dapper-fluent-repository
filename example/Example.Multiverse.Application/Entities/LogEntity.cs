using System;

namespace Dapper.Fluent.Domain
{
    public class LogEntity
    {
        public LogEntity()
        {

        }

        public LogEntity(PublicSchemaEntity entity)
        {
            PublicId = entity.Id;
            IntProperty = entity.IntProperty;
            TextProperty = entity.TextProperty;
            LimitedTextProperty = entity.LimitedTextProperty;
            BooleanProperty = entity.BooleanProperty;
            DateProperty = DateTime.UtcNow;
            DecimalProperty = entity.DecimalProperty;
        }

        public int Id { get; set; }
        public int PublicId { get; set; }
        public int IntProperty { get; set; }
        public string TextProperty { get; set; }
        public string LimitedTextProperty { get; set; }
        public bool BooleanProperty { get; set; }
        public DateTime DateProperty { get; set; }
        public decimal DecimalProperty { get; set; }
    }
}
