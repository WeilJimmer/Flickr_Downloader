using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flickr_Downloader {
    public class photo_obj {
        bool checked_ = false;
        string title, id, gtitle, gid, url_X;

        public photo_obj(string title, string id, bool checked_) {
            this.title = title;
            this.id = id;
            this.checked_ = checked_;
        }

        public photo_obj() {
            this.title = null;
            this.id = null;
        }

        public bool Checked_ {
            get { return checked_; }
            set { checked_ = value; }
        }
        public string Title {
            get { return title; }
            set { title = value; }
        }
        public string Id {
            get { return id; }
            set { id = value; }
        }
        public string GTitle {
            get { return gtitle; }
            set { gtitle = value; }
        }
        public string GId {
            get { return gid; }
            set { gid = value; }
        }
        public string URL_X {
            get { return url_X; }
            set { url_X = value; }
        }
    }
}
