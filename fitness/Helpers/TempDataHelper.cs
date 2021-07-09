using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitness.Helpers
{
    public static class TempDataHelper
    {
        public static void Put<T> (this ITempDataDictionary temData, string key, T value)
            where T: class
        {
            temData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T> (this ITempDataDictionary temData , string key) where T: class
        {
            object o;
            temData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}
