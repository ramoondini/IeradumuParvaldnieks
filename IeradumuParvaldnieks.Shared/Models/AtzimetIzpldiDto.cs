using System;
using System.Collections.Generic;
using System.Text;

namespace IeradumuParvaldnieks.Shared.Models
{
    // DTO: datu struktūra, lai atzīmētu ieraduma izpildi
    public class AtzimetIzpldiDto
    {
        public DateTime datums { get; set; }
        public string? piezimes { get; set; }  
    }
}
