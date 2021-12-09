using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsAPI.Models
{
    public class Comments
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }
    }
}
