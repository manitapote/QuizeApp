using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using modelTest.Models;

namespace modelTest.ViewModel
{
    public class QsnGreSat
    {
        public IEnumerable<questionGRE> questionGRE { get; set; }
        public IEnumerable<questionSAT> questionSAT { get; set; }
    }

    public class QsnGreSat_single
    {
        public questionGRE questionGRE { get; set; }
        public questionSAT questionSAT { get; set; }
    }
}