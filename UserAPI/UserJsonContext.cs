using System.Text.Json.Serialization;
using UserAPI;

namespace UserAPI;

[JsonSerializable(typeof(User))]
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class UserJsonContext : JsonSerializerContext
{

}
