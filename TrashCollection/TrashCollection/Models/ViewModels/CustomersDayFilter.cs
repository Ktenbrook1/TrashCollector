using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollection.Models.ViewModels
{
    public class CustomersDayFilter
    {
        public List<Customer> Customers { get; set; }
        public string DaySelection { get; set; }
    }
}
