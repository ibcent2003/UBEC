﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project.ConsoleApp
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="UBEC")]
	public partial class DataDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public DataDBDataContext() : 
				base(global::Project.Console.Properties.Settings.Default.UBECConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<AppData> AppDatas
		{
			get
			{
				return this.GetTable<AppData>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AppData")]
	public partial class AppData
	{
		
		private int _Id;
		
		private string _SerialNo;
		
		private string _ProjectType;
		
		private string _NewSerialNo;
		
		private string _Status;
		
		private System.Nullable<double> _WorkFlowId;
		
		private string _Description;
		
		private string _Location;
		
		private string _LGAId;
		
		private System.Nullable<double> _StageOfCompletion;
		
		private string _ContractorId;
		
		private System.Nullable<double> _EnableSum;
		
		private System.Nullable<double> _ProjectTypeId;
		
		public AppData()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.Always, DbType="Int NOT NULL IDENTITY", IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SerialNo", DbType="NVarChar(255)")]
		public string SerialNo
		{
			get
			{
				return this._SerialNo;
			}
			set
			{
				if ((this._SerialNo != value))
				{
					this._SerialNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProjectType", DbType="NVarChar(255)")]
		public string ProjectType
		{
			get
			{
				return this._ProjectType;
			}
			set
			{
				if ((this._ProjectType != value))
				{
					this._ProjectType = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NewSerialNo", DbType="NVarChar(MAX)")]
		public string NewSerialNo
		{
			get
			{
				return this._NewSerialNo;
			}
			set
			{
				if ((this._NewSerialNo != value))
				{
					this._NewSerialNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="NVarChar(255)")]
		public string Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this._Status = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WorkFlowId", DbType="Float")]
		public System.Nullable<double> WorkFlowId
		{
			get
			{
				return this._WorkFlowId;
			}
			set
			{
				if ((this._WorkFlowId != value))
				{
					this._WorkFlowId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(255)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this._Description = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Location", DbType="NVarChar(255)")]
		public string Location
		{
			get
			{
				return this._Location;
			}
			set
			{
				if ((this._Location != value))
				{
					this._Location = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LGAId", DbType="NVarChar(255)")]
		public string LGAId
		{
			get
			{
				return this._LGAId;
			}
			set
			{
				if ((this._LGAId != value))
				{
					this._LGAId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StageOfCompletion", DbType="Float")]
		public System.Nullable<double> StageOfCompletion
		{
			get
			{
				return this._StageOfCompletion;
			}
			set
			{
				if ((this._StageOfCompletion != value))
				{
					this._StageOfCompletion = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ContractorId", DbType="NVarChar(255)")]
		public string ContractorId
		{
			get
			{
				return this._ContractorId;
			}
			set
			{
				if ((this._ContractorId != value))
				{
					this._ContractorId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EnableSum", DbType="Float")]
		public System.Nullable<double> EnableSum
		{
			get
			{
				return this._EnableSum;
			}
			set
			{
				if ((this._EnableSum != value))
				{
					this._EnableSum = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProjectTypeId", DbType="Float")]
		public System.Nullable<double> ProjectTypeId
		{
			get
			{
				return this._ProjectTypeId;
			}
			set
			{
				if ((this._ProjectTypeId != value))
				{
					this._ProjectTypeId = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
