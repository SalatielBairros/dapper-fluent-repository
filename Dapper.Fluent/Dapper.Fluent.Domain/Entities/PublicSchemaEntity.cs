using System;

namespace Dapper.Fluent.Domain
{
    public class PublicSchemaEntity
    {
        public int Id { get; set; }
        public int IntProperty { get; set; }
        public string TextProperty { get; set; }
        public string LimitedTextProperty { get; set; }
        public bool BooleanProperty { get; set; }
        public DateTime DateProperty { get; set; }
        public decimal DecimalProperty { get; set; }
    }
}
