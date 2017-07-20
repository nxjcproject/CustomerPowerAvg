using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using CustomerPowerAvg.Service.CustomerPowerAvg;

namespace CustomerPowerAvg.Web.UI_CustomerPowerAvg
{
    public partial class PowerAvgConfigure : WebStyleBaseForEnergy.webStyleBase
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
        public static char[] AuthorityControl()
        {
            return mPageOpPermission.ToArray();
        }
        [WebMethod]
        public static string GetPowerAvg(string mOrganizationId)
        {
            DataTable table = PowerAvgConfigureService.GetPowerAvgDataTable(mOrganizationId);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetMaterial(string mOrganizationId)
        {
            DataTable table = PowerAvgConfigureService.GetMaterialDataTable(mOrganizationId);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static int EditPowerAvg(string mEquipmentGroupId, string mOrganizationId, string mItemName, string mFormula, string mMaterial, string mDisplayIndex, string mEnabled)
        {
            int result = PowerAvgConfigureService.EditPowerAvgConfigure(mEquipmentGroupId, mOrganizationId, mItemName, mFormula, mMaterial, mDisplayIndex, mEnabled);
            return result;
        }
        [WebMethod]
        public static int AddPowerAvg(string mOrganizationId, string mItemName, string mFormula, string mMaterial, string mDisplayIndex, string mEnabled)
        {
            int result = PowerAvgConfigureService.AddPowerAvgConfigure(mOrganizationId, mItemName, mFormula, mMaterial, mDisplayIndex, mEnabled);
            return result;
        }
        [WebMethod]
        public static int DeletePowerAvg(string mEquipmentGroupId)
        {
            int result = PowerAvgConfigureService.DeletePowerAvgConfigure(mEquipmentGroupId);
            return result;
        }
    }
}