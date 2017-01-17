using System.Collections.Generic;

namespace ISTatRIT
{
    public class UgMinor
    {
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<string> courses { get; set; }
        public string note { get; set; }
    }

    public class Minors
    {
        public List<UgMinor> UgMinors { get; set; }
    }
}