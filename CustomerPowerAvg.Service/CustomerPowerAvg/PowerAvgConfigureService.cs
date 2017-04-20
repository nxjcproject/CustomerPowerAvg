using SqlServerDataAdapter;
using CustomerPowerAvg.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CustomerPowerAvg.Service.CustomerPowerAvg
{
    public class PowerAvgConfigureService
    {
        public static DataTable GetPowerAvgDataTable(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string mySql = @"SELECT A.[EquipmentGroupId]
                                    ,A.[Name]
                                    ,A.[OrganizationID]
                                    ,A.[Formula]
                                    ,A.[MaterialId]
                                    ,A.[DisplayIndex]
                                    ,A.[Enabled]
		                            ,B.[Name]+C.[Name] as [NewName]
                                FROM [dbo].[equipment_CustomerPowerContrast] A,[dbo].[tz_Material] B,[dbo].[material_MaterialDetail] C
                                where A.[OrganizationID]='zc_nxjc_byc_byf'
	                                and A.[MaterialId]=C.[MaterialId]
	                                and B.KeyID=C.KeyID
                                    and A.[Enabled]=1
                                order by [DisplayIndex]";
            SqlParameter sqlParameter = new SqlParameter("@organizationId", organizationId);
            DataTable table = dataFactory.Query(mySql, sqlParameter);
            return table;
        }
        public static DataTable GetMaterialDataTable(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string mySql = @"SELECT A.[OrganizationID]
	                          ,A.[Name] + B.[Name] as [Name]
                              ,A.[CreatedDate]
                              ,A.[Type]
                              ,B.[MaterialId]     
                          FROM [NXJC].[dbo].[tz_Material] A,[NXJC].[dbo].[material_MaterialDetail] B
                          where A.[OrganizationID] like 'zc_nxjc_byc_byf'+'%'
                                and A.KeyID=B.KeyID
		                        and A.[Enable]=1";
            SqlParameter sqlParameter = new SqlParameter("@organizationId", organizationId);
            DataTable table = dataFactory.Query(mySql, sqlParameter);
            return table;
        }
        public static int EditPowerAvgConfigure(string mEquipmentGroupId, string mOrganizationId, string mItemName, string mFormula, string mMaterial, string mDisplayIndex, string mEnabled)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);

            string mySql = @"UPDATE [dbo].[equipment_CustomerPowerContrast]
                           SET [Name]= @mItemName
                            ,[OrganizationID]= @mOrganizationId
                            ,[Formula]= @mFormula
                            ,[MaterialId]= @mMaterial
                            ,[DisplayIndex]= @mDisplayIndex
                            ,[Enabled]= @mEnabled
                         WHERE [EquipmentGroupId] = @mEquipmentGroupId";
            SqlParameter[] para = { new SqlParameter("@mEquipmentGroupId",mEquipmentGroupId),
                                    new SqlParameter("@mItemName",mItemName),
                                    new SqlParameter("@mOrganizationId",mOrganizationId),
                                    new SqlParameter("@mFormula", mFormula),
                                    new SqlParameter("@mMaterial", mMaterial),                                   
                                    new SqlParameter("@mDisplayIndex",  mDisplayIndex),
                                    new SqlParameter("@mEnabled", mEnabled)};
            int dt = factory.ExecuteSQL(mySql, para);
            return dt;
        }
        public static int AddPowerAvgConfigure(string mOrganizationId, string mItemName, string mFormula, string mMaterial, string mDisplayIndex, string mEnabled)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            string mySql = @"INSERT INTO [dbo].[equipment_CustomerPowerContrast]
                                    ([EquipmentGroupId]
                                    ,[Name]
                                    ,[OrganizationID]
                                    ,[Formula]
                                    ,[MaterialId]
                                    ,[DisplayIndex]
                                    ,[Enabled])
                             VALUES
                                   (@mEquipmentGroupId
                                   ,@mItemName
                                   ,@mOrganizationId
                                   ,@mFormula
                                   ,@mMaterial
                                   ,@mDisplayIndex
                                   ,@mEnabled)";
            SqlParameter[] para = { new SqlParameter("@mEquipmentGroupId",System.Guid.NewGuid().ToString()),
                                    new SqlParameter("@mItemName",mItemName),
                                    new SqlParameter("@mOrganizationId",mOrganizationId),
                                    new SqlParameter("@mFormula", mFormula),
                                    new SqlParameter("@mMaterial", mMaterial),
                                    new SqlParameter("@mDisplayIndex",  mDisplayIndex),
                                    new SqlParameter("@mEnabled", mEnabled)};
            int dt = factory.ExecuteSQL(mySql, para);
            return dt;
        }
        public static int DeletePowerAvgConfigure(string mEquipmentGroupId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);

            string mySql = @"delete from [dbo].[equipment_CustomerPowerContrast]
                         WHERE [EquipmentGroupId] =@mEquipmentGroupId";
            SqlParameter para = new SqlParameter("@mEquipmentGroupId", mEquipmentGroupId);
            int dt = factory.ExecuteSQL(mySql, para);
            return dt;
        }
    }
}
