using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models{
	public class Search{
		public string[] Types{
			get{
				return new string[]{"Id", "Name", "Admission Date", "Discharge Date", "Patient Name", "Date of Visit"};
			}
		}
            
        /*property to get and type and keyword */
		[Required]
		public string Type{ get; set; }
		public string Keyword{ get; set; }
	}
}