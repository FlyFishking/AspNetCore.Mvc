using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        public Startup.IDiTest test { get; set; }
        private ILog log = LogManager.GetLogger(GloableSetting.LoggerRepository.Name, typeof(IndexModel));

        public void OnGet([FromBody] string aa)
        {
            log.Error("fdafda");
            log.Info("fdafdafda");
        }
    }
}
