using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Services
{
    class TagService : ITagService
    {
        string path = @"C:\01_DATA\Temp\Tags.txt";

        public IEnumerable<string> ReadTags()
        {
            if (!File.Exists(path))
                return Enumerable.Empty<string>();

            var jsonString = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<IEnumerable<string>>(jsonString);

        }

        public void WriteTags(IEnumerable<string> todoTags)
        {
            if (!File.Exists(path))
            {
                using (var file = File.Create(path)) { };
            }

            var jsonString = JsonConvert.SerializeObject(todoTags, Formatting.Indented);

            File.WriteAllText(path, jsonString);


        }

    }
}
