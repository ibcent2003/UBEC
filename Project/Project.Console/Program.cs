using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DataDBDataContext db = new DataDBDataContext();
            var getData = db.AppDatas.ToList();
            foreach(var d in getData)
            {
                var check = db.AppDatas.Where(x => x.Id == d.Id).FirstOrDefault();
                check.NewSerialNo = check.SerialNo + check.ProjectType;
                db.SubmitChanges();
                Console.WriteLine(check.SerialNo + check.ProjectType + " Has been added...............");
            }
        }
    }
}
