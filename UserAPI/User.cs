using Npgsql;

namespace UserAPI
{
    public class User
    {
        private readonly IConfiguration? Configuration;

        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
        public UInt16 Location { get; set; } = default!;
        public bool Active { get; set; } = default!;

        public User(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public User() 
        { 
        }

        public User(string userId, string userName, string password, string email, ushort location, bool active)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Email = email;
            Location = location;
            Active = active;
        }

        public void Load(string userId = "", string email = "")
        {
            try
            {
                string query = "SELECT userId, userName, email, location, active FROM Users WHERE (userId = @userId OR email = @email)";
                string connStr = Configuration["ConfigurationString"];
                using var conn = new NpgsqlConnection(connStr);
                conn.Open();

                using var cmd = new NpgsqlCommand(query, conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (Enumerable.Range(1, 5).Any(x => reader.IsDBNull(x))) throw new Exception("Not found");

                    UserId = Convert.ToString(reader["userId"]);
                    UserName = Convert.ToString(reader["userName"]);
                    Email = Convert.ToString(reader["email"]);
                    Location = Convert.ToUInt16(reader["location"]);
                    Active = Convert.ToBoolean(reader["active"]);
                }
            }
            catch (Exception ex)
            {
                // Log exception
            }
        }
    }
}
