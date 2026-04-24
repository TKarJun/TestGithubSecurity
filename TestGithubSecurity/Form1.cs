using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace TestGithubSecurity
{
    public partial class Form1 : Form
    {
        // Hardcoded credentials - Security Issue
        private const string API_KEY = "sk-1234567890abcdefghijklmnopqrstuvwxyz";
        private const string PASSWORD = "Admin@123456";
        private const string CONNECTION_STRING = "Server=myServerAddress;Database=myDataBase;User Id=sa;Password=MyP@ssw0rd123;";
        private const string STRIPE_KEY = "sk_live_51H1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string GITHUB_PAT = "ghp_1234567890abcdefghijklmnopqrstuvwxyz";
        private const string AWS_KEY = "AKIAIOSFODNN7EXAMPLE";
        private const string AWS_SECRET = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";
        
        public Form1()
        {
            InitializeComponent();
            
            // Disable SSL certificate validation - CRITICAL VULNERABILITY
            ServicePointManager.ServerCertificateValidationCallback = 
                delegate { return true; };
        }

        // SQL Injection vulnerability - HIGH SEVERITY
        public void GetUserData(string userId)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                string query = "SELECT * FROM Users WHERE UserId = '" + userId + "'"; // SQL Injection
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteReader();
            }
        }

        // SQL Injection in login - CRITICAL
        public bool Login(string username, string password)
        {
            string query = "SELECT * FROM Users WHERE Username = '" + username + 
                          "' AND Password = '" + password + "'";
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                var result = cmd.ExecuteScalar();
                return result != null;
            }
        }

        // Weak cryptography - MD5 is broken
        public string WeakEncryption(string data)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }

        // Path traversal vulnerability
        public string ReadFile(string filename)
        {
            string path = "C:\\Data\\" + filename; // Path traversal
            return File.ReadAllText(path);
        }

        // Command injection vulnerability - CRITICAL
        public void ExecuteCommand(string userInput)
        {
            System.Diagnostics.Process.Start("cmd.exe", "/c " + userInput);
        }

        // OS Command injection via filename
        public void ProcessFile(string filename)
        {
            string command = "type " + filename;
            System.Diagnostics.Process.Start("cmd.exe", "/c " + command);
        }

        // Unvalidated redirect - Open redirect vulnerability
        public void RedirectToUrl(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        // Hardcoded password in authentication
        public bool AuthenticateAdmin(string password)
        {
            return password == "admin123"; // Hardcoded password
        }

        // Insecure random number generation
        public string GenerateToken()
        {
            Random random = new Random();
            return random.Next().ToString(); // Not cryptographically secure
        }

        // XSS vulnerability - Unvalidated user input
        public void DisplayMessage(string userMessage)
        {
            MessageBox.Show(userMessage); // Could contain malicious content
        }

        // Null pointer dereference
        public void ProcessData(string data)
        {
            string result = data.ToUpper(); // No null check
            Console.WriteLine(result);
        }

        // Resource leak - connection not properly disposed
        public void QueryDatabase(string query)
        {
            SqlConnection conn = new SqlConnection(CONNECTION_STRING);
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            // Connection never closed - resource leak
        }

        // Division by zero potential
        public int Calculate(int a, int b)
        {
            return a / b; // No check for b == 0
        }

        // Array index out of bounds
        public void AccessArray(int index)
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            int value = numbers[index]; // No bounds checking
            Console.WriteLine(value);
        }
    }
