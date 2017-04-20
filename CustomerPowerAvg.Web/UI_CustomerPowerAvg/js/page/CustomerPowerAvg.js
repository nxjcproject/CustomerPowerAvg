$(function () {
    InitialDate();
    LoadDataGrid("first");
});
function InitialDate() {
    var nowDate = new Date();
    var beforeDate = new Date();
    beforeDate.setDate(nowDate.getDate());
    var nowString = nowDate.getFullYear() + '-' + (nowDate.getMonth() + 1) + '-' + nowDate.getDate() + " " + nowDate.getHours() + ":" + nowDate.getMinutes() + ":" + nowDate.getSeconds();
    var beforeString = beforeDate.getFullYear() + '-' + (beforeDate.getMonth() + 1) + '-' + beforeDate.getDate() + " 00:00:00";
    $('#startTime').datetimebox('setValue', beforeString);
    $('#endTime').datetimebox('setValue', nowString);
}
function onOrganisationTreeClick(node) {
    $('#organizationName').textbox('setText', node.text);
    $('#organizationId').val(node.OrganizationId);
    mOrganizationId = node.OrganizationId;
}
function LoadDataGrid(type, myData) {
    if (type == "first") {
        $('#grid_Main').datagrid({
            columns: [[
                  { field: 'Name', title: '设备名称', width: 100 },
                  { field: 'AvgPower', title: '平均功率', width: 100, align: 'left' },
                  { field: 'SumPower', title: '平均功率之和', width: 100, align: 'left' },
                  { field: 'Production', title: '产量', width: 100, align: 'left' }
            ]],
            fit: true,
            toolbar: "#toorBar",
            idField: 'EquipmentGroupId',
            rownumbers: true,
            singleSelect: true,
            striped: true,
            data: []
        });
    }
    else {
        $('#grid_Main').datagrid('loadData', myData);
    }
}
function Query() {
    var beginTime = $('#startTime').datebox('getValue');
    var endTime = $('#endTime').datebox('getValue');
    var particleSize = $('#particleSize').combobox('getValue');
    var win = $.messager.progress({
        title: '请稍后',
        msg: '数据载入中...'
    });
    $.ajax({
        type: "POST",
        url: "CustomerPowerAvg.aspx/GetCustomerPower",
        data: "{mOrganizationId:'" + mOrganizationId + "',startTime:'" + beginTime + "',endTime:'" + endTime + "',particleSize:'" + particleSize + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.progress('close');
            var myData = jQuery.parseJSON(msg.d);
            if (myData.total == 0) {
                LoadDataGrid("last", []);
                $.messager.alert('提示', '没有查询到记录！');
            } else {
                LoadDataGrid("last", myData);
            }
        },
        beforeSend: function (XMLHttpRequest) {
            //alert('远程调用开始...');
            win;
        },
        error: function () {
            $.messager.progress('close');
            $("#grid_Main").datagrid('loadData', []);
            $.messager.alert('失败', '加载失败！');
        }
    });
}