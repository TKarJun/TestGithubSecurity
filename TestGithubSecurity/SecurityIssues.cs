using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace TestGithubSecurity
{
    public class SecurityIssues
    {
        // Hardcoded secrets
        private const string AWS_ACCESS_KEY = "AKIAIOSFODNN7EXAMPLE";
        private const string AWS_SECRET_KEY = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";
        private const string GITHUB_TOKEN = "ghp_1234567890abcdefghijklmnopqrstuvwxyz";
        private const string PRIVATE_KEY = "-----BEGIN RSA PRIVATE KEY-----\nMIIEpAIBAAKCAQEA1234567890\n-----END RSA PRIVATE KEY-----";

        // XXE (XML External Entity) vulnerability
        public void ParseXml(string xmlContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver(); // Vulnerable to XXE
            doc.LoadXml(xmlContent);
        }

        // Insecure deserialization
        public object DeserializeData(byte[] data)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new System.IO.MemoryStream(data))
            {
                return formatter.Deserialize(stream); // Insecure deserialization
            }
        }

        // Weak SSL/TLS configuration
        public void DisableSSLValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = 
                (sender, certificate, chain, sslPolicyErrors) => true; // Disables SSL validation
        }

        // Use of deprecated/insecure random
        public int GenerateRandomNumber()
        {
            Random random = new Random(); // Not cryptographically secure
            return random.Next();
        }

        // Unvalidated redirect
        public void RedirectUser(string url)
        {
            System.Diagnostics.Process.Start(url); // Open redirect vulnerability
        }
    }
}
