#r "System.Core.dll"
#r "System.Data.dll"
#r "System.XML.dll"
#r "System.Web.dll"
#r "System.Web.Extensions.dll"

using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

public class Startup
{
    public async Task<object> Invoke(dynamic input)
    {
		// Config Input
		string report = (string)input.report;

		// Init Report Viewer
		Warning[] warnings;
        string[] streamIds;
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;
		ReportViewer viewer = new ReportViewer();
		viewer.LocalReport.Refresh();
		viewer.LocalReport.ReportPath = report; // Filepath
		viewer.LocalReport.EnableExternalImages = true;

		// Parse Parameters
		IDictionary<string, object> parametersRaw = (dynamic)input.parameters;
		ReportParameter[] parameters = new ReportParameter[parametersRaw.Count];
		int i=0;foreach (string name in parametersRaw.Keys)
			parameters[i++] = new ReportParameter(name, (string)parametersRaw[name], true);

		// Set Parameters
		viewer.LocalReport.SetParameters(parameters);

		// Parse and add each Data Set
		viewer.LocalReport.DataSources.Clear();
		foreach (KeyValuePair<string, object> prop in input.data)
		{
			ReportDataSource dataSource = new ReportDataSource();
			dataSource.Name = prop.Key;
			dataSource.Value = buildDataTable(prop.Value);
			viewer.LocalReport.DataSources.Add(dataSource);
		}

		byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
		return bytes;
    }

    public DataTable buildDataTable(dynamic data)
    {
        DataTable dataTable = new DataTable();
        if (data.Length == 0) return dataTable;

        // Find out Properties in ExpandoObject, and Create Columns for each one
        foreach (KeyValuePair<string, object> prop in data[0]) 
            dataTable.Columns.Add(prop.Key, prop.Value.GetType());

        // Loop through each object in the array
        foreach (dynamic o in data)
        {
            DataRow row = dataTable.NewRow();
            // loop through each property in order and set our row columns to the value of the property
            int i = 0; foreach (KeyValuePair<string, object> prop in o) 
                row[i++] = prop.Value;
            // Add Row
            dataTable.Rows.Add(row);
        }

        return dataTable;
    }

}