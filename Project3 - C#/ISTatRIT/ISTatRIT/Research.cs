﻿using System.Collections.Generic;

namespace ISTatRIT
{

    public class ByInterestArea
    {
        public string areaName { get; set; }
        public List<string> citations { get; set; }
    }

    public class ByFaculty
    {
        public string facultyName { get; set; }
        public string username { get; set; }
        public List<string> citations { get; set; }
    }

    public class Research
    {
        public List<ByInterestArea> byInterestArea { get; set; }
        public List<ByFaculty> byFaculty { get; set; }
    }

}