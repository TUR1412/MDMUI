using System;

namespace MDMUI.Model
{
    public class EquipmentMaintenance
    {
        public int MaintenanceId { get; set; } // Corresponds to IDENTITY column, usually set by DB
        public string EquipmentId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; }
        public string MaintenancePerson { get; set; } // This is EmployeeId
        public string Description { get; set; }
        public decimal? Cost { get; set; } // Nullable decimal
        public string Result { get; set; }
        public DateTime CreateTime { get; set; } // Usually set by DB default

        // Optional: Add properties for related data if needed later (e.g., EquipmentName, EmployeeName)
    }
} 