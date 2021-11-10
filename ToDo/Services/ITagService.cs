using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Services
{
    public interface ITagService
    {
        IEnumerable<string> ReadTags();

        void WriteTags(IEnumerable<string> todoTags);
    }
}
