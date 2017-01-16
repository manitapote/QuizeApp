using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using modelTest.Models;

namespace modelTest.Models
{
    [Table("tblUser")]
    public class person
    {

        [Key]
        public int UserID { get; set; }       
        public string FirstName { get; set; }        
        public string LastName { get; set; }     
        public string Gender { get; set; }       
        public string Email { get; set; }        
        public string passwrd { get; set; }        
        public string Test { get; set; }        
        public int TotalPoint { get; set; }

    }

    public  class question
    {
        [Key]
        public int QsnID { get; set; }
        public string Qsn { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAs { get; set; }
        public string Detail { get; set; }
    }
       
    [Table("tblQuestionGRE")]
    public class questionGRE:question
    {      
    }
    
    [Table("tblQuestionSAT")]
    public class questionSAT : question
    {
        public string OptionE { get; set; }
    }

    public class question_display
    {
        public int id { get; set; }
        public string option { get; set; }
    }    
}