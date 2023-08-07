using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace letter_of_no_evidence.web.Helper
{
    public static class TempDataExtensions
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }
        public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            var obj = tempData.Peek(key);
            return obj == null ? default : JsonConvert.DeserializeObject<T>((string)obj);
        }

        public static void RemoveObject(this ITempDataDictionary tempData, string key)
        {
            tempData.Remove(key);
        }
    }
}
