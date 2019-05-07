using GenerateToolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateToolApp.Service
{
    public interface IDBTester
    {
        bool TestConnection(DatabaseModel dbModel);
    }
}
