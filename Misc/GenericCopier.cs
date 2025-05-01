using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewGameProject.Misc
{
    public static class GenericCopier<T>
    {
        public static T DeepCopy(object objectToCopy)
        {
            string jsonString = JsonSerializer.Serialize(objectToCopy);

            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
