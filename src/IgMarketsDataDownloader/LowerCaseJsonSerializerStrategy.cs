using SimpleJson;

namespace IgMarketsDataDownloader
{
    public class LowerCaseJsonSerializerStrategy : PocoJsonSerializerStrategy
    {
        protected override string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {
            //PascalCase to snake_case
            return clrPropertyName.ToLowerInvariant();
        }
    }
}
