using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace TestGithubSecurity
{
    public class VulnerableCode
    {
        // SQL Injection in multiple methods
        public void DeleteUser(string userId)
        {
            string connStr = "Server=localhost;Database=mydb;User=sa;Password=Pass123;";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "DELETE FROM Users WHERE Id = " + userId; // SQL Injection
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateUser(string userId, string email)
        {
            string connStr = "Server=localhost;Database=mydb;User=sa;Password=Pass123;";
            string query = $"UPDATE Users SET Email = '{email}' WHERE Id = {userId}"; // SQL Injection
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
        }

        // Command Injection
        public void PingHost(string hostname)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "ping";
            process.StartInfo.Arguments = hostname; // Command injection
            process.Start();
        }

        public void CompressFile(string filename)
        {
            string command = $"tar -czf archive.tar.gz {filename}"; // Command injection
            System.Diagnostics.Process.Start("bash", "-c " + command);
        }

        // Path Traversal
        public string ReadUserFile(string filename)
        {
            string basePath = @"C:\Users\Public\";
            string fullPath = basePath + filename; // Path traversal
            return File.ReadAllText(fullPath);
        }

        public void WriteLog(string logFile, string message)
        {
            string path = "/var/log/" + logFile; // Path traversal
            File.AppendAllText(path, message);
        }

        // Weak Cryptography - Multiple instances
        public string MD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash);
            }
        }

        public string SHA1Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash);
            }
        }

        // Hardcoded credentials
        public bool ConnectToDatabase()
        {
            string server = "production-db.example.com";
            string username = "admin";
            string password = "P@ssw0rd123!"; // Hardcoded password
            string connStr = $"Server={server};User={username};Password={password};";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Insecure random for security-sensitive operations
        public string GenerateSessionToken()
        {
            Random random = new Random();
            byte[] tokenBytes = new byte[32];
            random.NextBytes(tokenBytes); // Not cryptographically secure
            return Convert.ToBase64String(tokenBytes);
        }

        public string GeneratePassword()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Weak password generation
        }

        // Null reference bugs
        public void ProcessUser(string username)
        {
            string upperName = username.ToUpper(); // No null check
            Console.WriteLine(upperName);
        }

        public int GetStringLength(string input)
        {
            return input.Length; // Potential null reference
        }

        // Resource leaks
        public void ReadFileNoDispose(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] buffer = new byte[1024];
            fs.Read(buffer, 0, buffer.Length);
            // FileStream never closed or disposed
        }

        public void ConnectNoDispose()
        {
            SqlConnection conn = new SqlConnection("Server=localhost;Database=test;");
            conn.Open();
            // Connection never closed
        }

        // Insecure SSL/TLS
        public void MakeInsecureRequest(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = 
                delegate { return true; }; // Accept all certificates
            
            WebClient client = new WebClient();
            string response = client.DownloadString(url);
        }

        // Integer overflow potential
        public int MultiplyNumbers(int a, int b)
        {
            return a * b; // No overflow checking
        }

        // Array out of bounds
        public void AccessElement(int[] array, int index)
        {
            int value = array[index]; // No bounds check
            Console.WriteLine(value);
        }

        // Division by zero
        public double Divide(double numerator, double denominator)
        {
            return numerator / denominator; // No zero check
        }

        // Information disclosure
        public void HandleError(Exception ex)
        {
            Console.WriteLine("Full error: " + ex.ToString()); // Exposes sensitive info
            File.WriteAllText("error.log", ex.StackTrace); // Logs sensitive data
        }
    }
}
