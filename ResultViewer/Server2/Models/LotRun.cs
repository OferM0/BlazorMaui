namespace ResultViewer.Server.Models
{
    public class LotRun
    {
        public Guid Run_Id { get; set; }
        public string Lot_Name { get; set; }

        public string Recipe_Name { get; set; }

        public string Stepper_Id { get; set; }

        public string Operator_Name { get; set; }

        public string Tool_Id { get; set; }

        public float Run_Start_Time { get; set; }

        public float Run_End_Time { get; set; }

        public float Failed_Wafers { get; set; }

        public float Failed_Sites { get; set; }

        public float Lot_Status { get; set; }

        public string Comments { get; set; }

        public string Stepper_Id_2 { get; set; }

        public Guid Recipe_Run_Id { get; set; }

        public float Slot_Map { get; set; }

        public float Iteration { get; set; }

        public float Calibration_Mode { get; set; }

        public float Port_Num { get; set; }

        public float ARP_Flag { get; set; }

        public float TIS_Mode { get; set; }

        public float Access_Method { get; set; }

        public decimal Lied_File_Flag { get; set; }

        public string Control_Job_Id { get; set; }

        public string Process_Job_Id { get; set; }

        public string Carrier_Id { get; set; }

        public string Analysis_Recipe_Name { get; set; }

        public decimal Handling_Mode { get; set; }

        public decimal DS_Iteration_Number { get; set; }

        public Byte Calc_Qmerit { get; set; }

        public Byte Is_ATM_Selected { get; set; }

        public Byte Calc_Qmerit_Layer { get; set; }

        public Byte Modeled_TIS_Run_Enabled { get; set; }

        public Byte Imaging_Accuracy_Metrics { get; set; }
    }
}
