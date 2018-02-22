using Newtonsoft.Json;

namespace VincereSharp
{
    public static class Serialize
    {
        public static string ToJson(this Contact self) => JsonConvert.SerializeObject(self, VincereSharp.Converter.Settings);
    }
}