using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

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
        }

        // SQL Injection vulnerability
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

        // Weak cryptography
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

        // Command injection vulnerability
        public void ExecuteCommand(string userInput)
        {
            System.Diagnostics.Process.Start("cmd.exe", "/c " + userInput);
        }
    }
