using System;

using Newtonsoft.Json;


namespace S9.Utility
{
    public static class JSON<T>
    {
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize(string json)
        {
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
    }

   

}
