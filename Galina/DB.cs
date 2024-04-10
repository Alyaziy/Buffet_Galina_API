using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galina
{
    public static class DB
    {
        private static User01Context _context;
        public static User01Context Instanse
        {
            get
            {
                if(_context == null)
                    _context = new User01Context();
                return _context;
            }
        }
    }
}
