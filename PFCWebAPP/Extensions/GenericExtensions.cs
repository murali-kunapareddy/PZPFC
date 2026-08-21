using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace PFCWebAPP.Extensions
{
    public static class GenericExtensions
    {
        public static string Serialize(this object obj, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return (jsonSerializerSettings == null) ? JsonConvert.SerializeObject(obj) : JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }

        public static TResponse Deserialize<TResponse>(this string content, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return (jsonSerializerSettings == null) ? JsonConvert.DeserializeObject<TResponse>(content) : JsonConvert.DeserializeObject<TResponse>(content, jsonSerializerSettings);
        }

        public static Dictionary<string, string> AsStringDictionary<T>(this T obj)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj).ToString());
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
