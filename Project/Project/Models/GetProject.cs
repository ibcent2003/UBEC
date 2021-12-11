using Project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
   
    public class GetProject
    {

        private PROEntities db = new PROEntities();
        //public static ProjectApplication pullProject(PROEntities db, Guid Id)
        //{
        //    var project = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
        //    return project;

        //}

        //public static ProjectApplication pullProject(PROEntities db, Guid Id)
        //{
        //    var project = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
        //    return project;

        //}

        private ProjectApplication getProjectDetails(Guid Id)
        {
            try
            {
                var project = db.ProjectApplication.Where(x => x.TransactionId == Id).FirstOrDefault();
                return project;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

    }
}