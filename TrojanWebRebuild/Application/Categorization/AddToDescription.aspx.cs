using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using TrojanWebRebuild.Logic;
namespace TrojanWebRebuild.Application.Categorization
{
    public partial class AddToDescription : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawId = Request.QueryString["AttributeID"];
            int AttributeId;
            if (!String.IsNullOrEmpty(rawId) && int.TryParse(rawId, out AttributeId))
            {
                using (VirusDescriptionActions usersVirusDescription = new VirusDescriptionActions())
                {
                    usersVirusDescription.AddToVirus(Convert.ToInt16(rawId));
                }

            }
            else
            {
                Debug.Fail("ERROR : We should never get to AddToDescription.aspx without an AttributeId.");
                throw new Exception("ERROR : It is illegal to load AddToDescription.aspx without setting a AttributeId.");
            }
            Response.Redirect("VirusDescription.aspx");
        }
    }
}