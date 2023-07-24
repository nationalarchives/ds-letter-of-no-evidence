using Newtonsoft.Json;

namespace letter_of_no_evidence.web.Helper
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void RemoveObject(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}
