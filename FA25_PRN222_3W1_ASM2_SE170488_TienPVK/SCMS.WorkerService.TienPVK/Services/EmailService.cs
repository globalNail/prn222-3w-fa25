using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SCMS.WorkerService.TienPVK.Services
{
    /// <summary>
    /// Configuration for email settings
    /// </summary>
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = true;
        public List<string> DefaultRecipients { get; set; } = new List<string>();
    }

    /// <summary>
    /// Service for sending emails with attachments using MailKit
    /// </summary>
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(ILogger<EmailService> logger, EmailSettings emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings;
        }

        /// <summary>
        /// Sends an email with optional attachments
        /// </summary>
        /// <param name="recipients">List of email recipients</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body (HTML supported)</param>
        /// <param name="attachmentPaths">Optional paths to files to attach</param>
        /// <param name="isHtml">Whether the body is HTML</param>
        /// <returns>True if email sent successfully, false otherwise</returns>
        public async Task<bool> SendEmailAsync(
            List<string> recipients,
            string subject,
            string body,
            List<string>? attachmentPaths = null,
            bool isHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));

                // Add recipients
                foreach (var recipient in recipients)
                {
                    message.To.Add(MailboxAddress.Parse(recipient));
                }

                message.Subject = subject;

                // Build message body
                var builder = new BodyBuilder();
                if (isHtml)
                {
                    builder.HtmlBody = body;
                }
                else
                {
                    builder.TextBody = body;
                }

                // Add attachments if any
                if (attachmentPaths != null && attachmentPaths.Any())
                {
                    foreach (var filePath in attachmentPaths)
                    {
                        if (File.Exists(filePath))
                        {
                            builder.Attachments.Add(filePath);
                            _logger.LogInformation("Added attachment: {FilePath}", filePath);
                        }
                        else
                        {
                            _logger.LogWarning("Attachment file not found: {FilePath}", filePath);
                        }
                    }
                }

                message.Body = builder.ToMessageBody();

                // Send email
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        _emailSettings.SmtpServer,
                        _emailSettings.SmtpPort,
                        _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

                    // Authenticate if credentials provided
                    if (!string.IsNullOrEmpty(_emailSettings.Username) && !string.IsNullOrEmpty(_emailSettings.Password))
                    {
                        await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                    }

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation("Email sent successfully to {Recipients}. Subject: {Subject}",
                    string.Join(", ", recipients), subject);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email. Subject: {Subject}, Recipients: {Recipients}",
                    subject, string.Join(", ", recipients));
                return false;
            }
        }

        /// <summary>
        /// Sends an email to default recipients from configuration
        /// </summary>
        public async Task<bool> SendEmailToDefaultRecipientsAsync(
            string subject,
            string body,
            List<string>? attachmentPaths = null,
            bool isHtml = true)
        {
            if (_emailSettings.DefaultRecipients == null || !_emailSettings.DefaultRecipients.Any())
            {
                _logger.LogWarning("No default recipients configured");
                return false;
            }

            return await SendEmailAsync(_emailSettings.DefaultRecipients, subject, body, attachmentPaths, isHtml);
        }

        /// <summary>
        /// Sends a report email with Excel attachment
        /// </summary>
        public async Task<bool> SendReportEmailAsync(string excelFilePath, string reportTitle, int recordCount)
        {
            var subject = $"Worker Service Report - {reportTitle} - {DateTime.Now:yyyy-MM-dd HH:mm}";
            
            var body = $@"
                <html>
                <body>
                    <h2>Student Club Management System - Worker Service Report</h2>
                    <p><strong>Report Date:</strong> {DateTime.Now:dddd, MMMM dd, yyyy HH:mm:ss}</p>
                    <p><strong>Report Type:</strong> {reportTitle}</p>
                    <p><strong>Total Records:</strong> {recordCount}</p>
                    <p>Please find the attached Excel file with detailed information.</p>
                    <br/>
                    <p><em>This is an automated email from the Worker Service. Please do not reply.</em></p>
                </body>
                </html>
            ";

            return await SendEmailToDefaultRecipientsAsync(
                subject,
                body,
                new List<string> { excelFilePath },
                isHtml: true);
        }

        /// <summary>
        /// Validates email configuration
        /// </summary>
        public bool ValidateConfiguration()
        {
            if (string.IsNullOrEmpty(_emailSettings.SmtpServer))
            {
                _logger.LogError("SMTP Server is not configured");
                return false;
            }

            if (_emailSettings.SmtpPort <= 0)
            {
                _logger.LogError("SMTP Port is not configured properly");
                return false;
            }

            if (string.IsNullOrEmpty(_emailSettings.SenderEmail))
            {
                _logger.LogError("Sender Email is not configured");
                return false;
            }

            _logger.LogInformation("Email configuration validated successfully");
            return true;
        }
    }
}
