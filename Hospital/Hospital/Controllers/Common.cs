using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Hospital.Controllers {
	public class Common{
		private static Common instance;

		public static Common Instance{
			get{
				if(instance== null)
					instance= new Common();

				return instance;
			}
		}

		public void ConnnectionCheck(DbContext context){
			if(context.Database.Connection.State!= ConnectionState.Open)
				context.Database.Connection.Open();
		}
	}
}