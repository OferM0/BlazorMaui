namespace ResultViewer.Server.Models
{
    class Result
    {
        public int Id { get; set; }
        public string? ResultName { get; set; }
        public bool Status { get; set; }
        public DateTime Date {  get; set; }
    }
}
