﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Note
    {
        public Note(DateTime da, string cont, string capt)
        {
            date = da;
            content = cont;
            caption = capt;
        }
        DateTime date;
        public DateTime Date 
        {
            get { return date; }
            set { date = value; }
        }
        string caption;
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}