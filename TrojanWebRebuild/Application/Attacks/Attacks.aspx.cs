using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrojanWebRebuild.Models;

namespace TrojanWebRebuild.Application.Attacks
{
    public partial class Attacks : System.Web.UI.Page
    {
        List<string> aList = new List<string> { "Limited - (3)", "Partial - (4)", "Full - (5)" };
        List<string> tList = new List<string> { "Limited - (6)", "Moderate - (7)", "Excessive - (8)" };
        List<string> rList = new List<string> { "Short - (9)", "Medium - (10)", "Long - (11)" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populate();
                aDropList.Items.Insert(0, new ListItem("-- Select an Accessibility Rating --", "0"));
                rDropList.Items.Insert(0, new ListItem("-- Select a Resources Rating --", "0"));
                tDropList.Items.Insert(0, new ListItem("-- Select a Timing Rating --", "0"));
            }
        }
        void populate()
        {
            aDropList.DataSource = aList;
            aDropList.DataBind();

            rDropList.DataSource = rList;
            rDropList.DataBind();

            tDropList.DataSource = tList;
            tDropList.DataBind();
        }
        private void fillVectors()
        {
            //AttackVector A = new AttackVector(1, 1, 1);
            //AttackVector B = new AttackVector(2, 3, 1);
            //AttackVector C = new AttackVector(2, 1, 2);
            AttackVector newAttack = new AttackVector((int)aDropList.SelectedIndex, (int)rDropList.SelectedIndex, (int)tDropList.SelectedIndex);
            List<AttackVector> attacks = new List<AttackVector> { newAttack };
            foreach (AttackVector X in attacks)
            {
                ClientScript.RegisterArrayDeclaration("input", JsonConvert.SerializeObject(X));
            }
        }
        protected void VisualGo_Btn_Click(object sender, EventArgs e)
        {
            fillVectors();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "id", "start()", true);
        }
    }
}