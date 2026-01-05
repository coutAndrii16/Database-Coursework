using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DormitoryManagementSystem.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserInfo> Students { get; set; }
    }
}
