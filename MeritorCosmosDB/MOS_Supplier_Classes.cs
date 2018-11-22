using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeritorCosmosDB
{
    class MOS_Supplier_Base
    {
        public string DataType { get; set; }        
        public string region { get; set; }
    }

    class MOS_Supplier_Plant: MOS_Supplier_Base
    {
        public string PlantName { get; set; }
        public string BusinessGroup { get; set; }
    }

    class MOS_Supplier_Detail : MOS_Supplier_Base
    {
        public string SupplierName { get; set; }
        public string PlantName { get; set; }
        public string DUNSNumber { get; set; }
    }

    class MOS_Supplier_Rejection : MOS_Supplier_Base
    {
        public string SupplierName { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string PartNumber { get; set; }
    }
}
