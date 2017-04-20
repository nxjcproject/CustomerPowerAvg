using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using CustomerPowerAvg.Service.PowerAvg;

namespace CustomerPowerAvg.Web.UI_CustomerPowerAvg
{
    public partial class CustomerPowerAvg : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
#if DEBUG
                ////////////////////调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf", "zc_nxjc_qtx_tys" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#elif RELEASE
#endif
                this.OrganisationTree_ProductionLine.Organizations = GetDataValidIdGroup("ProductionOrganization");                         //向web用户控件传递数据授权参数
                this.OrganisationTree_ProductionLine.PageName = "CustomerPowerAvg.aspx";   //向web用户控件传递当前调用的页面名称
                this.OrganisationTree_ProductionLine.LeveDepth = 5;
            }
        }
        [WebMethod]
        public static string GetCustomerPower(string mOrganizationId, string startTime, string endTime, string particleSize)
        {
            DataTable table = CustomerPowerAvgService.GetCustomerPowerDataTable(mOrganizationId, startTime, endTime, particleSize);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
    }
}