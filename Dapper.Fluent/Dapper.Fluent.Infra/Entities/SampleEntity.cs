using System;
using Dapper.Fluent.Repository.Attributes;

namespace Dapper.Fluent.Infra
{
    public class SampleEntity
    {
        [Identity]
        [PrimaryKey]
        public int Id { get; set; }
        public int IntProperty { get; set; }
        public string TextProperty { get; set; }
        public string LimitedTextProperty { get; set; }
        public bool BooleanProperty { get; set; }

        [Column("date")]
        public DateTime DateProperty { get; set; }
        public decimal DecimalProperty { get; set; }
    }
}
