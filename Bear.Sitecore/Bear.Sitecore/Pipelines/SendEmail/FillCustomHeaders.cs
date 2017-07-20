using Sitecore.Analytics.Data;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Data;
using Sitecore.EDS.Core.Dispatch;
using Sitecore.Modules.EmailCampaign.Core.Pipelines;
using Sitecore.Modules.EmailCampaign.Messages;

namespace Bear.Sitecore.Pipelines.SendEmail
{
    /// <summary>
    /// A pipeline process to fill custom headers after the Sitecore FillEmail process has completed.
    /// </summary>
    public class FillCustomHeaders
    {
        protected EmailMessage Email { get; set; }

        public void Process(SendMessageArgs args)
        {
            // Do we have a message to work with?
            if (args.EcmMessage == null)
                return;

            // Was the EmailMessage filled and passed?
            Email = (EmailMessage)args.CustomData["EmailMessage"];
            if (Email == null)
                return;
            
            FillHeaders(args.EcmMessage);
            SetArgs(args);
        }

        private void FillHeaders(IMessage message)
        {
            var htmlMail = (HtmlMailBase) message;
            if (htmlMail != null)
            {
                FillHtmlHeaders(htmlMail);
            }
            else
            {
                var mailMessage = (MailMessageItem) message;
                if (mailMessage == null)
                    return;
                FillMailHeaders(mailMessage);
            }
        }

        private void FillHtmlHeaders(HtmlMailBase htmlMail)
        {
            // Add headers here if the data comes from HtmlMailBase.
			Email.Headers.Set("X-AltTextHeader", htmlMail.AlternateText);
            FillMailHeaders(htmlMail);
        }

        private void FillMailHeaders(MailMessageItem mailMessage)
        {
            // Add headers if the data comes from MailMessageItem
            Email.Headers.Set("X-NameHeader", mailMessage.Name);
			// ...or access data using RecipientId, MessageId, PlanId, etc...
            var contactId = new ShortID(mailMessage.RecipientId).ToString();
            var contactRepository = new ContactRepository();
			var contact = contactRepository.LoadContactReadOnly(contactId);

            if (contact == null) return;
            // Grab what you need and set it.
            var personalInfo = contact.GetFacet<IContactPerso‌​nalInfo>("Personal");
            Email.Headers.Set("X-JobTitleHeader", personalInfo.JobTitle);
        }

        private void SetArgs(SendMessageArgs args)
        {
            // Add Back to the Pipeline and Ensure Our Updates are Received
            args.CustomData["EmailMessage"] = Email;
        }
    }
}
