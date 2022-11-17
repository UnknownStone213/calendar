using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Note
    {
        public Note(DateTime da, string capt, string cont)
        {
            date = da;
            caption = capt;
            content = cont;
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
        public string GetNote()
        {
            return "NOTE " + date.ToString("MM/dd/yyyy") + " " + caption + " " + content;
        }

    }
}
