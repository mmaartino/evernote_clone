﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverNoteClone.Model
{
    public class Notebook
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int UserId { get; set; }
        public string Name { get; set; }


    }
}
