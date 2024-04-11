using System.Collections.Generic;

namespace Dapper.Fluent.Domain;

public class Category
{
    public int Id { get; set; }
    public string Description { get; set; }
    public List<CategoryData> Data { get; set; }
}
