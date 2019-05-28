using DZHCodeBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZHCodeBuilder.Service
{
    public interface IDBTester
    {
        bool TestConnection(DBConfigModel dbModel);
    }
}
