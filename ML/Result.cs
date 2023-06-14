using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Result
    {
        public bool Correct { get; set; }

        public string? ErrorMessage { get; set; }

        public Exception? Ex { get; set; }

        public object? Object { get; set; }

        public int? sum { get; set; }

        public int? N { get; set; }
        public int? S { get; set; }
        public int? E { get; set; }
        public int? O { get; set; }

        public List<object>? Objects { get; set; }
    }
}
