using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapShopWeb.Repository
{
    public interface IScraperRepository
    {
        void CreateSelectedScrapers();
        void Begin(string query);
        void Kill();
        void TEST();
    }
}
