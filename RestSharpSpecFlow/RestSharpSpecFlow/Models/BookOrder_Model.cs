using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpSpecFlow.Models
{
    public class BookOrder_Model
    {
        public string Id { get; set; }
        public int BookId { get; set; }
        public string CustomerName { get; set; }
        public string CreatedBy { get; set; } 
        public int Quantity { get; set; }
        public long TimeStamp { get; set; }
    }
}
