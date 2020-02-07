using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taiwan_AskFaceMaskApp.Models
{
	public class FaceMaskInDrugStore
	{
		[PrimaryKey]
		public string DrugStoreId { get; set; }
		public string Name { get; set; }
		[Ignore]
		public string Address { get; set; }
		[Ignore]
		public string Tel { get; set; }
		public string AdultCount { get; set; }

		public string ChildCount { get; set; }
		public string DataSourceTime { get; set; }
	}
}
