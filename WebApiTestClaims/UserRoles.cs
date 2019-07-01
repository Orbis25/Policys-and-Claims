using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTestClaims
{
    [Table("UserRoles", Schema = "dbo")]
    public class UserRoles
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
