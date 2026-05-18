using System.Text;

namespace Service.IA.Util
{
    public static class Conversor
    {
        public static HttpContent ConvertJson<t>(t data)
        {
            string json = ConvertStringJson(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static string ConvertStringJson<t>(t data) => System.Text.Json.JsonSerializer.Serialize(data);
    }
}
