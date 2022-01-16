using System;
using System.ComponentModel.DataAnnotations;

namespace challenge.Models
{
    //I'm treating this as a Value Object, not an Entity
    public class Compensation
    {
        //Keys
        public string EmployeeId { get; set; }
        public DateTime EffectiveDate { get; set; }

        //Excluded from the PK to disallow duplicate Employee-EffectiveDate pairs
        public decimal Salary { get; set; }

        //Navigation Property
        public Employee Employee { get; set; }
    }
}
