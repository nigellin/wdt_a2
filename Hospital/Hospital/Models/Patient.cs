//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hospital.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

    public partial class Patient
    {
        public Patient()
        {
            this.Visits = new HashSet<Visit>();
        }
		
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name= "Patient Id")]
        public int		Id { get; set; }

		[Required]
		[Display(Name= "Patient Name")]
		[StringLength(50)]
        public string	Name { get; set; }

		[Required]
		[StringLength(255)]
        public string	Address { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name= "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
		
		[Required]
		[StringLength(10, MinimumLength= 10, ErrorMessage= "Phone number required numbers with 10 digits long")]
		[RegularExpression("\\d+", ErrorMessage= "Phone number cannot contained other than numbers")]
        public string	Phone { get; set; }

		[Required]
		[StringLength(255)]
		[Display(Name= "Emergency Contact")]
        public string	EmergencyContact { get; set; }

		[DataType(DataType.DateTime)]
		[Display(Name= "Date of Registration")]
        public DateTime DateOfRegistration { get; set; }
    
        public virtual ICollection<Visit> Visits { get; set; }
    }
}