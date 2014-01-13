using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BlogImport
{
	public class GhostModel 
	{
		public GhostModel()
		{
			meta = new Metadata ();
			data = new Datablock ();
		}
		public Metadata meta { get; set; }
		public Datablock data { get; set; }
	}

	public class Metadata 
	{
		public Metadata()
		{
			version = "001";
			exported_on = DateTime.Now;
		}

		public DateTime exported_on { get; set; }
		public string version { get; set; }
	}

	public class Datablock 
	{
		public Datablock()
		{
			posts = new List<Post> ();
		}
		public List<Post> posts { get; set; }
	}

	public class Post
	{
		public Post()
		{
			uuid = Guid.NewGuid ();
			image = null;
			featured = 0;
			page = 0;
			status = "published";
			language = "en_US";
			meta_title = null;
			meta_description = null;
			author_id = 1;
			created_by = 1;
			updated_by = 1;
			published_by = 1;
		}
		public int id { get; set; }
		public Guid uuid { get; set; }
		public string title { get; set; }
		public string slug { get; set; }
		public string markdown { get; set; }
		public string html { get; set; }
		public string image { get; set;}
		public int featured { get; set; }
		public int page { get; set; }
		public string status { get; set; }
		public string language { get; set; }
		public string meta_title { get; set; }
		public string meta_description { get; set; }
		public int author_id { get; set; }
		public DateTime created_at { get; set; }
		public int created_by { get; set; }
		public DateTime updated_at { get; set; }
		public int updated_by { get; set; }
		public DateTime published_at { get; set; }
		public int published_by { get; set; }
	}
}

