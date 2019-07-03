using SourceCode.EnvironmentLibrary.Management;
using SourceCode.EnvironmentSettings.Client;
using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace K2Documentation.Samples.Management.EnvironmentLibrary
{
	class Program
	{
		static void Main(string[] args)
		{
			AddEditEnvironment();
		}

		private static void AddEditEnvironment()
		{
			EnvironmentSettingsManager environmentSettingsManager;
			string EnvironmentName;
			string EnvironmentDesc;
			string EnvironmentId;
			string TemplateId;
			string TemplateName;
			bool IsDefault;

			try
			{
				// Get values from App.Config to be added or edited.  An empty EnvironmentId creates a new EnvironmentLibrary.  One with a value updates the existing.
				SetEnvironmentValues(out EnvironmentName, out EnvironmentDesc, out EnvironmentId, out TemplateId, out TemplateName, out IsDefault);

				if(string.IsNullOrEmpty(EnvironmentId))  // Creating a new environment library.
				{
					InitializeEnvironmentSettingsManager(out environmentSettingsManager);

					AddEnvironment(environmentSettingsManager, EnvironmentName, EnvironmentDesc, EnvironmentId, TemplateName, IsDefault);

					environmentSettingsManager.Disconnect();

					var managementServer = new EnvironmentLibraryManagementServer(GetConnectionString(), true);

					SaveMappings(EnvironmentId, TemplateId, managementServer);
				}
				else // Editing an existing environment library.
				{
					InitializeEnvironmentSettingsManager(out environmentSettingsManager);

					EditEnvironment(environmentSettingsManager, EnvironmentName, EnvironmentDesc, EnvironmentId, IsDefault);
					environmentSettingsManager.Disconnect();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private static void SaveMappings(string EnvironmentId, string TemplateId, EnvironmentLibraryManagementServer managementServer)
		{
			var objectMappingNames = managementServer.GetObjectManagementNames();
			var templateObjectMappings = managementServer.GetObjectUserMappings(GenerateActionsDataTable(objectMappingNames), TemplateId);
			managementServer.SaveObjectUserMappings(templateObjectMappings, EnvironmentId);
			var templateRoleMappings = managementServer.GetObjectRoleMappings(GenerateActionsDataTable(objectMappingNames), TemplateId);
			managementServer.SaveObjectRoleMappings(templateRoleMappings, EnvironmentId);
		}

		private static void SetEnvironmentValues(out string EnvironmentName, out string EnvironmentDesc, out string EnvironmentId, out string TemplateId, out string TemplateName, out bool IsDefault)
		{
			EnvironmentName = ConfigurationManager.AppSettings["EnvironmentName"];
			EnvironmentDesc = ConfigurationManager.AppSettings["EnvironmentDesc"];
			EnvironmentId = ConfigurationManager.AppSettings["EnvironmentId"];
			TemplateId = ConfigurationManager.AppSettings["TemplateId"];
			TemplateName = ConfigurationManager.AppSettings["TemplateName"];
			IsDefault = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDefault"]);
		}

		private static void AddEnvironment(EnvironmentSettingsManager environmentSettingsManager, string EnvironmentName, string EnvironmentDesc, string EnvironmentId, string TemplateName, bool IsDefault)
		{
			EnvironmentId = System.Guid.NewGuid().ToString();

			// Create a new environment instance with the given values.
			EnvironmentInstance environment = new EnvironmentInstance(EnvironmentId, EnvironmentName, EnvironmentDesc);
			environment.IsDefaultEnvironment = IsDefault;

			InitializeEnvironmentSettingsManager(out environmentSettingsManager);

			var template = environmentSettingsManager.EnvironmentTemplates.GetItemByName(TemplateName);

			// Add new environment to the EnvironmentInstanceCollection
			template.Environments.Add(environment);
		}

		private static void EditEnvironment(EnvironmentSettingsManager environmentSettingsManager, string EnvironmentName, string EnvironmentDesc, string EnvironmentId, bool IsDefault)
		{
			EnvironmentInstance environment = environmentSettingsManager.EnvironmentTemplates.FindEnvironment(EnvironmentId);
			environment.EnvironmentName = EnvironmentName;
			environment.EnvironmentDescription = EnvironmentDesc;
			environment.IsDefaultEnvironment = IsDefault;

			environment.SaveUpdate();
		}

		private static void InitializeEnvironmentSettingsManager(out EnvironmentSettingsManager environmentSettingsManager)
		{
			environmentSettingsManager = new EnvironmentSettingsManager(false);

			environmentSettingsManager.ConnectToServer(GetConnectionString());
			environmentSettingsManager.InitializeSettingsManager();
		}

		private static string GetConnectionString()
		{
			SCConnectionStringBuilder connectionStringBuilder = new SCConnectionStringBuilder
			{
				Host = ConfigurationManager.AppSettings["Host"],
				Port = Convert.ToUInt32(ConfigurationManager.AppSettings["Port"]),
				IsPrimaryLogin = true,
				Integrated = true
			};
			
			return connectionStringBuilder.ConnectionString;
		}

		private static DataTable GenerateActionsDataTable(Dictionary<string, string> objectMappingNames)
		{
			DataTable dtActions = new DataTable();
			dtActions.Columns.Add("UsersGroups");
			foreach(KeyValuePair<string, string> kvp in objectMappingNames)
			{
				dtActions.Columns.Add(kvp.Key, System.Type.GetType("System.Boolean"));
			}
			dtActions.Columns.Add("Type", System.Type.GetType("System.String"));
			return dtActions;
		}
	}
}