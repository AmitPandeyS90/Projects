using System.Collections.Generic;

namespace ISTatRIT
{

    public class Year
    {
        public string date { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class Quarter
    {
        public string date { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class Older
    {
        public string date { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class News
    {
        public List<Year> year { get; set; }
        public List<Quarter> quarter { get; set; }
        public List<Older> older { get; set; }
    }

}