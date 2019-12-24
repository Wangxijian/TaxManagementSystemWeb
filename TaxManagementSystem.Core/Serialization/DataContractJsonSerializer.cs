namespace TaxManagementSystem.Core.Serialization
{
    using System;
    using System.IO;
    using System.Text;
    using R = System.Runtime.Serialization.Json;

    public static class DataContractJsonSerializer
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        public static void Serialize(Stream s, object value)
        {
            if (value == null || s == null)
                throw new ArgumentNullException();
            if (!s.CanWrite)
                throw new ArgumentNullException();
            R.DataContractJsonSerializer serializer = new R.DataContractJsonSerializer(value.GetType());
            serializer.WriteObject(s, value);
        }

        public static string Serialize(object value)
        {
            if (value == null)
                throw new ArgumentNullException();
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer.Serialize(ms, value);
                byte[] buffer = ms.ToArray();
                return Encoding.GetString(buffer);
            }
        }

        public static object Deserialize(Stream s, Type type)
        {
            if (s == null || type == null || type == null)
                throw new ArgumentNullException();
            if (!s.CanRead)
                throw new ArgumentException();
            R.DataContractJsonSerializer serializer = new R.DataContractJsonSerializer(type);
            return serializer.ReadObject(s);
        }

        public static T Deserialize<T>(Stream s)
        {
            object value = DataContractJsonSerializer.Deserialize(s, typeof(T));
            if (value == null)
                return default(T);
            return (T)value;
        }

        public static object Deserialize(string json, Type type)
        {
            if (string.IsNullOrEmpty(json) || type == null)
                throw new ArgumentNullException();
            byte[] buffer = Encoding.GetBytes(json);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return DataContractJsonSerializer.Deserialize(ms, type);
            }
        }

        public static T Deserialize<T>(string json)
        {
            object value = DataContractJsonSerializer.Deserialize(json, typeof(T));
            if (value == null)
                return default(T);
            return (T)value;
        }
    }
}
