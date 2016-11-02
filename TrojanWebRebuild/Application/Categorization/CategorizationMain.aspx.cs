using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrojanWebRebuild.Logic;
using TrojanWebRebuild.Models;

namespace TrojanWebRebuild.Application.Categorization
{
    public partial class CategorizationMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadDropdown();
                trojanDrpDown.Items.Insert(0, new ListItem("-- Select a Trojan --", "0"));
            }
        }
        TrojanContext db = new TrojanContext();
        public string selectedVirusName;
        public string getSelectedVirus
        {
            get
            {
                return trojanDrpDown.SelectedValue;
            }
        }
        private void loadDropdown()
        {
            try
            {
                trojanDrpDown.DataSource = (from b in db.Virus where (b.userName == HttpContext.Current.User.Identity.Name) select b).ToList();
                trojanDrpDown.DataValueField = "virusNickName";
                trojanDrpDown.DataBind();
            }
            catch (Exception)
            {
                //Do Nothing
                trojanDrpDown.ClearSelection();
            }
        }

        protected void trojanDrpDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            trojanDrpDown.Items.Insert(0, new ListItem("-- Select a Trojan --", "0"));
            int selectd = trojanDrpDown.SelectedIndex;
            if (trojanDrpDown.SelectedIndex != 0)
            {
                selectedVirusName = trojanDrpDown.SelectedValue;
                //Response.Redirect("~/Application/Categorization/Review.aspx");
                Server.Transfer("~/Application/Categorization/Review.aspx", true);
            }
        }

        protected void newVirusBtn_Click(object sender, EventArgs e)
        {
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                usersVirus.setNewVirusId();
            }
            Response.Redirect("~/Application/Categorization/VirusDescription.aspx");
            //Response.Redirect("~/Application/Categorization/AttributeList.aspx");
        }
    }
}