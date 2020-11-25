using System.Collections.Generic;

namespace CheapShopWeb
{
    public class Values
    {
        public static List<string> Urls = new List<string>
        {
            /*rde,          */ "https://rde.lt/search_result/lt/word/☼/page/1",
            /*bigbox,       */ "https://bigbox.lt/paieska?search_query=",
            /*pigu,         */ "https://pigu.lt/lt/search?q=",
            /*novastar,     */ "https://novastar.lt/search/?q=",
            /*topocentras,  */ "https://topocentras.lt/catalogsearch/result/?q=",
            /*skytech,      */ "http://skytech.lt/search.php?x=0&y=0&search_in_description=0&pagesize=500&keywords=",
            /*senukai       */ "https://senukai.lt/paieska/?q=",
            /*autoaibe,     */ "https://autoaibe.lt/search/?q=",
            /*eoltas,       */ "https://eoltas.lt/lt_LT/search/☼/2",
            /*ermitazas,    */ "https://ermitazas.lt/index.php?lang=2&cl=search&_artperpage=90&searchparam=",
            /*varle         */ "https://varle.lt/search/?q="
        };

        public static int scraperAmount = 3;
        public static int scraperTimeout = 60;
    }
}