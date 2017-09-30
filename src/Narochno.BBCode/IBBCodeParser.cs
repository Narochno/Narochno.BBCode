using System;
using System.Collections.Generic;
using System.Text;

namespace Narochno.BBCode
{
    public interface IBBCodeParser
    {
        string ToHtml(string bbCode);
    }
}
