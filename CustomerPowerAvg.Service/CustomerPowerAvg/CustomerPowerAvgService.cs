using SqlServerDataAdapter;
using CustomerPowerAvg.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CustomerPowerAvg.Service.PowerAvg
{
    public class CustomerPowerAvgService
    {
        public static DataTable GetCustomerPowerDataTable(string organizationId, string startTime, string endTime, string particleSize)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string mySql = @"SELECT [EquipmentGroupId]
                                    ,[Name]
                                    ,[OrganizationID]
                                    ,[Formula]
                                    ,[MaterialId]
                                    ,[DisplayIndex]
                                    ,[Enabled]
                                FROM [dbo].[equipment_CustomerPowerContrast]
                                where [OrganizationID]=@organizationId
                                    and [Enabled]=1
                                order by [DisplayIndex]";
            SqlParameter sqlParameter = new SqlParameter("@organizationId", organizationId);
            DataTable table = dataFactory.Query(mySql, sqlParameter);
            table.Columns.Add("AvgPower");
            table.Columns.Add("SumPower");
            table.Columns.Add("Production");
            int count = table.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                if (organizationId == "zc_nxjc_qtx_efc")
                {
                    organizationId = "Db_02_01";
                }
                string mformula = table.Rows[i]["Formula"].ToString().Trim();
                TimeSpan ts = Convert.ToDateTime(endTime) - Convert.ToDateTime(startTime);
                int c = (int)(ts.TotalMinutes);
                double mcount = 0;
                double addTime = 0;
                if (particleSize == "tenMinute")
                {
                    mcount = Math.Ceiling((double)c / (double)10);
                     //mcount = c / 10;
                     addTime = 10;
                }
                if (particleSize == "hour")
                {
                    mcount = Math.Ceiling((double)c / (double)60);
                     addTime = 60;
                }
                if (particleSize == "day")
                {
                    mcount = Math.Ceiling((double)c / (double)1440);
                     addTime = 1440;
                }
                double sum_formula = 0.00;
                string m_Time = startTime;
                for (int j = 0; j < mcount; j++)
                {                                              
                    //m_Time = Convert.ToDateTime(m_Time).AddMinutes(10).ToString();               
                    string sql = @"select avg(A.Formula) as Formula from 
                                    (select vDate,{0} as Formula from {1}.[dbo].[HistoryAmmeter]
                                where vDate>=@startTime
                                    and vDate<=@endTime) A
                                where A.vDate>@start_m_Time
                                        and A.vDate<=@m_Time";
                    SqlParameter[] m_para ={
                                    new SqlParameter("@startTime", startTime),
                                    new SqlParameter("@endTime", endTime),
                                    new SqlParameter("@start_m_Time", m_Time),
                                    new SqlParameter("@m_Time", Convert.ToDateTime(m_Time).AddMinutes(10).ToString())
                                };
                    DataTable m_table = dataFactory.Query(string.Format(sql, mformula, organizationId), m_para);
                    string avgFormula = m_table.Rows[0]["Formula"].ToString().Trim();
                    if (avgFormula=="")
                    {                          
                        avgFormula = "0";
                    }
                    double m_formula = Convert.ToDouble(avgFormula);
                    //string sum_formula = m_formula;
                    sum_formula = sum_formula + m_formula;
                    m_Time = Convert.ToDateTime(m_Time).AddMinutes(addTime).ToString();                       
                }
                string sum2_formula =sum_formula.ToString("0.00");
                string lastAvgFormula = (sum_formula / mcount).ToString("0.00");                   
                table.Rows[i]["AvgPower"] = lastAvgFormula;
                table.Rows[i]["SumPower"] = sum2_formula;
                //接下来是查询产量
                string materialId = table.Rows[i]["MaterialId"].ToString().Trim();
                string mSql = @"SELECT [MaterialId]
                              ,[VariableId]
                              ,[Name]
                              ,[KeyID]
                              ,[Type]
                              ,[Unit]
                              ,[MaterialErpCode]
                              ,[TagTableName]
                              ,[Formula]
                              ,[Coefficient]
                              ,[Visible]
                          FROM [NXJC].[dbo].[material_MaterialDetail]
                               where [MaterialId]=@materialId";
                SqlParameter para = new SqlParameter("@materialId", materialId);
                DataTable materialTable = dataFactory.Query(mSql, para);
                string materialFormula = materialTable.Rows[0]["Formula"].ToString();
                string lastSql = @"select cast(sum({0}) as decimal(18,2)) as SumDcs from {1}.[dbo].[HistoryDCSIncrement]
                                    where vDate>=@startTime
                                          and vDate<=@endTime";
                SqlParameter[] lastPara ={
                                     new SqlParameter("@startTime", startTime),
                                     new SqlParameter("@endTime", endTime),
                                 };
                DataTable resultTable = dataFactory.Query(string.Format(lastSql, materialFormula, organizationId), lastPara);
                string production = resultTable.Rows[0]["SumDcs"].ToString();
                table.Rows[i]["Production"] = production;
            }
            return table;
        }
    }
}
