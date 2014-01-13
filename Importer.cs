using System;
using System.IO;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlogImport
{
	public class Importer
	{
		public Importer ()
		{
		}

		public static void Start ()
		{
			var imp = new Importer ();
			imp.Source = "/Users/nic/code/website/octo1/source/_posts";
			imp.Go ();
			File.WriteAllText("/Users/nic/Desktop/ghost-import.json", imp.ModelAsJson);

		}

		enum State
		{
			None,
			Header,
			Markdown
		}

		public string Source { get; set; }

		public GhostModel Model { get; set; }

		public string ModelAsJson
		{
			get
			{
				return JsonConvert.SerializeObject (Model, new IsoDateTimeConverter());
			}
		}

		public void Go ()
		{
			Model = new GhostModel ();


			foreach (var file in Directory.GetFiles(Source, "*.markdown"))
			{
				var name = new FileInfo (file).Name;

				var content = File.ReadAllLines (file);

				Console.WriteLine ("processing " + name);


				var state = State.None;
				var currentMarkdown = new StringBuilder ();
				var currentPost = new Post ();

				foreach (var line in content)
				{
					if (line == "---")
					{
						if (state == State.None)
						{
							state = State.Header;
							continue;
						} else if (state == State.Header)
						{
							state = State.Markdown;
							continue;
						}
					}

					if (state == State.Markdown)
					{
						currentMarkdown.AppendLine (line);
					} else if (state == State.Header)
					{
						if (line.StartsWith ("title"))
						{
							currentPost.title = line.Substring ("title: ".Length).Trim ();

							if (currentPost.title.StartsWith ("\""))
							{
								currentPost.title = currentPost.title.Remove (0, 1);
							}
							if (currentPost.title.EndsWith ("\""))
							{
								currentPost.title = currentPost.title.Remove (currentPost.title.Length - 1);
							}


						} else if (line.StartsWith ("date"))
						{
							string dtline = line.Substring ("date: ".Length).Replace ("'", "");
							DateTime dt = DateTime.Now;
							try
							{
								dt = DateTime.ParseExact (dtline, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
							} catch
							{
								dt = DateTime.ParseExact (dtline, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
							}
							currentPost.created_at = dt;
							currentPost.published_at = dt;
							currentPost.updated_at = dt;
						}
					}
				}
				currentPost.slug = MassageSlug (name);
				currentPost.markdown = currentMarkdown.ToString ();
				currentPost.markdown = currentPost.markdown.Replace ("(/uploads", "(/content/images/uploads");

				Model.data.posts.Add (currentPost);
			}

			int id = 100;
			foreach (var post in Model.data.posts)
			{
				post.id = id;
				id++;
			}
		}

		public string MassageSlug (string fullname)
		{
			return (fullname.Substring (0, "2013-14-12-".Length).Replace ("-", "/")) +
			(fullname.Substring ("2013-14-12-".Length)).Replace (".markdown", "");
		}
	}
}

