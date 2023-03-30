﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Ecosystem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Difficulty { get; set; }

        public Ecosystem(int id, string name, string image, double difficulty)
        {
            Id = id;
            Name = name;
            Image = image;
            Difficulty = difficulty;
        }
    }
}
