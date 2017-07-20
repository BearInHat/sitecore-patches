# Pipelines

Patch Notes:
```xml
<processor type="Bear.Sitecore.Pipelines.SendEmail.FillCustomHeaders, Sitecore.Analytics" patch:after="processor[@type='Sitecore.EmailCampaign.Cm.Pipelines.SendEmail.FillEmail, Sitecore.EmailCampaign.Cm']"/>
```
or patch after "Sitecore.Support.EmailCampaign.Cm.Pipelines.SendEmail.FillEmail, Sitecore.Support.118405", if you are using this patch.