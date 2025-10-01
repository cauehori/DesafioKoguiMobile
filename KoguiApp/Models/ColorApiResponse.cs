using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoguiApp.Models
{
    public class ColorName
    {
        public string value { get; set; }
    }

    public class ColorApiResponse
    {
        public ColorName name { get; set; }
    }
}
