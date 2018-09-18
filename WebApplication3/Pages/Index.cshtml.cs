using System.Linq;
using WebApplication3.DBContext.Models;
using WebApplication3.Kernel;
using WebApplication3.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication3.Pages
{
    public class IndexModel : PageModel
    {
        private IStudentService svcStudent;
        public IndexModel(IStudentService student)
        {
            svcStudent = student;
        }
        public void OnGet()
        {
            var log = GlobalSetting.GetLog4Net<IndexModel>();
            log.Error("index get method errror");
            log.Info("index get methon infoffff");
            var list = svcStudent.GetQuery().ToList();
            var stdu = svcStudent.Get(1);
            svcStudent.Insert(new Student());
            list = svcStudent.GetQuery().ToList();
            log.Info(list.Count);
        }
    }
}
