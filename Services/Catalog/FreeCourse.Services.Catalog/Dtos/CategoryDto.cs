using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Dtos
{
    public class CategoryDto
    {
     
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
