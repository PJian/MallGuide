using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 商场
    /// </summary>
    public class Mall
    {
        public Mall()
        {
            this.ImgPath = new List<string>();
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _resourceType;

        public int ResourceType
        {
            get { return _resourceType; }
            set { _resourceType = value; }
        }
        private string _moviePath;

        public string MoviePath
        {
            get { return _moviePath; }
            set { _moviePath = value; }
        }
        private List<string> _imgPath;

        public List<string> ImgPath
        {
            get { return _imgPath; }
            set { _imgPath = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Mall))
                return false;
            Mall m = (Mall)obj;
            return m.Name != null && this.Name.Equals(m.Name) && (

                (this.MoviePath == null && m.MoviePath == null) ||
                this.MoviePath.Equals(m.MoviePath)
                )
                && this.Id == m.Id
                && compareIMgPaths(this.ImgPath, m.ImgPath);
        }

        private bool compareIMgPaths(List<string> imgs1, List<string> imgs2)
        {
            if (imgs2.Count != imgs1.Count) return false;
            for (int i = 0; i < imgs1.Count; i++)
            {
                if (!imgs1.ElementAt(i).Equals(imgs2.ElementAt(i)))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
