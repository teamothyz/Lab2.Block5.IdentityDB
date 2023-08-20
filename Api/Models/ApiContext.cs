namespace Api.Models
{
    public class ApiContext
    {
        private static readonly AsyncLocal<ApiContext> instance;

        static ApiContext()
        {
            instance = new AsyncLocal<ApiContext>();
        }

        public static ApiContext Current
        {
            get => instance.Value ??= new ApiContext();
            set => instance.Value = value;
        }

        //public string? Email { get; set; } = null;

        //public string? Username { get; set; } = null;

        public string Token { get; set; } = string.Empty;

        public static void Empty()
        {
            if (instance != null) instance.Value = new ApiContext();
        }
    }
}
