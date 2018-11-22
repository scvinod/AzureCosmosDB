using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeritorCosmosDB
{
    class MOS_Base
    {
        public string DataType { get; set; }
        public string PlantName { get; set; }
    }

    class MOS_Pant : MOS_Base
    {
        public string Region { get; set; }
        public string Status { get; set; }

    }

    class MOS_Local_Supplier : MOS_Base
    {
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public int DUNSNumber { get; set; }

    }
}
