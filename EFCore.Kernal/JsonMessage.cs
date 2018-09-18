namespace EFCore.Kernal
{
    public class JsonMessage
    {
        public JsonStatus Status { get; set; }
        public string Message { get; set; }
        public dynamic Model { get; set; }
        public bool Dialog { get; set; }
        public bool Init { get; set; }
        public bool Refresh { get; set; }
    }

    public enum JsonStatus
    {
        Success = 1,
        Failure = 0
    }
}