﻿using System;

namespace Example_SearchEngine.Model
{
    internal class Contents
    {
        public int Idx { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime? CreateDt { get; set; }
        public string CreateUserNm { get; set; }
        public string Tags { get; set; }
    }
}