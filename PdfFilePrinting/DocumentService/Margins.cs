using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PdfFilePrinting.DocumentService
{
    public struct Margins
    {
        public Margins()
        {

        }
        public Margins(int right, int left, int top, int bottom)
        {
            Right = right;
            Left = left;
            Top = top;
            Bottom = bottom;
        }
        [XmlAttribute("right")]
        public int Right { get; init; }
        [XmlAttribute("left")]
        public int Left { get; init; }
        [XmlAttribute("top")]
        public int Top { get; init; }
        [XmlAttribute("bottom")]
        public int Bottom { get; init; }
    }
}
