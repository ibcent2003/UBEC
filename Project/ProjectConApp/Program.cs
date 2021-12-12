using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DataDBDataContext db = new DataDBDataContext();
            var getData = db.AppData2s.ToList();
            foreach (var d in getData)
            {
                var check = db.AppData2s.Where(x => x.Id == d.Id).FirstOrDefault();
                check.TransactionId = Guid.NewGuid();
                check.ModifiedBy = "ibrahim";
                check.ModifiedDate = DateTime.Now;
                check.IsDeleted = false;
                db.AppDatas.Context.SubmitChanges();
                db.SubmitChanges();
                Console.WriteLine(check.TransactionId + " Has been added...............");
              
            }
            Console.ReadLine();
            Console.WriteLine("Finished...............");
        }
    }
}
